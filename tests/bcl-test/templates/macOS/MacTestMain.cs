using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
#if XAMCORE_2_0 || __UNIFIED__
using AppKit;
using Foundation;
#else
using MonoMac.AppKit;
using MonoMac.Foundation;
#endif
using BCLTests;
using Xamarin.iOS.UnitTests;
using Xamarin.iOS.UnitTests.NUnit;
using BCLTests.TestRunner.Core;
using Xamarin.iOS.UnitTests.XUnit;
using System.IO;
using NUnit.Framework.Internal.Filters;

namespace Xamarin.Mac.Tests
{
	static class MainClass
	{
		static int Main (string[] args)
		{
			NSApplication.Init();
			return RunTests (args);
		}

		internal static IEnumerable<TestAssemblyInfo> GetTestAssemblies ()
 		{
			// var t = Path.GetFileName (typeof (ActivatorCas).Assembly.Location);
			foreach (var name in RegisterType.TypesToRegister.Keys) {
				var a = RegisterType.TypesToRegister [name].Assembly;
				yield return new TestAssemblyInfo (a, a.Location);
			}
 		}
 		
		static int RunTests (string [] original_args)
		{
			Console.WriteLine ("Running tests");
			var options = ApplicationOptions.Current;

			// we generate the logs in two different ways depending if the generate xml flag was
			// provided. If it was, we will write the xml file to the tcp writer if present, else
			// we will write the normal console output using the LogWriter
			var logger = new LogWriter (Console.Out); 
			logger.MinimumLogLevel = MinimumLogLevel.Info;
			var testAssemblies = GetTestAssemblies ();
			var runner = RegisterType.IsXUnit ? (TestRunner) new XUnitTestRunner (logger) : new NUnitTestRunner (logger);
			var categories = RegisterType.IsXUnit ?
				new List<string> { 
					"failing",
					"nonmonotests",
					"outerloop",
					"nonosxtests"
				} :
				new List<string> {
					"MacNotWorking",
					"MobileNotWorking",
					"NotOnMac",
					"NotWorking",
					"ValueAdd",
					"CAS",
					"InetAccess",
					"NotWorkingLinqInterpreter"
				};

			if (RegisterType.IsXUnit) {
				// special case when we are using the xunit runner,
				// there is a trait we are not interested in which is
				// the Benchmark one
				var xunitRunner = runner as XUnitTestRunner;
				xunitRunner.AddFilter (XUnitFilter.CreateTraitFilter ("Benchmark", "true", true));
			}
			
			runner.SkipCategories (categories);
			var skippedTests = IgnoreFileParser.ParseContentFiles (NSBundle.MainBundle.ResourcePath);
			if (skippedTests.Any ()) {
				// ensure that we skip those tests that have been passed via the ignore files
				runner.SkipTests (skippedTests);
			}
			runner.Run (testAssemblies.ToList ());

			if (options.ResultFile != null) {
				using (var writer = new StreamWriter (options.ResultFile)) {
					runner.WriteResultsToFile (writer);
				}
				logger.Info ($"Xml result can be found {options.ResultFile}");
			}
			
			logger.Info ($"Tests run: {runner.TotalTests} Passed: {runner.PassedTests} Inconclusive: {runner.InconclusiveTests} Failed: {runner.FailedTests} Ignored: {runner.FilteredTests}");
			return runner.FailedTests != 0 ? 1 : 0;
		}
	}
}
