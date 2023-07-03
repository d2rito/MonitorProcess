using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorProcess
{
    internal class UnitTests
    {
        [TestFixture]
        public class ProcessMonitorTests
        {
            [Test]
            public void MonitorProcess_TerminatesProcess_AfterMaxLifetime()
            {
                // Arrange
                string processName = "notepad";
                int maxLifetime = 1;
                int monitorFrequency = 1;

                // Act
                Thread monitorThread = new Thread(() => ProcessMonitor.MonitorProcess(processName, maxLifetime, monitorFrequency));
                monitorThread.Start();
                Thread.Sleep(5000); // run for a few secs
                monitorThread.Abort(); // terminate it

                // Assert
                Process[] processes = Process.GetProcessesByName(processName);
                Assert.AreEqual(0, processes.Length, "The process should be terminated after the max lifetime.");
            }

            [Test]
            public void MonitorProcess_DoesNotTerminateProcess_BeforeMaxLifetime()
            {
                // Arrange
                string processName = "notepad";
                int maxLifetime = 5;
                int monitorFrequency = 1;
                ProcessStartInfo startInfo = new ProcessStartInfo(processName);
                Process process = Process.Start(startInfo);

                Thread monitorThread = null;

                try
                {
                    // Act
                    monitorThread = new Thread(() => ProcessMonitor.MonitorProcess(processName, maxLifetime, monitorFrequency));
                    monitorThread.Start();
                    Thread.Sleep(3000);

                    // Assert
                    Process[] processes = Process.GetProcessesByName(processName);
                    Assert.AreEqual(1, processes.Length, "The process should not be terminated before the max lifetime.");

                    Thread.Sleep((maxLifetime + 1) * 60000);

                    processes = Process.GetProcessesByName(processName);
                    Assert.AreEqual(1, processes.Length, "The process should not be terminated after the max lifetime.");
                }
                finally
                {

                    process.Kill();
                    if (monitorThread != null)
                    {
                        monitorThread.Abort();
                    }
                }
            }
        }

        // helper to test the method
        public class ProcessMonitor
        {
            public static void MonitorProcess(string targetProcessName, int maxLifetime, int monitorFrequency)
            {
                while (true)
                {
                    Process[] processes = Process.GetProcesses();

                    foreach (Process process in processes)
                    {
                        if (process.ProcessName.Equals(targetProcessName, StringComparison.OrdinalIgnoreCase))
                        {
                            TimeSpan runningTime = DateTime.Now - process.StartTime;

                            if (runningTime.TotalMinutes > maxLifetime)
                            {
                                Console.WriteLine($"Process '{targetProcessName}' (PID: {process.Id}) exceeded the maximum lifetime and will be terminated.");
                                process.Kill();
                            }
                        }
                    }

                    Thread.Sleep(monitorFrequency * 60000);

                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                    {
                        break;
                    }
                }
            }
        }
    }
}
