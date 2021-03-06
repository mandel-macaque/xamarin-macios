using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Xharness.BCLTestImporter;
using Xharness.BCLTestImporter.Templates;
using Xharness.BCLTestImporter.Xamarin;

namespace Xharness.Tests.BCLTestImporter.Tests {


	// test the class so that we ensure that we do call the template object and that we are correctly creating the
	// default projects.
	public class BCLTestProjectGeneratorTest
	{
		string outputdir;
		AssemblyLocator assemblyLocator;
		Mock<ITemplatedProject> template;
		BCLTestProjectGenerator generator;

		[SetUp]
		public void SetUp ()
		{
			outputdir = Path.GetTempFileName ();
			File.Delete (outputdir);
			Directory.CreateDirectory (outputdir);
			assemblyLocator = new AssemblyLocator ();
			template = new Mock<ITemplatedProject> ();
			generator = new BCLTestProjectGenerator (outputdir) {
				AssemblyLocator = assemblyLocator,
				TemplatedProject = template.Object
			};
		}

		[TearDown]
		public void TearDown ()
		{
			if (Directory.Exists (outputdir))
				Directory.Delete (outputdir, false);
			template = null;
		}

		[Test]
		public void ConstructorNullOutputDir ()
		{
			Assert.Throws<ArgumentNullException> (() => new BCLTestProjectGenerator (null));
			Assert.Throws<ArgumentNullException> (() => new BCLTestProjectGenerator (null, ""));
		}
		
		[Test]
		public void ConstructorNullMonoDir () => 
			Assert.Throws<ArgumentNullException> (() => new BCLTestProjectGenerator ("", null));

		[Test]
		public void iOSMonoSDKPathGetterTest ()
		{
			assemblyLocator.iOSMonoSDKPath = "/path/to/ios/sdk";
			Assert.AreEqual (assemblyLocator.iOSMonoSDKPath, generator.iOSMonoSDKPath);
		}

		[Test]
		public void iOSMonoSDKPathSetterTest ()
		{
			generator.iOSMonoSDKPath = "/path/to/ios/sdk";
			Assert.AreEqual (generator.iOSMonoSDKPath, assemblyLocator.iOSMonoSDKPath);
		}

		[Test]
		public void MacMonoSDKPathGetterTest ()
		{
			assemblyLocator.MacMonoSDKPath = "/path/to/mac/sdk";
			Assert.AreEqual (assemblyLocator.MacMonoSDKPath, generator.MacMonoSDKPath);
		}

		[Test]
		public void MacMonoSDKPathSetterTest ()
		{
			generator.MacMonoSDKPath = "/path/to/mac/sdk";
			Assert.AreEqual (generator.MacMonoSDKPath, assemblyLocator.MacMonoSDKPath);
		}

		[Test]
		public async Task GenerateTestProjectsAsyncTest ()
		{
			var projects = new List<BclTestProject> () {
				new BclTestProject  {
					Name = "First project",
					XUnit = false,
				},
				new BclTestProject {
					Name = "Second project",
					XUnit = true,
				},
			};
			var infos = new List<BclTestProjectInfo> {
				new BclTestProjectInfo {
					Name = "First project",
				},
				new BclTestProjectInfo {
					Name = "Second project",
				}
			};
			template.Setup (t => t.GenerateTestProjectsAsync (It.IsAny<IEnumerable<BclTestProjectInfo>> (), It.IsAny<Platform> ())).Returns (() => {
				return Task.FromResult (projects);
			});
			var result = await generator.GenerateTestProjectsAsync (infos, Platform.iOS);
			Assert.AreEqual (projects.Count, result.Count);
			template.Verify (t => t.GenerateTestProjectsAsync (It.IsAny<IEnumerable<BclTestProjectInfo>> (), It.IsAny<Platform> ()));
		}
	}
}