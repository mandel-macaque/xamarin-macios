using System;
using System.Collections.Generic;
using Mono.Cecil;

#nullable enable

namespace Microsoft.MaciOS.Nnyeah.AssemblyComparator {
	public class ComparingVisitor {
		readonly ModuleDefinition earlierModule;
		readonly ModuleDefinition laterModule;
		readonly bool publicOnly;

		public ItemEvents<TypeDefinition> TypeEvents { get; } = new ();
		public ItemEvents<MethodDefinition> MethodEvents { get; } = new ();
		public ItemEvents<FieldDefinition> FieldEvents { get; } = new ();
		public ItemEvents<EventDefinition> EventsEvents { get; } = new ();
		public ItemEvents<PropertyDefinition> PropertyEvents { get; } = new ();

		public ComparingVisitor (ModuleDefinition earlierModule, ModuleDefinition laterModule, bool publicOnly)
		{
			this.earlierModule = earlierModule;
			this.laterModule = laterModule;
			this.publicOnly = publicOnly;
		}

		public void Visit ()
		{
			var earlierElements = ModuleElements.Import (earlierModule, publicOnly);
			if (earlierElements is null)
				throw new Exception (Errors.E0007);
			var laterElements = ModuleElements.Import (laterModule, publicOnly);
			if (laterElements is null)
				throw new Exception (Errors.E0007);
			var reworker = new TypeReworker (earlierModule);
			VisitTypes (reworker, earlierElements, laterElements);
		}

		void VisitTypes (TypeReworker reworker, ModuleElements earlier, ModuleElements later)
		{
			foreach (var typeName in earlier.Types.Keys) {
				if (later.Types.TryGetValue (typeName, out var laterElems)) {
					 TypeEvents.InvokeFound (this, typeName, laterElems.DeclaringType.ToString ());
					 if (earlier.Types.TryGetValue (typeName, out var earlierElems)) {
						 VisitAllMembers (reworker, earlierElems, laterElems);
					 } else {
						 throw new Exception (Errors.E0007);
					 }
				} else {
					TypeEvents.InvokeNotFound (this, typeName);
				}
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
			List<T> earlier, List<T> later, ItemEvents<T> events) where T: notnull
		{
			foreach (var earlierElem in earlier) {
				VisitMember (reworker, earlierElem, later, events);
			}
		}

		void VisitMember<T> (TypeReworker reworker, T elem,
			List<T> later, ItemEvents<T> events) where T: notnull
		{
			var elemSignature = elem.ToString ()!;
			foreach (var late in later) {
				var lateSignature = late.ToString ()!;
				if (elemSignature == lateSignature) {
					events.InvokeFound (this, elemSignature, lateSignature);
					return;
				}
			}
			var remappedSig = RemappedSignature (reworker, elem);
			if (remappedSig == elemSignature) {
				events.InvokeNotFound (this, elemSignature);
				return;
			}
			foreach (var late in later) {
				var lateSignature = late.ToString ()!;
				if (remappedSig == lateSignature) {
					events.InvokeFound (this, elemSignature, lateSignature);
					return;
				}
			}
			events.InvokeNotFound (this, elemSignature);
		}

		static string RemappedSignature<T> (TypeReworker reworker, T elem) =>
			elem switch {
				MethodDefinition method => reworker.ReworkMethod (method).ToString (),
				FieldDefinition field => reworker.ReworkField (field).ToString (),
				EventDefinition @event => reworker.ReworkEvent (@event).ToString (),
				PropertyDefinition property => reworker.ReworkProperty (property).ToString (),
				_ => throw new ArgumentException (nameof (elem))
			};
	}
}
