using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace VEEAM.Test
{
    [TestFixture]
    public class WindowsProcessTests
    {
        [Test]
        public static void ProcessMonitor_CheckAndKillProcesses_ProcessesKilled()
        {
            // Arrange
            string processName = "notepad";
            int maxLifetimeMinutes = 1;
            int monitoringFrequencyMinutes = 1;
            WindowsProcess monitor = new WindowsProcess(processName, maxLifetimeMinutes, monitoringFrequencyMinutes);

            Thread monitoringThread = new Thread(monitor.StartMonitoring);
            monitoringThread.Start();

            Process notepadProcess = Process.Start("notepad.exe");
            Thread.Sleep((maxLifetimeMinutes + 1) * 60 * 1000);

            // Assert
            Assert.That(notepadProcess.HasExited, Is.True); 

            notepadProcess.Close();
        }

        [Test]
        public static void ProcessMonitor_LogProcessTermination_LogCreated()
        {
            // Arrange
            string processName = "notepad";
            int maxLifetimeMinutes = 1;
            int monitoringFrequencyMinutes = 1;
            WindowsProcess monitor = new WindowsProcess(processName, maxLifetimeMinutes, monitoringFrequencyMinutes);
            string logFilePath = "process_log.txt";

            // Act
            monitor.StartMonitoring();
            Process notepadProcess = Process.Start("notepad.exe");
            Thread.Sleep((maxLifetimeMinutes + 1) * 60 * 1000);
            notepadProcess.Close();

            // Assert
            Assert.That(File.Exists(logFilePath), Is.True); 

            File.Delete(logFilePath);
        }
    }
}
