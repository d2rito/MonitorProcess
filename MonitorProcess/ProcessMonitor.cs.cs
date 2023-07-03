using System;
using System.Diagnostics;
using System.Threading;

class ProcessMonitor
{
    static void Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: monitor.exe <processName> <maxLifetime> <monitorFrequency>");
            return;
        }

        string targetProcessName = args[0];
        int maxLifetime = int.Parse(args[1]);
        int monitorFrequency = int.Parse(args[2]);

        Console.WriteLine($"Monitoring process '{targetProcessName}' with a max lifetime of {maxLifetime} minutes.");

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
            // time to terminate te program
            Thread.Sleep(monitorFrequency * 60000); 

            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
            {
                break;
            }
        }
    }
}