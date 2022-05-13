using System;
using System.Diagnostics;
using System.Linq;
using Mono.Unix.Native;

#nullable enable

namespace Xharness {
	[AttributeUsage (AttributeTargets.Field)]
	public class LabelAttribute : Attribute {
		public string Label { get; }

		public LabelAttribute (string label)
		{
			Label = label;
		}
	}

	[Flags]
	public enum TestLabel {
		[Label ("none")]
		None = 0,
		[Label ("bcl")]
		Bcl = 1 << 0,
		[Label ("mac")]
		Mac = 1 << 1,
		[Label ("ios")]
		iOS = 1 << 2,
		[Label ("ios-32")]
		iOS32 = 1 << 3,
		[Label ("ios-64")]
		iOS64 = 1 << 4,
		[Label ("ios-extensions")]
		iOSExtension = 1 << 5,
		[Label ("ios-simulator")]
		iOSSimulator = 1 << 6,
		[Label ("old-simulator")]
		OldiOSSimulator = 1 << 7,
		[Label ("device")]
		Device = 1 << 8,
		[Label ("xtro")]
		Xtro = 1 << 9,
		[Label ("cecil")]
		Cecil = 1 << 10,
		[Label ("docs")]
		Docs = 1 << 11,
		[Label ("bcl-xunit")]
		BclXUnit = 1 << 12,
		[Label ("bcl-nunit")]
		BclNUnit = 1 << 13,
		[Label ("mscorlib")]
		Mscorlib = 1 << 14,
		[Label ("non-monotouch")]
		NonMonotouch = 1 << 15,
		[Label ("monotouch")]
		Monotouch = 1 << 16,
		[Label ("dotnet")]
		Dotnet = 1 << 17,
		[Label ("maccatalyst")]
		MacCatalyst = 1 << 18,
		[Label ("tvos")]
		tvOS = 1 << 19,
		[Label ("watchos")]
		watchOS = 1 << 20,
		[Label ("mmp")]
		Mmp = 1 << 21,
		[Label ("msbuild")]
		Msbuild = 1 << 22,
		[Label ("mtouch")]
		Mtouch = 1 << 23,
		[Label ("btouch")]
		Btouch = 1 << 24,
		[Label ("mac-binding-project")]
		MacBindingProject = 1 << 25,
		[Label ("system-permission")]
		SystemPermission = 1 << 26,
		[Label ("all")]
		All = ~None,
	}

	static class TestLabelExtensions {
		public static string GetLabel (this TestLabel self)
		{
			var enumType = typeof(TestLabel);
			var name = Enum.GetName(typeof(TestLabel), self);
			var attr = enumType.GetField(name).GetCustomAttributes(false)
				.OfType<LabelAttribute>().SingleOrDefault();
			return attr.Label;
		}

		public static TestLabel GetLabel (this string self)
		{
			foreach (var l in Enum.GetValues (typeof(TestLabel))) {
				var value = (TestLabel) l;
				if (value.GetLabel () == self) {
					return value;
				}
			}

			throw new InvalidOperationException ($"Unknown label '{self}'");
		}
	}
}
