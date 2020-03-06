using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xharness;
using Xharness.Execution;
using Xharness.Logging;

namespace xharness.Hardware {

	public class SimDevice : ISimulatorDevice {
		public string UDID { get; set; }
		public string Name { get; set; }
		public IProcessManager ProcessManager { get; set; } = new ProcessManager ();
		public string SimRuntime { get; set; }
		public string SimDeviceType { get; set; }
		public string DataPath { get; set; }
		public string LogPath { get; set; }
		public string SystemLog { get { return Path.Combine (LogPath, "system.log"); } }

		public IHarness Harness;

		public bool IsWatchSimulator { get { return SimRuntime.StartsWith ("com.apple.CoreSimulator.SimRuntime.watchOS", StringComparison.Ordinal); } }

		public string OSVersion {
			get {
				var v = SimRuntime.Substring ("com.apple.CoreSimulator.SimRuntime.".Length);
				var dash = v.IndexOf ('-');
				return v.Substring (0, dash) + " " + v.Substring (dash + 1).Replace ('-', '.');
			}
		}

		public async Task EraseAsync (ILog log)
		{
			// here we don't care if execution fails.
			// erase the simulator (make sure the device isn't running first)
			await Harness.ExecuteXcodeCommandAsync ("simctl", new [] { "shutdown", UDID }, log, TimeSpan.FromMinutes (1));
			await Harness.ExecuteXcodeCommandAsync ("simctl", new [] { "erase", UDID }, log, TimeSpan.FromMinutes (1));

			// boot & shutdown to make sure it actually works
			await Harness.ExecuteXcodeCommandAsync ("simctl", new [] { "boot", UDID }, log, TimeSpan.FromMinutes (1));
			await Harness.ExecuteXcodeCommandAsync ("simctl", new [] { "shutdown", UDID }, log, TimeSpan.FromMinutes (1));
		}

		public async Task ShutdownAsync (ILog log)
		{
			await Harness.ExecuteXcodeCommandAsync ("simctl", new [] { "shutdown", UDID }, log, TimeSpan.FromMinutes (1));
		}

		public static async Task KillEverythingAsync (ILog log, IProcessManager processManager = null)
		{
			if (processManager == null)
				processManager = new ProcessManager ();
			await processManager.ExecuteCommandAsync ("launchctl", new [] { "remove", "com.apple.CoreSimulator.CoreSimulatorService" }, log, TimeSpan.FromSeconds (10));

			var to_kill = new string [] { "iPhone Simulator", "iOS Simulator", "Simulator", "Simulator (Watch)", "com.apple.CoreSimulator.CoreSimulatorService", "ibtoold" };

			var args = new List<string> ();
			args.Add ("-9");
			args.AddRange (to_kill);
			await processManager.ExecuteCommandAsync ("killall", args, log, TimeSpan.FromSeconds (10));

			foreach (var dir in new string [] {
				Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.UserProfile), "Library", "Saved Application State", "com.apple.watchsimulator.savedState"),
				Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.UserProfile), "Library", "Saved Application State", "com.apple.iphonesimulator.savedState"),
			}) {
				try {
					if (Directory.Exists (dir))
						Directory.Delete (dir, true);
				} catch (Exception e) {
					log.WriteLine ("Could not delete the directory '{0}': {1}", dir, e.Message);
				}
			}
		}

		int TCCFormat {
			get {
				// v1: < iOS 9
				// v2: >= iOS 9 && < iOS 12
				// v3: >= iOS 12
				if (SimRuntime.StartsWith ("com.apple.CoreSimulator.SimRuntime.iOS-", StringComparison.Ordinal)) {
					var v = Version.Parse (SimRuntime.Substring ("com.apple.CoreSimulator.SimRuntime.iOS-".Length).Replace ('-', '.'));
					if (v.Major >= 12) {
						return 3;
					} else if (v.Major >= 9) {
						return 2;
					} else {
						return 1;
					}
				} else if (SimRuntime.StartsWith ("com.apple.CoreSimulator.SimRuntime.tvOS-", StringComparison.Ordinal)) {
					var v = Version.Parse (SimRuntime.Substring ("com.apple.CoreSimulator.SimRuntime.tvOS-".Length).Replace ('-', '.'));
					if (v.Major >= 12) {
						return 3;
					} else {
						return 2;
					}
				} else if (SimRuntime.StartsWith ("com.apple.CoreSimulator.SimRuntime.watchOS-", StringComparison.Ordinal)) {
					var v = Version.Parse (SimRuntime.Substring ("com.apple.CoreSimulator.SimRuntime.watchOS-".Length).Replace ('-', '.'));
					if (v.Major >= 5) {
						return 3;
					} else {
						return 2;
					}
				} else {
					throw new NotImplementedException ();
				}
			}
		}

		public async Task AgreeToPromptsAsync (ILog log, params string [] bundle_identifiers)
		{
			if (bundle_identifiers == null || bundle_identifiers.Length == 0) {
				log.WriteLine ("No bundle identifiers given when requested permission editing.");
				return;
			}

			var TCC_db = Path.Combine (DataPath, "data", "Library", "TCC", "TCC.db");
			var sim_services = new string [] {
					"kTCCServiceAddressBook",
					"kTCCServiceCalendar",
					"kTCCServicePhotos",
					"kTCCServiceMediaLibrary",
					"kTCCServiceMicrophone",
					"kTCCServiceUbiquity",
					"kTCCServiceWillow"
				};

			var failure = false;
			var tcc_edit_timeout = 30;
			var watch = new Stopwatch ();
			watch.Start ();

			do {
				if (failure) {
					log.WriteLine ("Failed to edit TCC.db, trying again in 1 second... ", (int)(tcc_edit_timeout - watch.Elapsed.TotalSeconds));
					await Task.Delay (TimeSpan.FromSeconds (1));
				}
				failure = false;
				foreach (var bundle_identifier in bundle_identifiers) {
					var args = new List<string> ();
					var sql = new System.Text.StringBuilder ();
					args.Add (TCC_db);
					foreach (var service in sim_services) {
						switch (TCCFormat) {
						case 1:
							// CREATE TABLE access (service TEXT NOT NULL, client TEXT NOT NULL, client_type INTEGER NOT NULL, allowed INTEGER NOT NULL, prompt_count INTEGER NOT NULL, csreq BLOB, CONSTRAINT key PRIMARY KEY (service, client, client_type));
							sql.AppendFormat ("INSERT INTO access VALUES('{0}','{1}',0,1,0,NULL);", service, bundle_identifier);
							sql.AppendFormat ("INSERT INTO access VALUES('{0}','{1}',0,1,0,NULL);", service, bundle_identifier + ".watchkitapp");
							break;
						case 2:
							// CREATE TABLE access (service	TEXT NOT NULL, client TEXT NOT NULL, client_type INTEGER NOT NULL, allowed INTEGER NOT NULL, prompt_count INTEGER NOT NULL, csreq BLOB, policy_id INTEGER, PRIMARY KEY (service, client, client_type), FOREIGN KEY (policy_id) REFERENCES policies(id) ON DELETE CASCADE ON UPDATE CASCADE);
							sql.AppendFormat ("INSERT INTO access VALUES('{0}','{1}',0,1,0,NULL,NULL);", service, bundle_identifier);
							sql.AppendFormat ("INSERT INTO access VALUES('{0}','{1}',0,1,0,NULL,NULL);", service, bundle_identifier + ".watchkitapp");
							break;
						case 3: // Xcode 10+
								// CREATE TABLE access (    service        TEXT        NOT NULL,     client         TEXT        NOT NULL,     client_type    INTEGER     NOT NULL,     allowed        INTEGER     NOT NULL,     prompt_count   INTEGER     NOT NULL,     csreq          BLOB,     policy_id      INTEGER,     indirect_object_identifier_type    INTEGER,     indirect_object_identifier         TEXT,     indirect_object_code_identity      BLOB,     flags          INTEGER,     last_modified  INTEGER     NOT NULL DEFAULT (CAST(strftime('%s','now') AS INTEGER)),     PRIMARY KEY (service, client, client_type, indirect_object_identifier),    FOREIGN KEY (policy_id) REFERENCES policies(id) ON DELETE CASCADE ON UPDATE CASCADE)
							sql.AppendFormat ("INSERT OR REPLACE INTO access VALUES('{0}','{1}',0,1,0,NULL,NULL,NULL,'UNUSED',NULL,NULL,{2});", service, bundle_identifier, DateTimeOffset.Now.ToUnixTimeSeconds ());
							sql.AppendFormat ("INSERT OR REPLACE INTO access VALUES('{0}','{1}',0,1,0,NULL,NULL,NULL,'UNUSED',NULL,NULL,{2});", service, bundle_identifier + ".watchkitapp", DateTimeOffset.Now.ToUnixTimeSeconds ());
							break;
						default:
							throw new NotImplementedException ();
						}
					}
					args.Add (sql.ToString ());
					var rv = await ProcessManager.ExecuteCommandAsync ("sqlite3", args, log, TimeSpan.FromSeconds (5));
					if (!rv.Succeeded) {
						failure = true;
						break;
					}
				}
			} while (failure && watch.Elapsed.TotalSeconds <= tcc_edit_timeout);

			if (failure) {
				log.WriteLine ("Failed to edit TCC.db, the test run might hang due to permission request dialogs");
			} else {
				log.WriteLine ("Successfully edited TCC.db");
			}

			log.WriteLine ("Current TCC database contents:");
			await ProcessManager.ExecuteCommandAsync ("sqlite3", new [] { TCC_db, ".dump" }, log, TimeSpan.FromSeconds (5));
		}

		async Task OpenSimulator (ILog log)
		{
			string simulator_app;

			if (IsWatchSimulator && Harness.XcodeVersion.Major < 9) {
				simulator_app = Path.Combine (Harness.XcodeRoot, "Contents", "Developer", "Applications", "Simulator (Watch).app");
			} else {
				simulator_app = Path.Combine (Harness.XcodeRoot, "Contents", "Developer", "Applications", "Simulator.app");
				if (!Directory.Exists (simulator_app))
					simulator_app = Path.Combine (Harness.XcodeRoot, "Contents", "Developer", "Applications", "iOS Simulator.app");
			}

			await ProcessManager.ExecuteCommandAsync ("open", new [] { "-a", simulator_app, "--args", "-CurrentDeviceUDID", UDID }, log, TimeSpan.FromSeconds (15));
		}

		public async Task PrepareSimulatorAsync (ILog log, params string [] bundle_identifiers)
		{
			// Kill all existing processes
			await KillEverythingAsync (log);

			// We shutdown and erase all simulators.
			await EraseAsync (log);

			// Edit the permissions to prevent dialog boxes in the test app
			var TCC_db = Path.Combine (DataPath, "data", "Library", "TCC", "TCC.db");
			if (!File.Exists (TCC_db)) {
				log.WriteLine ("Opening simulator to create TCC.db");
				await OpenSimulator (log);

				var tcc_creation_timeout = 60;
				var watch = new Stopwatch ();
				watch.Start ();
				while (!File.Exists (TCC_db) && watch.Elapsed.TotalSeconds < tcc_creation_timeout) {
					log.WriteLine ("Waiting for simulator to create TCC.db... {0}", (int)(tcc_creation_timeout - watch.Elapsed.TotalSeconds));
					await Task.Delay (TimeSpan.FromSeconds (0.250));
				}
			}

			if (File.Exists (TCC_db)) {
				await AgreeToPromptsAsync (log, bundle_identifiers);
			} else {
				log.WriteLine ("No TCC.db found for the simulator {0} (SimRuntime={1} and SimDeviceType={1})", UDID, SimRuntime, SimDeviceType);
			}

			// Make sure we're in a clean state
			await KillEverythingAsync (log);

			// Make 100% sure we're shutdown
			await ShutdownAsync (log);
		}

	}
}
