﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Xharness.Execution;
using Xharness.Hardware;
using Xharness.Logging;

namespace Xharness.Tests.Hardware.Tests {
	[TestFixture]
	public class SimulatorDeviceTest {

		Mock<ILog> executionLog;
		Mock<IProcessManager> processManager;
		Mock<IHarness> harness;
		SimulatorDevice simulator;

		[SetUp]
		public void SetUp ()
		{
			executionLog = new Mock<ILog> ();
			processManager = new Mock<IProcessManager> ();
			harness = new Mock<IHarness> ();
			simulator = new SimulatorDevice (harness.Object, processManager.Object) {
				UDID = Guid.NewGuid ().ToString ()
			};
		}

		[TearDown]
		public void TearDown ()
		{
			executionLog = null;
			processManager = null;
			simulator = null;
		}

		[TestCase ("com.apple.CoreSimulator.SimRuntime.watchOS-5-1", true)]
		[TestCase ("com.apple.CoreSimulator.SimRuntime.iOS-7-1", false)]
		public void IsWatchSimulatorTest (string runtime, bool expectation)
		{
			simulator.SimRuntime = runtime;
			Assert.AreEqual (expectation, simulator.IsWatchSimulator);
		}

		[TestCase ("com.apple.CoreSimulator.SimRuntime.iOS-12-1", "iOS 12.1")]
		[TestCase ("com.apple.CoreSimulator.SimRuntime.iOS-10-1", "iOS 10.1")]
		public void OSVersionTest (string runtime, string expected)
		{
			simulator.SimRuntime = runtime;
			Assert.AreEqual (expected, simulator.OSVersion);
		}

		[Test]
		public async Task EraseAsyncTest ()
		{
			// just call and verify the correct args are pass
			await simulator.EraseAsync (executionLog.Object);
			harness.Verify (h => h.ExecuteXcodeCommandAsync (It.Is<string> (s => s == "simctl"), It.Is<string []> (args => args.Where (a => a == simulator.UDID || a == "shutdown").Count () == 2), It.IsAny<ILog> (), It.IsAny<TimeSpan> ()));
			harness.Verify (h => h.ExecuteXcodeCommandAsync (It.Is<string> (s => s == "simctl"), It.Is<string []> (args => args.Where (a => a == simulator.UDID || a == "erase").Count () == 2), It.IsAny<ILog> (), It.IsAny<TimeSpan> ()));
			harness.Verify (h => h.ExecuteXcodeCommandAsync (It.Is<string> (s => s == "simctl"), It.Is<string []> (args => args.Where (a => a == simulator.UDID || a == "boot").Count () == 2), It.IsAny<ILog> (), It.IsAny<TimeSpan> ()));
			harness.Verify (h => h.ExecuteXcodeCommandAsync (It.Is<string> (s => s == "simctl"), It.Is<string []> (args => args.Where (a => a == simulator.UDID || a == "shutdown").Count () == 2), It.IsAny<ILog> (), It.IsAny<TimeSpan> ()));

		}

		[Test]
		public async Task ShutdownAsyncTest ()
		{
			await simulator.ShutdownAsync (executionLog.Object);
			// just call and verify the correct args are pass
			harness.Verify (h => h.ExecuteXcodeCommandAsync (It.Is<string> (s => s == "simctl"), It.Is<string []> (args => args.Where (a => a == simulator.UDID || a == "shutdown").Count () == 2), It.IsAny<ILog> (), It.IsAny<TimeSpan> ()));
		}

		[Test]
		[Ignore("Running this test will actually kill simulators on the machine")]
		public async Task KillEverythingAsyncTest ()
		{
			Func<IList<string>, bool> verifyKillAll = (args) => {
				var toKill = new List<string> { "-9", "iPhone Simulator", "iOS Simulator", "Simulator", "Simulator (Watch)", "com.apple.CoreSimulator.CoreSimulatorService", "ibtoold" };
				return args.Where (a => toKill.Contains (a)).Count () == toKill.Count;
			};

			var simulator = new SimulatorDevice (Mock.Of<IHarness> (), processManager.Object);
			await simulator.KillEverythingAsync (executionLog.Object);

			// verify that all the diff process have been killed making sure args are correct
			processManager.Verify (p => p.ExecuteCommandAsync (It.Is<string> (s => s == "launchctl"), It.Is<string []> (args => args.Where (a => a == "remove" || a == "com.apple.CoreSimulator.CoreSimulatorService").Count () == 2), It.IsAny<ILog> (), It.IsAny<TimeSpan> (), null, null));
			processManager.Verify (p => p.ExecuteCommandAsync (It.Is<string> (s => s == "killall"), It.Is<IList<string>> (a => verifyKillAll (a)), It.IsAny<ILog> (), It.IsAny<TimeSpan> (), null, null));
		}

	}
}
