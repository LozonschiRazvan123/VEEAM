using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEEAM
{
    public class WindowsProcess
    {
        private string processName;
        private int maxLifetimeMinutes;
        private int monitoringFrequencyMinutes;

        public WindowsProcess(string processName, int maxLifetimeMinutes, int monitoringFrequencyMinutes)
        {
            this.processName = processName;
            this.maxLifetimeMinutes = maxLifetimeMinutes;
            this.monitoringFrequencyMinutes = monitoringFrequencyMinutes;
        }

        public void StartMonitoring()
        {
            Console.WriteLine($"Monitoring process '{processName}' every {monitoringFrequencyMinutes} minute(s).");
            Console.WriteLine($"Killing processes running longer than {maxLifetimeMinutes} minute(s).");

            while (true)
            {
                CheckAndKillProcesses();
                Thread.Sleep(monitoringFrequencyMinutes * 60 * 1000); 
            }
        }

        private void CheckAndKillProcesses()
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                TimeSpan lifetime = DateTime.Now - process.StartTime;
                if (lifetime.TotalMinutes > maxLifetimeMinutes)
                {
                    Console.WriteLine($"Process '{processName}' with ID {process.Id} has exceeded maximum lifetime. Killing...");
                    process.Kill();
                    LogProcessTermination(processName, process.Id);
                }
            }
        }

        private void LogProcessTermination(string processName, int processId)
        {
            Console.WriteLine($"Process '{processName}' with ID {processId} terminated at {DateTime.Now}.");
        }
    }
}
