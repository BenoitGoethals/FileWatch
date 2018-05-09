using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileWatch
{
    partial class ServiceFile : ServiceBase
    {

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        private int eventId = 1;
     private readonly Watcher _watcher = new Watcher() { SourcePath = "c:/source", DestPath = "c:/archive" };
        public ServiceFile()
        {
            InitializeComponent();
            eventLogWatch = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("MyWatcherSource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "MyWatcherSource", "MyNewWatcherLog");
            }
            eventLogWatch.Source = "MyWatcherSource";
            eventLogWatch.Log = "MyNewWatcherLog";
        }


        public void RunAsConsole(string[] args)
        {
            OnStart(args);
            Console.WriteLine("Press any key to exit...");
            _watcher.Watch();
            Console.ReadLine();
            OnStop();
        }
        protected override void OnStart(string[] args)
        {
            Creat(_watcher.SourcePath);
            Creat(_watcher.DestPath);
            eventLogWatch.WriteEntry("In OnStart FileWather");
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
          
          
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLogWatch.WriteEntry("In OnStart Running");
        }

        protected override void OnStop()
        {
            eventLogWatch.WriteEntry("In onStop.");
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLogWatch.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }



        private void Creat(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
    }
}
