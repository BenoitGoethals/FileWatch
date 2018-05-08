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
        private readonly FileSystemWatcher _watcher=new FileSystemWatcher();

        public string SourcePath { get; set; }

        public string DestPath { get; set; }

        public void Watch()
        {

            _watcher.Path = SourcePath;
           
            _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                                            | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _watcher.Filter = "*.*";
          
            _watcher.Created += OnChanged;

            _watcher.Changed += OnChanged;
            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
           if( File.Exists(e.FullPath))
            {
            string extension = Path.GetExtension(e.Name)?.Substring(1);
            if (!String.IsNullOrEmpty(extension))
            {
                Created(extension);
                CopyFil(e.Name, e.Name, extension);
            }
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
            using (_watcher)
            {
                File.Copy($"{SourcePath}/{source}", $"{DestPath}/{ext}/{Guid.NewGuid()}{dest}");
                File.Delete($"{SourcePath}/{source}");
            }
            
        }

    }
}
