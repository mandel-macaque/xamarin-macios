using System;
using System.Collections.Generic;

namespace Xharness {
	[AttributeUsage(AttributeTargets.Field)]  
	public class LabelAttribute : Attribute {
		public string Label { get; }

		public LabelAttribute (string label)
		{
			Label = label;
		}
	}

	public enum TestLabel {
		None,

		[Label ("bcl")]
		Bcl,
		[Label("bindings")]
		Bindings,
		[Label("bindings-framework")]
		BindingsFramework,
		[Label ("bindings-xcframework")]
		BindingsXcframework,
		[Label ("bgen")]
		Bgen,
		[Label ("cecil")]
		Cecil,
		[Label ("dotnet")]
		Dotnet,
		[Label ("framework")]
		Framework,
		[Label ("fsharp")]
		Fsharp,
		[Label("interdependent-binding-projects")]
		InterdependentBindingProjects,
		[Label ("introspection")]
		Introspection,
		[Label ("install-source")]
		InstallSource,
		[Label ("library-projects")]
		LibraryProjects,
		[Label ("linker")]
		Linker,
		[Label ("mmptest")]
		Mmp,
		[Label ("mononative")]
		Mononative,
		[Label ("monotouch")]
		Monotouch,
		[Label ("msbuild")]
		Msbuild,
		[Label ("mtouch")]
		Mtouch,
		[Label ("generator")]
		Generator,
		[Label ("sampletester")]
		Sampletester,
		[Label ("xammac")]
		Xammac,
		[Label ("xcframework")]
		Xcframework,
	}

	public static class TestLabelExtensions {
		static readonly Lazy<HashSet<string>> validLabels = new (() => {
			var values = new HashSet<string> ();
			var type = typeof(TestLabel);
			foreach (var val in Enum.GetValues (typeof(TestLabel))) {
				var info = type.GetMember(val.ToString());
				if (Attribute.GetCustomAttribute (info[0], typeof (LabelAttribute)) is LabelAttribute attr) {
					values.Add (attr.Label);
				}
			}
			return values;
		});

		public static HashSet<string> ValidLabels (this TestLabel label) => validLabels.Value;

		public static bool IsValidLabel (this string label)
		{
			// get the label pattern, there are two of those
			// run-*-test
			// skip-*-test
			// we also have a few special labels we need to validate that are old (pre-label per test)
			return false;
		}
	}
}
