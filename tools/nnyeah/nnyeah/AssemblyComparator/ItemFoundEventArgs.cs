﻿using System;
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

	public class ItemEvents<T> where T : IMemberDefinition {
		public EventHandler<ItemNotFoundEventArgs<T>> NotFound = (s, e) => { };
		public EventHandler<ItemFoundEventArgs<T>> Found = (s, e) => { };

		public void InvokeFound (object sender, string original, string mapped) =>
			Found.Invoke (sender, new(original, mapped));

		public void InvokeNotFound (object sender, string original) =>
			NotFound (sender, new(original));
	}
}
