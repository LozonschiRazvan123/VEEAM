using System.Diagnostics;
using VEEAM;

namespace WindowsProcessTests
{
    public class UnitTest1
    {
        [Fact]
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
            Assert.True(notepadProcess.HasExited);

            notepadProcess.Close();
        }

        [Fact]
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
            Assert.True(File.Exists(logFilePath));

            File.Delete(logFilePath);
        }
    }
}