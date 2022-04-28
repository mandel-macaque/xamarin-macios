using System;
using Mono.Cecil;

#nullable enable

namespace Microsoft.MaciOS.AssemblyComparator {
	public class ItemNotFoundEventArgs<T> : EventArgs where T: IMemberDefinition{
		public ItemNotFoundEventArgs (string original)
		{
			Original = original;
		}
		public string Original { get; }
	}

	public class ItemFoundEventArgs<T> : EventArgs where T: IMemberDefinition{
		public ItemFoundEventArgs (string original, string mapped)
		{
			Original = original;
			Mapped = mapped;
		}
		public string Original { get; }
		public string Mapped { get; }
	}
}
