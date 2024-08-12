using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;

namespace XMLProject
{
    internal class ServiceAdapter
    {
        Mail Mail = new Mail();
        IP ip = new IP();

        private static Dictionary<int, string> previouslyRunningProcesses = new Dictionary<int, string>();
        private static System.Timers.Timer _timer;
        public ServiceAdapter() 
        {

            _timer = new System.Timers.Timer(3000);
            _timer.Elapsed += TimerElapsed;
            _timer.AutoReset = true;

        }
        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            string computerName = Environment.MachineName;
            string userName = UserInfo.GetUserName();
            string ipAddress = IP.GetLocalIPAddress();
            string xmlFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Project.xml");
            xmlFilePath = @"C:\Project.xml";
            if (!File.Exists(xmlFilePath))
            {
                Console.WriteLine($"File not found: {xmlFilePath}");
                Mail.SendEmail("Application Error", $"File not found: {xmlFilePath}");
                return;
            }

            XDocument xDoc = XDocument.Load(xmlFilePath);
            var applications = xDoc.Descendants("application");

            Dictionary<int, string> currentlyRunningProcesses = new Dictionary<int, string>();

            foreach (var application in applications)
            {
                string path = application.Element("path")?.Value;
                string processName = Path.GetFileNameWithoutExtension(path);

                var processes = Process.GetProcessesByName(processName);

                if (processes.Length > 0)
                {
                    foreach (var process in processes)
                    {
                        try
                        {
                            TimeSpan uptime = DateTime.Now - process.StartTime;

                            string emailBody = Mail.CreateEmailBody(process.ProcessName, process.Id, computerName, userName, ipAddress, "Started");

                            if (!previouslyRunningProcesses.ContainsKey(process.Id))
                            {
                                Console.WriteLine($"Application {path} started.");
                                Console.WriteLine($"Process Name: {process.ProcessName}");
                                Console.WriteLine($"Process ID: {process.Id}");
                                Console.WriteLine($"Started at: {process.StartTime}");
                                Console.WriteLine($"Uptime: {uptime.TotalHours:F2} hours");
                                Console.WriteLine($"Computer Name: {computerName}");
                                Console.WriteLine($"Windows User Name: {userName}");
                                Console.WriteLine($"IP Address: {ipAddress}");
                                Console.WriteLine("********************************");

                                Mail.SendEmail("Application Started", emailBody);
                                previouslyRunningProcesses[process.Id] = process.ProcessName;
                            }

                            currentlyRunningProcesses[process.Id] = process.ProcessName;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error retrieving information for process ID {process.Id}: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Application {path} is not running.");
                }
            }

            foreach (var runningProcess in previouslyRunningProcesses.ToList())
            {
                if (!currentlyRunningProcesses.ContainsKey(runningProcess.Key))
                {
                    Mail.SendEmail("Application Stopped", Mail.CreateEmailBody(runningProcess.Value, runningProcess.Key, computerName, userName, ipAddress, "Stopped"));
                    previouslyRunningProcesses.Remove(runningProcess.Key);
                }
            }
        }
        public void Start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
        }

    }
}
