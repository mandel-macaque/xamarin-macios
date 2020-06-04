﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.XHarness.iOS.Shared;
using Microsoft.DotNet.XHarness.iOS.Shared.Execution;
using Microsoft.DotNet.XHarness.iOS.Shared.Hardware;
using Microsoft.DotNet.XHarness.iOS.Shared.Tasks;
using Xharness.Jenkins.TestTasks;

namespace Xharness.Jenkins {
	// lets try and keep this class stateless, will make our lifes better
	class RunSimulatorTasksFactory {

		public async Task<IEnumerable<ITestTask>> CreateAsync (Jenkins jenkins, IProcessManager processManager, TestVariationsFactory testVariationsFactory)
		{
			var runSimulatorTasks = new List<RunSimulatorTask> ();

			foreach (var project in jenkins.Harness.IOSTestProjects) {
				if (!project.IsExecutableProject)
					continue;

				bool ignored = !jenkins.TestSelection.HasFlag (TestSelection.Simulator);
				if (!jenkins.IsIncluded (project))
					ignored = true;

				var ps = new List<Tuple<TestProject, TestPlatform, bool>> ();
				if (!project.SkipiOSVariation)
					ps.Add (new Tuple<TestProject, TestPlatform, bool> (project, TestPlatform.iOS_Unified, ignored || !jenkins.TestSelection.HasFlag (TestSelection.iOS64)));
				if (project.MonoNativeInfo != null)
					ps.Add (new Tuple<TestProject, TestPlatform, bool> (project, TestPlatform.iOS_TodayExtension64, ignored || !jenkins.TestSelection.HasFlag(TestSelection.iOS64)));
				if (!project.SkiptvOSVariation)
					ps.Add (new Tuple<TestProject, TestPlatform, bool> (project.AsTvOSProject (), TestPlatform.tvOS, ignored || !jenkins.TestSelection.HasFlag (TestSelection.tvOS)));
				if (!project.SkipwatchOSVariation)
					ps.Add (new Tuple<TestProject, TestPlatform, bool> (project.AsWatchOSProject (), TestPlatform.watchOS, ignored || !jenkins.TestSelection.HasFlag (TestSelection.watchOs)));

				var configurations = project.Configurations;
				if (configurations == null)
					configurations = new string [] { "Debug" };
				foreach (var config in configurations) {
					foreach (var pair in ps) {
						var derived = new MSBuildTask (jenkins: jenkins, testProject: project, processManager: processManager) {
							ProjectConfiguration = config,
							ProjectPlatform = "iPhoneSimulator",
							Platform = pair.Item2,
							Ignored = pair.Item3,
							TestName = project.Name,
							Dependency = project.Dependency,
						};
						derived.CloneTestProject (jenkins.MainLog, processManager, pair.Item1);
						var simTasks = CreateAsync (jenkins, processManager, derived);
						runSimulatorTasks.AddRange (simTasks);
						foreach (var task in simTasks) {
							if (configurations.Length > 1)
								task.Variation = config;
							task.TimeoutMultiplier = project.TimeoutMultiplier;
						}
					}
				}
			}

			var testVariations = testVariationsFactory.CreateTestVariations (runSimulatorTasks, (buildTask, test, candidates) =>
				new RunSimulatorTask (
					jenkins: jenkins, 
					simulators: jenkins.Simulators,
					buildTask: buildTask,
					processManager: processManager,
					tunnelBore: jenkins.TunnelBore,
					candidates: candidates?.Cast<SimulatorDevice> () ?? test.Candidates)).ToList ();

			foreach (var tv in testVariations) {
				if (!tv.Ignored)
					await tv.FindSimulatorAsync ();
			}

			var rv = new List<AggregatedRunSimulatorTask> ();
			foreach (var taskGroup in testVariations.GroupBy ((RunSimulatorTask task) => task.Device?.UDID ?? task.Candidates.ToString ())) {
				rv.Add (new AggregatedRunSimulatorTask (jenkins: jenkins, tasks: taskGroup) {
					TestName = $"Tests for {taskGroup.Key}",
				});
			}
			return rv;
		}

		IEnumerable<RunSimulatorTask> CreateAsync (Jenkins jenkins, IProcessManager processManager, MSBuildTask buildTask)
		{
			var runtasks = new List<RunSimulatorTask> ();

			TestTarget [] targets = buildTask.Platform.GetAppRunnerTargets ();
			TestPlatform [] platforms;
			bool [] ignored;

			switch (buildTask.Platform) {
			case TestPlatform.tvOS:
				platforms = new TestPlatform [] { TestPlatform.tvOS };
				ignored = new [] { false };
				break;
			case TestPlatform.watchOS:
				platforms = new TestPlatform [] { TestPlatform.watchOS_32 };
				ignored = new [] { false };
				break;
			case TestPlatform.iOS_Unified:
				platforms = new TestPlatform [] { TestPlatform.iOS_Unified32, TestPlatform.iOS_Unified64 };
				ignored = new [] { !jenkins.TestSelection.HasFlag (TestSelection.iOS32) , false };
				break;
			case TestPlatform.iOS_TodayExtension64:
				targets = new TestTarget [] { TestTarget.Simulator_iOS64 };
				platforms = new TestPlatform [] { TestPlatform.iOS_TodayExtension64 };
				ignored = new [] { false };
				break;
			default:
				throw new NotImplementedException ();
			}

			for (int i = 0; i < targets.Length; i++) {
				var sims = jenkins.Simulators.SelectDevices (targets [i], jenkins.SimulatorLoadLog, false);
				runtasks.Add (new RunSimulatorTask (
					jenkins: jenkins,
					simulators: jenkins.Simulators,
					buildTask: buildTask,
					processManager: processManager,
					tunnelBore: jenkins.TunnelBore,
					candidates: sims) {
					Platform = platforms [i],
					Ignored = ignored [i] || buildTask.Ignored
				});
			}

			return runtasks;
		}
	}
}
