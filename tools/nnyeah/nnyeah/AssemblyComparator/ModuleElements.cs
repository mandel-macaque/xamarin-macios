﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MaciOS.Nnyeah;
using Mono.Cecil;

#nullable enable

namespace Microsoft.MaciOS.AssemblyComparator {
	public class ModuleElements {
		public Dictionary<string, TypeElements> Types { get; init; } = new Dictionary<string, TypeElements> ();
		ModuleElements ()
		{
		}

		public static ModuleElements Import (ModuleDefinition module, bool publicOnly)
		{
			var moduleElements = new ModuleElements ();
			var typeStack = new Stack<TypeElements> ();

			var visitor = new ModuleVisitor (module, publicOnly);

			visitor.TypeVisited += (s, e) => {
				if (e.Kind == VisitKind.Start) {
					typeStack.Push (new TypeElements (e.Type));
				} else if (e.Kind == VisitKind.End) {
					var typeElements = typeStack.Pop ();
					moduleElements.Types.Add (typeElements.DeclaringType.FullName, typeElements);
				} else {
					throw new Exception (String.Format(Errors.E0008, e.Kind.ToString()));
				}
			};

			visitor.FieldVisited += (s, e) => {
				if (e.Kind == VisitKind.Start) {
					var typeElements = typeStack.Peek ();
					var member = e.Member;
					typeElements.Fields.Add (member);
				}
			};

			visitor.EventVisited += (s, e) => {
				if (e.Kind == VisitKind.Start) {
					var typeElements = typeStack.Peek ();
					var member = e.Member;
					typeElements.Events.Add (member);
				}
			};

			visitor.PropertyVisited += (s, e) => {
				if (e.Kind == VisitKind.Start) {
					var typeElements = typeStack.Peek ();
					var member = e.Member;
					typeElements.Properties.Add (member);
				}
			};

			visitor.MethodVisited += (s, e) => {
				if (e.Kind == VisitKind.Start) {
					var typeElements = typeStack.Peek ();
					var member = e.Member;
					typeElements.Methods.Add (member);
				}
			};

			visitor.Visit ();
			if (typeStack.Count != 0) {
				throw new Exception (Errors.E0009);
			}

			return moduleElements;
		}
	}
}
