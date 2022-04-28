﻿using System;
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;

#nullable enable

namespace Microsoft.MaciOS.AssemblyComparator {
	public class TypeElement<T> where T : IMemberDefinition {
		public TypeElement (T element)
		{
			Element = element;
		}
		public T Element { get; init; }
		public override string ToString ()
		{
			return Element.ToString ();
		}
	}

	public class TypeElements {
		public List<TypeElement<MethodDefinition>> Methods { get; init; } = new List<TypeElement<MethodDefinition>> ();
		public List<TypeElement<PropertyDefinition>> Properties { get; init; } = new List<TypeElement<PropertyDefinition>> ();
		public List<TypeElement<FieldDefinition>> Fields { get; init; } = new List<TypeElement<FieldDefinition>> ();
		public List<TypeElement<EventDefinition>> Events { get; init; } = new List<TypeElement<EventDefinition>> ();
		public TypeDefinition DeclaringType { get; init; }

		public TypeElements (TypeDefinition declaringType)
		{
			DeclaringType = declaringType;
		}

		public override string ToString ()
		{
			return DeclaringType.FullName;
		}
	}
}
