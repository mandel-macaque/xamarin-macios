﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xharness.BCLTestImporter.Templates.Managed {

	// template project that uses the Xamarin.iOS and Xamarin.Mac frameworks
	// to create a testing application for given xunit and nunit test assemblies
	public class XamariniOSTemplate : ITemplatedProject {
		// vars that contain the different keys used in the templates
		internal static readonly string ProjectGuidKey = "%PROJECT GUID%";
		internal static readonly string NameKey = "%NAME%";
		internal static readonly string ReferencesKey = "%REFERENCES%";
		internal static readonly string RegisterTypeKey = "%REGISTER TYPE%";
		internal static readonly string ContentKey = "%CONTENT RESOURCES%";
		internal static readonly string PlistKey = "%PLIST PATH%";
		internal static readonly string WatchOSTemplatePathKey = "%TEMPLATE PATH%";
		internal static readonly string WatchOSCsporjAppKey = "%WATCH APP PROJECT PATH%";
		internal static readonly string WatchOSCsporjExtensionKey = "%WATCH EXTENSION PROJECT PATH%";
		internal static readonly string TargetFrameworkVersionKey = "%TARGET FRAMEWORK VERSION%";
		internal static readonly string TargetExtraInfoKey = "%TARGET EXTRA INFO%";
		internal static readonly string DefineConstantsKey = "%DEFINE CONSTANTS%";
		internal static readonly string DownloadPathKey = "%DOWNLOAD PATH%";

		// resource related static vars used to copy the embedded src to the hd
		static string srcResourcePrefix = "Xharness.BCLTestImporter.Templates.Managed.Resources.src.";
		static string registerTemplateResourceName = "RegisterType.cs";
		static string [] [] srcDirectories = new [] {
			new [] { "common", },
			new [] { "common", "TestRunner" },
			new [] { "common", "TestRunner", "Core" },
			new [] { "common", "TestRunner", "NUnit" },
			new [] { "common", "TestRunner", "xUnit" },
			new [] { "iOSApp" },
			new [] { "macOS" },
			new [] { "today" },
			new [] { "tvOSApp" },
			new [] { "watchOS" },
			new [] { "watchOS", "App" },
			new [] { "watchOS" ,"App", "Images.xcassets" },
			new [] { "watchOS", "App", "Images.xcassets", "AppIcons.appiconset" },
			new [] { "watchOS", "App", "Resources" },
			new [] { "watchOS", "App", "Resources", "Images.xcassets" },
			new [] { "watchOS", "App", "Resources", "Images.xcassets", "AppIcons.appiconset" },
			new [] { "watchOS", "Container" },
			new [] { "watchOS", "Container", "Resources" },
			new [] { "watchOS", "Container", "Resources", "Images.xcassets" },
			new [] { "watchOS", "Container", "Resources", "Images.xcassets", "AppIcons.appiconset" },
			new [] { "watchOS", "Extension" }
		};

		static readonly Dictionary<Platform, string> plistTemplateMatches = new Dictionary<Platform, string> {
			{Platform.iOS, "Managed.iOS.plist.in"},
			{Platform.TvOS, "Managed.tvOS.plist.in"},
			{Platform.WatchOS, "Managed.watchOS.plist.in"},
			{Platform.MacOSFull, "Managed.macOS.plist.in"},
			{Platform.MacOSModern, "Managed.macOS.plist.in"},
		};
		static readonly Dictionary<Platform, string> projectTemplateMatches = new Dictionary<Platform, string> {
			{Platform.iOS, "Managed.iOS.csproj.in"},
			{Platform.TvOS, "Managed.tvOS.csproj.in"},
			{Platform.WatchOS, "Managed.watchOS.csproj.in"},
			{Platform.MacOSFull, "Managed.macOS.csproj.in"},
			{Platform.MacOSModern, "Managed.macOS.csproj.in"},
		};
		static readonly Dictionary<WatchAppType, string> watchOSProjectTemplateMatches = new Dictionary<WatchAppType, string>
		{
			{ WatchAppType.App, "Managed.watchOS.App.csproj.in"},
			{ WatchAppType.Extension, "Managed.watchOS.Extension.csproj.in"}
		};

		static readonly Dictionary<WatchAppType, string> watchOSPlistTemplateMatches = new Dictionary<WatchAppType, string> {
			{WatchAppType.App, "Managed.watchOS.App.plist.in"},
			{WatchAppType.Extension, "Managed.watchOS.Extension.plist.in"}
		};

		public string OutputDirectoryPath { get; set; }
		string GeneratedCodePathRoot => Path.Combine (OutputDirectoryPath, "generated");
		public string IgnoreFilesRootDirectory { get; set; }
		public IAssemblyLocator AssemblyLocator { get; set; } 
		public IProjectFilter ProjectFilter { get; set; }
		public ITestAssemblyDefinitionFactory AssemblyDefinitionFactory { get; set; }

		public Func<string, Guid> GuidGenerator { get; set; }

		// helpers that will return the destination of the different templates once writtne locally
		string WatchContainerTemplatePath => Path.Combine (OutputDirectoryPath, "templates", "watchOS", "Container").Replace ("/", "\\");
		string WatchAppTemplatePath => Path.Combine (OutputDirectoryPath, "templates", "watchOS", "App").Replace ("/", "\\");
		string WatchExtensionTemplatePath => Path.Combine (OutputDirectoryPath, "templates", "watchOS", "Extension").Replace ("/", "\\");

		Stream GetTemplateStream (string templateName)
		{
			var resources = GetType ().Assembly.GetManifestResourceNames ();
			var name = GetType ().Assembly.GetManifestResourceNames ().Where (a => a.EndsWith (templateName, StringComparison.Ordinal)).FirstOrDefault ();
			return GetType ().Assembly.GetManifestResourceStream (name);
		}

		public Stream GetPlistTemplate (Platform platform) => GetTemplateStream (plistTemplateMatches [platform]);

		public Stream GetPlistTemplate (WatchAppType appType) => GetTemplateStream (watchOSPlistTemplateMatches [appType]);

		public Stream GetProjectTemplate (Platform platform) => GetTemplateStream (projectTemplateMatches [platform]);

		public Stream GetProjectTemplate (WatchAppType appType) => GetTemplateStream (watchOSProjectTemplateMatches [appType]);

		public Stream GetRegisterTypeTemplate () => GetTemplateStream (registerTemplateResourceName);

		void BuildSrcTree (string srcOuputPath)
		{
			// loop over the known paths, and build them accordingly
			foreach (var components in srcDirectories) {
				var completePathComponents = new [] { srcOuputPath }.Concat (components).ToArray ();
				var path = Path.Combine (completePathComponents);
				Directory.CreateDirectory (path);
			}
		}

		/// <summary>
		/// Decides what would be the final path of the src resources in the tree that will be used as the source of
		/// the scaffold
		/// </summary>
		/// <param name="srcOuputPath">The dir where we want to ouput the src.</param>
		/// <param name="resourceFullName">The resource full name.</param>
		/// <returns></returns>
		string CalculateDestinationPath (string srcOuputPath, string resourceFullName)
		{
			// we do know that we don't care about our prefix
			var resourceName = resourceFullName.Substring (srcResourcePrefix.Length);
			// icon sets are special, they have a dot, which is also a dot in the resources :/
			var iconSetSubPath = "Images.xcassets.AppIcons.appiconset";
			int lastIndex = resourceName.LastIndexOf (iconSetSubPath);
			if (lastIndex >= 0) {
				var partialPath = Path.Combine (resourceName.Substring (0, lastIndex).Replace ('.', Path.DirectorySeparatorChar), "Images.xcassets", "AppIcons.appiconset");
				// substring up to the iconset path, replace . for PathSeparator, add icon set + name
				resourceName = Path.Combine (partialPath, resourceName.Substring (partialPath.Length + 1));
			} else {
				// replace all . for the path separator, since that is how resource names are built
				lastIndex = resourceName.LastIndexOf ('.');
				if (resourceFullName.Contains (".designer.cs"))
					lastIndex = resourceName.LastIndexOf (".designer.cs");
				if (lastIndex > 0) {
					var partialPath = resourceName.Substring (0, lastIndex).Replace ('.', Path.DirectorySeparatorChar);
					resourceName = partialPath + resourceName.Substring (partialPath.Length);
				}
			}
			return Path.Combine (srcOuputPath, resourceName);
		}

		/// <summary>
		/// Returns the path to be used to store the project file depending on the platform.
		/// </summary>
		/// <param name="projectName">The name of the project being generated.</param>
		/// <param name="platform">The supported platform by the project.</param>
		/// <returns>The final path to which the project file should be written.</returns>
		internal string GetProjectPath (string projectName, Platform platform)
		{
			switch (platform) {
			case Platform.iOS:
				return Path.Combine (OutputDirectoryPath, $"{projectName}.csproj");
			case Platform.TvOS:
				return Path.Combine (OutputDirectoryPath, $"{projectName}-tvos.csproj");
			case Platform.WatchOS:
				return Path.Combine (OutputDirectoryPath, $"{projectName}-watchos.csproj");
			case Platform.MacOSFull:
				return Path.Combine (OutputDirectoryPath, $"{projectName}-mac-full.csproj");
			case Platform.MacOSModern:
				return Path.Combine (OutputDirectoryPath, $"{projectName}-mac-modern.csproj");
			default:
				return null;
			}
		}

		/// <summary>
		/// Returns the path to be used to store the project file depending on the type of watchOS
		/// app that is generated.
		/// </summary>
		/// <param name="projectName">The name of the project being generated.</param>
		/// <param name="appType">The typoe of watcOS application.</param>
		/// <returns>The final path to which the project file should be written.</returns>
		internal string GetProjectPath (string projectName, WatchAppType appType)
		{
			switch (appType) {
			case WatchAppType.App:
				return Path.Combine (OutputDirectoryPath, $"{projectName}-watchos-app.csproj");
			default:
				return Path.Combine (OutputDirectoryPath, $"{projectName}-watchos-extension.csproj");
			}
		}

		/// <summary>
		/// Returns the path to be used to store the projects plist file depending on the platform.
		/// </summary>
		/// <param name="rootDir">The root dir to use.</param>
		/// <param name="platform">The platform that is supported by the project.</param>
		/// <returns>The final path to which the plist should be written.</returns>
		internal static string GetPListPath (string rootDir, Platform platform)
		{
			switch (platform) {
			case Platform.iOS:
				return Path.Combine (rootDir, "Info.plist");
			case Platform.TvOS:
				return Path.Combine (rootDir, "Info-tv.plist");
			case Platform.WatchOS:
				return Path.Combine (rootDir, "Info-watchos.plist");
			case Platform.MacOSFull:
			case Platform.MacOSModern:
				return Path.Combine (rootDir, "Info-mac.plist");
			default:
				return Path.Combine (rootDir, "Info.plist");
			}
		}

		/// <summary>
		/// Returns the path to be used to store the projects plist file depending on the watch application type.
		/// </summary>
		/// <param name="rootDir">The root dir to use.</param>
		/// <param name="appType">The watchOS application path whose plist we want to generate.</param>
		/// <returns></returns>
		internal static string GetPListPath (string rootDir, WatchAppType appType)
		{
			switch (appType) {
			case WatchAppType.App:
				return Path.Combine (rootDir, "Info-watchos-app.plist");
			default:
				return Path.Combine (rootDir, "Info-watchos-extension.plist");
			}
		}

		public async Task GenerateSource (string srcOuputPath)
		{
			// mk the expected directories
			if (Directory.Exists (srcOuputPath))
				Directory.Delete (srcOuputPath, true); // delete, we always want to add the embeded src
			BuildSrcTree (srcOuputPath);
			// the code is simple, we are going to look for all the resources that we know are src and will write a
			// copy of the stream in the designated output path
			var resources = GetType ().Assembly.GetManifestResourceNames ().Where (a => a.StartsWith (srcResourcePrefix));

			// we need to be smart, since the resource name != the path
			foreach (var r in resources) {
				var path = CalculateDestinationPath (srcOuputPath, r);
				// copy the stream
				using (var sourceReader = GetType ().Assembly.GetManifestResourceStream (r))
				using (var destination = File.Create (path)) {
					await sourceReader.CopyToAsync (destination);
				}
			}
		}

		static void WriteReferenceFailure (StringBuilder sb, string failureMessage)
		{
			sb.AppendLine ($"<!-- Failed to load all assembly references; please 'git clean && make' the tests/ directory to re-generate the project files -->");
			sb.AppendLine ($"<!-- Failure message: {failureMessage} --> ");
			sb.AppendLine ("<Reference Include=\"ProjectGenerationFailure_PleaseRegenerateProjectFiles.dll\" />"); // Make sure the project fails to build.
		}

		// creates the reference node
		internal static string GetReferenceNode (string assemblyName, string hintPath = null)
		{
			// lets not complicate our life with Xml, we just need to replace two things
			if (string.IsNullOrEmpty (hintPath)) {
				return $"<Reference Include=\"{assemblyName}\" />";
			} else {
				// the hint path is using unix separators, we need to use windows ones
				hintPath = hintPath.Replace ('/', '\\');
				var sb = new StringBuilder ();
				sb.AppendLine ($"<Reference Include=\"{assemblyName}\" >");
				sb.AppendLine ($"<HintPath>{hintPath}</HintPath>");
				sb.AppendLine ("</Reference>");
				return sb.ToString ();
			}
		}

		internal static string GetRegisterTypeNode (string registerPath)
		{
			var sb = new StringBuilder ();
			sb.AppendLine ($"<Compile Include=\"{registerPath}\">");
			sb.AppendLine ($"<Link>{Path.GetFileName (registerPath)}</Link>");
			sb.AppendLine ("</Compile>");
			return sb.ToString ();
		}

		internal static string GetContentNode (string resourcePath)
		{
			var fixedPath = resourcePath.Replace ('/', '\\');
			var sb = new StringBuilder ();
			sb.AppendLine ($"<Content Include=\"{fixedPath}\">");
			sb.AppendLine ($"<Link>{Path.GetFileName (resourcePath)}</Link>");
			sb.AppendLine ("<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>");
			sb.AppendLine ("</Content>");
			return sb.ToString ();
		}

		string GenerateIncludeFilesNode (string projectName, (string FailureMessage, List<(string assembly, string hintPath)> Assemblies) info, Platform platform)
		{
			// all the data is provided by the filter, if we have no filter, return an empty string so that the project does not have any
			// includes.
			if (ProjectFilter == null)
				return string.Empty;
			var contentFiles = new StringBuilder ();
			foreach (var path in ProjectFilter.GetIgnoreFiles (projectName, info.Assemblies, platform)) {
				contentFiles.Append (GetContentNode (path));
			}
			// add the files that contain the traits/categoiries info
			foreach (var path in ProjectFilter.GetTraitsFiles (platform)) {
				contentFiles.Append (GetContentNode (path));
			}
			return contentFiles.ToString ();
		}

		public async Task<List<BclTestProject>> GenerateTestProjectsAsync (IEnumerable<BclTestProjectInfo> projects, Platform platform)
		{
			// generate the template c# code before we create the diff projects
			await GenerateSource (Path.Combine (OutputDirectoryPath, "templates"));
			var result = new List<BclTestProject> ();
			switch (platform) {
			case Platform.WatchOS:
				result = await GenerateWatchOSTestProjectsAsync (projects);
				break;
			case Platform.iOS:
			case Platform.TvOS:
				result = await GenerateiOSTestProjectsAsync (projects, platform);
				break;
			case Platform.MacOSFull:
			case Platform.MacOSModern:
				result = await GenerateMacTestProjectsAsync (projects, platform);
				break;
			}
			return result;
		}

		#region Watch Porjects generation

		string GenerateWatchProject (string projectName, string template, string infoPlistPath)
		{
			var result = template.Replace (NameKey, projectName);
			result = result.Replace (WatchOSTemplatePathKey, WatchContainerTemplatePath);
			result = result.Replace (PlistKey, infoPlistPath);
			result = result.Replace (WatchOSCsporjAppKey, GetProjectPath (projectName, WatchAppType.App).Replace ("/", "\\"));
			return result;
		}

		async Task<string> GenerateWatchAppAsync (string projectName, Stream template, string infoPlistPath)
		{
			using (var reader = new StreamReader (template)) {
				var result = await reader.ReadToEndAsync ();
				result = result.Replace (NameKey, projectName);
				result = result.Replace (WatchOSTemplatePathKey, WatchAppTemplatePath);
				result = result.Replace (PlistKey, infoPlistPath);
				result = result.Replace (WatchOSCsporjExtensionKey, GetProjectPath (projectName, WatchAppType.Extension).Replace ("/", "\\"));
				return result;
			}
		}

		async Task<string> GenerateWatchExtensionAsync (string projectName, Stream template, string infoPlistPath, string registerPath, (string FailureMessage, List<(string assembly, string hintPath)> Assemblies) info)
		{
			var rootAssembliesPath = AssemblyLocator.GetAssembliesRootLocation (Platform.WatchOS).Replace ("/", "\\");
			var sb = new StringBuilder ();
			if (!string.IsNullOrEmpty (info.FailureMessage)) {
				WriteReferenceFailure (sb, info.FailureMessage);
			} else {
				foreach (var assemblyInfo in info.Assemblies) {
					if (ProjectFilter == null || !ProjectFilter.ExcludeDll (Platform.WatchOS, assemblyInfo.assembly))
						sb.AppendLine (GetReferenceNode (assemblyInfo.assembly, assemblyInfo.hintPath));
				}
			}

			using (var reader = new StreamReader (template)) {
				var result = await reader.ReadToEndAsync ();
				result = result.Replace (DownloadPathKey, rootAssembliesPath);
				result = result.Replace (NameKey, projectName);
				result = result.Replace (WatchOSTemplatePathKey, WatchExtensionTemplatePath);
				result = result.Replace (PlistKey, infoPlistPath);
				result = result.Replace (RegisterTypeKey, GetRegisterTypeNode (registerPath));
				result = result.Replace (ReferencesKey, sb.ToString ());
				result = result.Replace (ContentKey, GenerateIncludeFilesNode (projectName, info, Platform.WatchOS));
				return result;
			}
		}

		// internal implementations that generate each of the diff projects
		async Task<List<BclTestProject>> GenerateWatchOSTestProjectsAsync (IEnumerable<BclTestProjectInfo> projects)
		{
			var projectPaths = new List<BclTestProject> ();
			foreach (var def in projects) {
				// each watch os project requires 3 different ones:
				// 1. The app
				// 2. The container
				// 3. The extensions
				// TODO: The following is very similar to what is done in the iOS generation. Must be grouped
				var projectDefinition = new BCLTestProjectDefinition (def.Name, AssemblyLocator, AssemblyDefinitionFactory, def.assemblies, def.ExtraArgs);
				if (ProjectFilter != null && ProjectFilter.ExludeProject (projectDefinition, Platform.WatchOS)) // if it is ignored, continue
					continue;

				if (!projectDefinition.Validate ())
					throw new InvalidOperationException ("xUnit and NUnit assemblies cannot be mixed in a test project.");
				var generatedCodeDir = Path.Combine (GeneratedCodePathRoot, projectDefinition.Name, "watch");
				if (!Directory.Exists (generatedCodeDir)) {
					Directory.CreateDirectory (generatedCodeDir);
				}
				var registerTypePath = Path.Combine (generatedCodeDir, "RegisterType.cs");
				string failure = null;
				string rootProjectPath = GetProjectPath (projectDefinition.Name, Platform.WatchOS); ;
				try {
					// create the plist for each of the apps
					var projectData = new Dictionary<WatchAppType, (string plist, string project)> ();
					foreach (var appType in new [] { WatchAppType.Extension, WatchAppType.App }) {
						(string plist, string project) data;
						var plist = await BCLTestInfoPlistGenerator.GenerateCodeAsync (GetPlistTemplate (appType), projectDefinition.Name);
						data.plist = GetPListPath (generatedCodeDir, appType);
						using (var file = new StreamWriter (data.plist, false)) { // false is do not append
							await file.WriteAsync (plist);
						}

						string generatedProject;
						switch (appType) {
						case WatchAppType.App:
							generatedProject = await GenerateWatchAppAsync (projectDefinition.Name, GetProjectTemplate (appType), data.plist);
							break;
						default:
							var info = projectDefinition.GetAssemblyInclusionInformation (Platform.WatchOS);
							generatedProject = await GenerateWatchExtensionAsync (projectDefinition.Name, GetProjectTemplate (appType), data.plist, registerTypePath, info);
							failure ??= info.FailureMessage;
							break;
						}
						data.project = GetProjectPath (projectDefinition.Name, appType);
						using (var file = new StreamWriter (data.project, false)) { // false is do not append
							await file.WriteAsync (generatedProject);
						}

						projectData [appType] = data;
					} // foreach app type

					var rootPlist = await BCLTestInfoPlistGenerator.GenerateCodeAsync (GetPlistTemplate (Platform.WatchOS), projectDefinition.Name);
					var infoPlistPath = GetPListPath (generatedCodeDir, Platform.WatchOS);
					using (var file = new StreamWriter (infoPlistPath, false)) { // false is do not append
						await file.WriteAsync (rootPlist);
					}

					using (var file = new StreamWriter (rootProjectPath, false)) // false is do not append
					using (var reader = new StreamReader (GetProjectTemplate (Platform.WatchOS))) {
						var template = await reader.ReadToEndAsync ();
						var generatedRootProject = GenerateWatchProject (def.Name, template, infoPlistPath);
						await file.WriteAsync (generatedRootProject);
					}
					var typesPerAssembly = projectDefinition.GetTypeForAssemblies (AssemblyLocator.GetAssembliesRootLocation (Platform.iOS), Platform.WatchOS);
					var registerCode = await RegisterTypeGenerator.GenerateCodeAsync (typesPerAssembly,
						projectDefinition.IsXUnit, GetRegisterTypeTemplate ());
					using (var file = new StreamWriter (registerTypePath, false)) { // false is do not append
						await file.WriteAsync (registerCode);
					}

					failure ??= typesPerAssembly.FailureMessage;
				} catch (Exception e) {
					failure = e.Message;
				}
				// we have the 3 projects we depend on, we need the root one, the one that will be used by harness
				projectPaths.Add (new BclTestProject { Name = projectDefinition.Name, Path = rootProjectPath, XUnit = projectDefinition.IsXUnit, ExtraArgs = projectDefinition.ExtraArgs, Failure = failure, TimeoutMultiplier = def.TimeoutMultiplier });
			} // foreach project

			return projectPaths;
		}

		#endregion

		#region iOS project genertion

		/// <summary>
		/// Generates an iOS project for testing purposes. The generated project will contain the references to the
		/// mono test assemblies to run.
		/// </summary>
		/// <param name="projectName">The name of the project under generation.</param>
		/// <param name="registerPath">The path to the code that register the types so that the assemblies are not linked.</param>
		/// <param name="info">The list of assemblies to be added to the project and their hint paths.</param>
		/// <param name="templatePath">A path to the template used to generate the path.</param>
		/// <param name="infoPlistPath">The path to the info plist of the project.</param>
		/// <returns></returns>
		async Task<string> GenerateAsync (string projectName, string registerPath, (string FailureMessage, List<(string assembly, string hintPath)> Assemblies) info, Stream template, string infoPlistPath, Platform platform)
		{
			var downloadPath = AssemblyLocator.GetAssembliesRootLocation (platform).Replace ("/", "\\");
			// fix possible issues with the paths to be included in the msbuild xml
			infoPlistPath = infoPlistPath.Replace ('/', '\\');
			var sb = new StringBuilder ();
			if (!string.IsNullOrEmpty (info.FailureMessage)) {
				WriteReferenceFailure (sb, info.FailureMessage);
			} else {
				foreach (var assemblyInfo in info.Assemblies) {
					if (ProjectFilter == null || !ProjectFilter.ExcludeDll (Platform.iOS, assemblyInfo.assembly))
						sb.AppendLine (GetReferenceNode (assemblyInfo.assembly, assemblyInfo.hintPath));
				}
			}

			var projectGuid = GuidGenerator?.Invoke (projectName) ?? Guid.NewGuid ();
			using (var reader = new StreamReader (template)) {
				var result = await reader.ReadToEndAsync ();
				result = result.Replace (DownloadPathKey, downloadPath);
				result = result.Replace (ProjectGuidKey, projectGuid.ToString ().ToUpperInvariant ());
				result = result.Replace (NameKey, projectName);
				result = result.Replace (ReferencesKey, sb.ToString ());
				result = result.Replace (RegisterTypeKey, GetRegisterTypeNode (registerPath));
				result = result.Replace (PlistKey, infoPlistPath);
				result = result.Replace (ContentKey, GenerateIncludeFilesNode (projectName, info, platform));
				return result;
			}
		}

		async Task<List<BclTestProject>> GenerateiOSTestProjectsAsync (IEnumerable<BclTestProjectInfo> projects, Platform platform)
		{
			if (platform == Platform.WatchOS)
				throw new ArgumentException (nameof (platform));
			if (!projects.Any ()) // return an empty list
				return new List<BclTestProject> ();
			var projectPaths = new List<BclTestProject> ();
			foreach (var def in projects) {
				if (def.assemblies.Length == 0)
					continue;
				var projectDefinition = new BCLTestProjectDefinition (def.Name, AssemblyLocator, AssemblyDefinitionFactory, def.assemblies, def.ExtraArgs);
				if (ProjectFilter != null && ProjectFilter.ExludeProject (projectDefinition, Platform.WatchOS)) // if it is ignored, continue
					continue;

				if (!projectDefinition.Validate ())
					throw new InvalidOperationException ("xUnit and NUnit assemblies cannot be mixed in a test project.");
				// generate the required type registration info
				var generatedCodeDir = Path.Combine (GeneratedCodePathRoot, projectDefinition.Name, (platform == Platform.iOS) ? "ios" : "tv");
				if (!Directory.Exists (generatedCodeDir)) {
					Directory.CreateDirectory (generatedCodeDir);
				}
				var registerTypePath = Path.Combine (generatedCodeDir, "RegisterType.cs");

				string projectPath = GetProjectPath (projectDefinition.Name, platform);
				string failure = null;
				try {
					var plist = await BCLTestInfoPlistGenerator.GenerateCodeAsync (GetPlistTemplate (platform), projectDefinition.Name);
					var infoPlistPath = GetPListPath (generatedCodeDir, platform);
					using (var file = new StreamWriter (infoPlistPath, false)) { // false is do not append
						await file.WriteAsync (plist);
					}

					var info = projectDefinition.GetAssemblyInclusionInformation (platform);
					var generatedProject = await GenerateAsync (projectDefinition.Name, registerTypePath, info, GetProjectTemplate (platform), infoPlistPath, platform);
					using (var file = new StreamWriter (projectPath, false)) { // false is do not append
						await file.WriteAsync (generatedProject);
					}
					var typesPerAssembly = projectDefinition.GetTypeForAssemblies (AssemblyLocator.GetAssembliesRootLocation (platform), platform);
					var registerCode = await RegisterTypeGenerator.GenerateCodeAsync (typesPerAssembly,
						projectDefinition.IsXUnit, GetRegisterTypeTemplate ());

					using (var file = new StreamWriter (registerTypePath, false)) { // false is do not append
						await file.WriteAsync (registerCode);
					}

					failure = failure ?? info.FailureMessage;
					failure = failure ?? typesPerAssembly.FailureMessage;
				} catch (Exception e) {
					failure = e.Message;
				}
				projectPaths.Add (new BclTestProject { Name = projectDefinition.Name, Path = projectPath, XUnit = projectDefinition.IsXUnit, ExtraArgs = projectDefinition.ExtraArgs, Failure = failure, TimeoutMultiplier = def.TimeoutMultiplier });
			} // foreach project

			return projectPaths;
		}
		#endregion

		#region Mac OS project generation

		async Task<string> GenerateMacAsync (string projectName, string registerPath, (string FailureMessage, List<(string assembly, string hintPath)> Assemblies) info, Stream template, string infoPlistPath, Platform platform)
		{
			var downloadPath = Path.Combine (AssemblyLocator.GetAssembliesRootLocation (platform), "mac-bcl", platform == Platform.MacOSFull ? "xammac_net_4_5" : "xammac").Replace ("/", "\\");
			infoPlistPath = infoPlistPath.Replace ('/', '\\');
			var sb = new StringBuilder ();
			if (!string.IsNullOrEmpty (info.FailureMessage)) {
				WriteReferenceFailure (sb, info.FailureMessage);
			} else {
				foreach (var assemblyInfo in info.Assemblies) {
					if (ProjectFilter == null || !ProjectFilter.ExcludeDll (platform, assemblyInfo.assembly))
						sb.AppendLine (GetReferenceNode (assemblyInfo.assembly, assemblyInfo.hintPath));
				}
			}

			var projectGuid = GuidGenerator?.Invoke (projectName) ?? Guid.NewGuid ();
			using (var reader = new StreamReader (template)) {
				var result = await reader.ReadToEndAsync ();
				result = result.Replace (DownloadPathKey, downloadPath);
				result = result.Replace (ProjectGuidKey, projectGuid.ToString ().ToUpperInvariant ());
				result = result.Replace (NameKey, projectName);
				result = result.Replace (ReferencesKey, sb.ToString ());
				result = result.Replace (RegisterTypeKey, GetRegisterTypeNode (registerPath));
				result = result.Replace (PlistKey, infoPlistPath);
				result = result.Replace (ContentKey, GenerateIncludeFilesNode (projectName, info, platform));
				switch (platform) {
				case Platform.MacOSFull:
					result = result.Replace (TargetFrameworkVersionKey, "v4.5.2");
					result = result.Replace (TargetExtraInfoKey,
						"<UseXamMacFullFramework>true</UseXamMacFullFramework>");
					result = result.Replace (DefineConstantsKey, "XAMCORE_2_0;ADD_BCL_EXCLUSIONS;XAMMAC_4_5");
					break;
				case Platform.MacOSModern:
					result = result.Replace (TargetFrameworkVersionKey, "v2.0");
					result = result.Replace (TargetExtraInfoKey,
						"<TargetFrameworkIdentifier>Xamarin.Mac</TargetFrameworkIdentifier>");
					result = result.Replace (DefineConstantsKey, "XAMCORE_2_0;ADD_BCL_EXCLUSIONS;MOBILE;XAMMAC");
					break;
				}
				return result;
			}
		}

		async Task<List<BclTestProject>> GenerateMacTestProjectsAsync (IEnumerable<BclTestProjectInfo> projects, Platform platform)
		{
			var projectPaths = new List<BclTestProject> ();
			foreach (var def in projects) {
				if (!def.assemblies.Any ())
					continue;
				var projectDefinition = new BCLTestProjectDefinition (def.Name, AssemblyLocator, AssemblyDefinitionFactory, def.assemblies, def.ExtraArgs);
				if (ProjectFilter != null && ProjectFilter.ExludeProject (projectDefinition, platform))
					continue;

				if (!projectDefinition.Validate ())
					throw new InvalidOperationException ("xUnit and NUnit assemblies cannot be mixed in a test project.");
				// generate the required type registration info
				var generatedCodeDir = Path.Combine (GeneratedCodePathRoot, projectDefinition.Name, "mac");
				Directory.CreateDirectory (generatedCodeDir);
				var registerTypePath = Path.Combine (generatedCodeDir, "RegisterType-mac.cs");

				var typesPerAssembly = projectDefinition.GetTypeForAssemblies (AssemblyLocator.GetAssembliesRootLocation  (platform), platform);
				var registerCode = await RegisterTypeGenerator.GenerateCodeAsync (typesPerAssembly,
					projectDefinition.IsXUnit, GetRegisterTypeTemplate ());

				using (var file = new StreamWriter (registerTypePath, false)) { // false is do not append
					await file.WriteAsync (registerCode);
				}
				var projectPath = GetProjectPath (projectDefinition.Name, platform);
				string failure = null;
				try {
					var plist = await BCLTestInfoPlistGenerator.GenerateCodeAsync (GetPlistTemplate (platform), projectDefinition.Name);
					var infoPlistPath = GetPListPath (generatedCodeDir, platform);
					using (var file = new StreamWriter (infoPlistPath, false)) { // false is do not append
						await file.WriteAsync (plist);
					}

					var info = projectDefinition.GetAssemblyInclusionInformation (platform);
					var generatedProject = await GenerateMacAsync (projectDefinition.Name, registerTypePath,
						info, GetProjectTemplate (platform), infoPlistPath, platform);
					using (var file = new StreamWriter (projectPath, false)) { // false is do not append
						await file.WriteAsync (generatedProject);
					}
					failure = failure ?? info.FailureMessage;
					failure = failure ?? typesPerAssembly.FailureMessage;
				} catch (Exception e) {
					failure = e.Message;
				}
				projectPaths.Add (new BclTestProject { Name = projectDefinition.Name, Path = projectPath, XUnit = projectDefinition.IsXUnit, ExtraArgs = projectDefinition.ExtraArgs, Failure = failure, TimeoutMultiplier = def.TimeoutMultiplier });

			}
			return projectPaths;
		}

		#endregion
	}
}
