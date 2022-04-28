﻿using System;
using System.Collections.Generic;
using Microsoft.MaciOS.Nnyeah;
using Mono.Cecil;

#nullable enable

namespace Microsoft.MaciOS.AssemblyComparator {
	public class ComparingVisitor {
		ModuleDefinition EarlierModule, LaterModule;
		bool PublicOnly;

		public ItemEvents<TypeDefinition> TypeEvents { get; } = new ();
		public ItemEvents<MethodDefinition> MethodEvents { get; } = new ();
		public ItemEvents<FieldDefinition> FieldEvents { get; } = new ();
		public ItemEvents<EventDefinition> EventsEvents { get; } = new ();
		public ItemEvents<PropertyDefinition> PropertyEvents { get; } = new ();

		public ComparingVisitor (ModuleDefinition earlierModule, ModuleDefinition laterModule, bool publicOnly)
		{
			EarlierModule = earlierModule;
			LaterModule = laterModule;
			PublicOnly = publicOnly;
		}

		public void Visit ()
		{
			var earlierElements = ModuleElements.Import (EarlierModule, PublicOnly);
			if (earlierElements is null)
				throw new Exception (Errors.E0007);
			var laterElements = ModuleElements.Import (LaterModule, PublicOnly);
			if (laterElements is null)
				throw new Exception (Errors.E0007);
			var reworker = new TypeReworker (EarlierModule);
			VisitTypes (reworker, earlierElements, laterElements);
		}

		void VisitTypes (TypeReworker reworker, ModuleElements earlier, ModuleElements later)
		{
			foreach (var typeName in earlier.Types.Keys) {
				if (!later.Types.TryGetValue (typeName, out var laterElems)) {
					TypeEvents.InvokeNotFound (this, typeName);
					continue;
				} else {
					TypeEvents.InvokeFound (this, typeName, laterElems.DeclaringType.ToString ());
				}
				if (!earlier.Types.TryGetValue (typeName, out var earlierElems)) {
					throw new Exception (Errors.E0007);
				}
				VisitAllMembers (reworker, earlierElems, laterElems);
			}
		}

		void VisitAllMembers (TypeReworker reworker, TypeElements earlier, TypeElements later)
		{
			VisitMembers (reworker, earlier.Methods, later.Methods, MethodEvents);
			VisitMembers (reworker, earlier.Fields, later.Fields, FieldEvents);
			VisitMembers (reworker, earlier.Events, later.Events, EventsEvents);
			VisitMembers (reworker, earlier.Properties, later.Properties, PropertyEvents);
		}

		void VisitMembers<T> (TypeReworker reworker,
			List<TypeElement<T>> earlier, List<TypeElement<T>> later, ItemEvents<T> events) where T : IMemberDefinition
		{
			foreach (var earlierElem in earlier) {
				VisitMember (reworker, earlierElem, later, events);
			}
		}

		void VisitMember<T> (TypeReworker reworker, TypeElement<T> elem,
			List<TypeElement<T>> later, ItemEvents<T> events) where T : IMemberDefinition
		{
			foreach (var late in later) {
				if (elem.Signature == late.Signature) {
					events.InvokeFound (this, elem.Signature, late.Signature);
					return;
				}
			}
			var remappedSig = RemappedSignature (reworker, elem.Element);
			if (remappedSig == elem.Signature) {
				events.InvokeNotFound (this, elem.Signature);
				return;
			}
			foreach (var late in later) {
				if (remappedSig == late.Signature) {
					events.InvokeFound (this, elem.Signature, late.Signature);
					return;
				}
			}
			events.InvokeNotFound (this, elem.Signature);
		}

		static string RemappedSignature<T> (TypeReworker reworker, T elem) where T : IMemberDefinition
		{
			switch (elem) {
			case MethodDefinition method:
				return reworker.ReworkMethod (method).ToString ();
			case FieldDefinition field:
				return reworker.ReworkField (field).ToString ();
			case EventDefinition @event:
				return reworker.ReworkEvent (@event).ToString ();
			case PropertyDefinition property:
				return reworker.ReworkProperty (property).ToString ();
			default:
				throw new ArgumentException (nameof (elem));
			}
		}
	}
}
