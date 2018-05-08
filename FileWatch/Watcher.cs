using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatch
{
    public class Watcher
    {
        FileSystemWatcher watcher;

        public string Path { get; set; }

        private void watch()
        {
            watcher = new FileSystemWatcher();
            watcher.Path = Path;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.*";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            
            watcher.EnableRaisingEvents = true;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            //Copies file to another directory.
        }

    }
}
