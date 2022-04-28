using System;
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;

#nullable enable

namespace Microsoft.MaciOS.AssemblyComparator {

	public class TypeElements {
		public List<MethodDefinition> Methods { get; } = new ();
		public List<PropertyDefinition> Properties { get; } = new ();
		public List<FieldDefinition> Fields { get; } = new ();
		public List<EventDefinition> Events { get; } = new ();
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
