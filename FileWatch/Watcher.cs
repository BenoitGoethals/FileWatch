using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FileWatch
{
    public class Watcher
    {
        private static readonly FileSystemWatcher _watcher=new FileSystemWatcher();

        public string SourcePath { get; set; }

        public string DestPath { get; set; }
        public EventLog eventLog { get; internal set; }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Watch()
        {

            _watcher.Path = SourcePath;
           
            _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                                            | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _watcher.Filter = "*.*";
            _watcher.Changed += new FileSystemEventHandler(OnChanged);
           // _watcher.Created += new FileSystemEventHandler(OnChanged);
           // _watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //  _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
        
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
           if( File.Exists(e.FullPath) )
           {
               Task.Delay(10000);
            string extension = Path.GetExtension(e.Name)?.Substring(1);
            if (!String.IsNullOrEmpty(extension))
            {
                Created(extension);
                CopyFil(e.Name, e.Name, extension);
                   
                }
            }
        }


        private bool FileIsReady(string path)
        {
            //One exception per file rather than several like in the polling pattern
            try
            {
                //If we can't open the file, it's still copying
                using (var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                return false;
            }
        }
        private void Created(string folder)
        {
            if (!Directory.Exists($"{DestPath}/{folder}"))
            {
                Directory.CreateDirectory($"{DestPath}/{folder}");
            }
        }


        private void CopyFil(string source,string dest,string ext)
        {
                if(!FileIsReady($"{SourcePath}/{source}")) return;
                File.Copy($"{SourcePath}/{source}", $"{DestPath}/{ext}/{Guid.NewGuid()}{dest}");
                File.Delete($"{SourcePath}/{source}");
            eventLog.WriteEntry($"Copy {SourcePath}/{source}, {DestPath}/{ext}/{Guid.NewGuid()}{dest}");

        }

    }
}
