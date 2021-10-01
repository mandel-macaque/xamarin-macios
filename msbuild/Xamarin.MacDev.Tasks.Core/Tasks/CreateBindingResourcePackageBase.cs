using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Localization.MSBuild;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	public abstract class CreateBindingResourcePackageBase : XamarinTask {
		[Required]
		public string Compress { get; set; }

		[Required]
		public string BindingResourcePath { get; set; }

		[Required]
		public string IntermediateOutputPath { get; set; }

		[Required]		
		public ITaskItem[] NativeReferences { get; set; }
		
		public override bool Execute ()
		{
			// LinkWith must be migrated for NoBindingEmbedding styled binding projects
			if (NativeReferences.Length == 0) {
				Log.LogError (7068, null, MSBStrings.E7068);
				return false;
			}

			var compress = false;
			if (string.Equals (Compress, "true", StringComparison.OrdinalIgnoreCase)) {
				compress = true;
			} else if (string.Equals (Compress, "false", StringComparison.OrdinalIgnoreCase)) {
				compress = false;
			} else if (string.Equals (Compress, "auto", StringComparison.OrdinalIgnoreCase)) {
				compress = ContainsSymlinks (NativeReferences);
				if (compress)
					Log.LogMessage (MessageImportance.Low, MSBStrings.W7085 /* "Creating a compressed binding resource package because there are symlinks in the input." */);
			} else {
				Log.LogError (MSBStrings.E7086 /* "The value '{0}' is invalid for the Compress property. Valid values: 'true', 'false' or 'auto'." */, Compress);
			}

			Directory.CreateDirectory (compress ? IntermediateOutputPath : BindingResourcePath);

			var manifestDirectory = compress ? IntermediateOutputPath : BindingResourcePath;
			var manifestPath = CreateManifest (manifestDirectory);

			if (compress) {
				var zipFile = Path.GetFullPath (BindingResourcePath + ".zip");
				Log.LogMessage (MSBStrings.M0121, zipFile);
				if (File.Exists (zipFile))
					File.Delete (zipFile);
				Directory.CreateDirectory (Path.GetDirectoryName (zipFile));

				var filesToZip = NativeReferences.Select (v => v.ItemSpec).ToList ();
				filesToZip.Add (manifestPath);

				foreach (var nativeRef in filesToZip) {
					var zipArguments = new List<string> ();
					zipArguments.Add ("-9");
					zipArguments.Add ("-r");
					zipArguments.Add ("-y");
					zipArguments.Add (zipFile);

					var fullPath = Path.GetFullPath (nativeRef);
					var workingDirectory = Path.GetDirectoryName (fullPath);
					zipArguments.Add (Path.GetFileName (fullPath));
					ExecuteAsync ("zip", zipArguments, workingDirectory: workingDirectory).Wait ();
				}
			} else {
				var bindingResourcePath = BindingResourcePath;
				Log.LogMessage (MSBStrings.M0121, bindingResourcePath);
				Directory.CreateDirectory (bindingResourcePath);
				foreach (var nativeRef in NativeReferences)
					Xamarin.Bundler.FileCopier.UpdateDirectory (nativeRef.ItemSpec, bindingResourcePath);
			}

			return !Log.HasLoggedErrors;
		}

		static bool ContainsSymlinks (ITaskItem[] items)
		{
			foreach (var item in items) {
				if (PathUtils.IsSymlinkOrContainsSymlinks (item.ItemSpec))
					return true;
			}

			return false;
		}

		string [] NativeReferenceAttributeNames = new string [] { "Kind", "ForceLoad", "SmartLink", "Frameworks", "WeakFrameworks", "LinkerFlags", "NeedsGccExceptionHandling", "IsCxx"};

		string CreateManifest (string resourcePath)
		{
			XmlWriterSettings settings = new XmlWriterSettings() {
				OmitXmlDeclaration = true,
				Indent = true,
				IndentChars = "\t",
			};

			string manifestPath = Path.Combine (resourcePath, "manifest");
			using (var writer = XmlWriter.Create (manifestPath, settings)) {
				writer.WriteStartElement ("BindingAssembly");

				foreach (var nativeRef in NativeReferences) {
					writer.WriteStartElement ("NativeReference");
					writer.WriteAttributeString ("Name", Path.GetFileName (nativeRef.ItemSpec));

					foreach (string attribute in NativeReferenceAttributeNames) {
						writer.WriteStartElement (attribute);
						writer.WriteString (nativeRef.GetMetadata (attribute));
						writer.WriteEndElement ();
					}

					writer.WriteEndElement ();
				}
				writer.WriteEndElement ();
			}
			return manifestPath;
		}
	}
}
