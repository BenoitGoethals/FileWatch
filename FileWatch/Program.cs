using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileWatch
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;
            if (Environment.UserInteractive)
            {
              ServiceFile ser=  new ServiceFile();
                ser.RunAsConsole(args);
            }
            else
            {
                ServicesToRun = new ServiceBase[]
            {
                new ServiceFile()
            };

            ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
