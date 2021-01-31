using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace CB.Utils
{
    /// <summary>
    /// Watches for file and directory changes for specified directory paths so changes can be made on the fly
    /// </summary>
    [System.Serializable]
    public class DirectoryWatcher
    {
        public string[] Paths;
        public string[] FileExtensions;

        /*Callbacks*/
        public event Action<string, string> OnDirectoryChanged;
        public event Action<string, string> OnFileChanged;
        public event Action<string, string> OnDeleted;

        /*Cache*/
        private List<FileSystemWatcher> Watchers = new List<FileSystemWatcher>();

        public DirectoryWatcher(string[] paths, string[] fileExtensions)
        {
            Paths = paths;
            FileExtensions = fileExtensions;
        }

        #region Listeners
        async UniTask OnChanged(object source, FileSystemEventArgs args, string root)
        {
            await UniTask.SwitchToMainThread();
            //Change
            if (File.Exists(args.FullPath) || Directory.Exists(args.FullPath))
            {
                //Directory change
                if (File.GetAttributes(args.FullPath) == FileAttributes.Directory) OnDirectoryChanged?.Invoke(args.FullPath, root);
                //File change
                else
                {
                    string fileExtension = $"*{Path.GetExtension(args.FullPath)}";
                    for (int i = 0; i < FileExtensions.Length; i++)
                    {
                        if (fileExtension == FileExtensions[i])
                        {
                            OnFileChanged?.Invoke(args.FullPath, root);
                            break;
                        }
                    }
                }
            }
            //Remove
            else OnDeleted?.Invoke(args.FullPath, root);
        }
        #endregion

        #region Utils
        public void Load()
        {
            for (int i = 0; i < Paths.Length; i++) ProcessDirectory(Path.GetFullPath(Paths[i]), Path.GetFullPath(Paths[i]));
        }

        public void Watch()
        {
            Dispose();
            for (int i = 0; i < Paths.Length; i++) WatchDirectory(Path.GetFullPath(Paths[i]));
        }

        public void Dispose()
        {
            for (int i = 0; i < Watchers.Count; i++) Watchers[i].Dispose();
            Watchers.Clear();
        }
        #endregion

        #region Function
        void WatchDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            FileSystemWatcher watcher = new FileSystemWatcher(path, "*.*");
            //watcher.SynchronizingObject = this;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.IncludeSubdirectories = true;

            //Set listeners
            watcher.Changed += new FileSystemEventHandler((sender, e) => OnChanged(sender, e, path).Forget());
            watcher.Created += new FileSystemEventHandler((sender, e) => OnChanged(sender, e, path).Forget());
            watcher.Deleted += new FileSystemEventHandler((sender, e) => OnChanged(sender, e, path).Forget());
            watcher.Renamed += new RenamedEventHandler((sender, e) => OnChanged(sender, e, path).Forget());

            //Start watching
            watcher.EnableRaisingEvents = true;
            Watchers.Add(watcher);
        }

        void ProcessDirectory(string path, string root)
        {
            OnDirectoryChanged?.Invoke(path, root);

            //Recursive branching
            string[] subFolders = Directory.GetDirectories(path);
            for (int i = 0; i < subFolders.Length; i++) ProcessDirectory(subFolders[i], root);

            for (int i = 0; i < FileExtensions.Length; i++)
            {
                string[] files = Directory.GetFiles(path, FileExtensions[i]);
                for (int j = 0; j < files.Length; j++) OnFileChanged?.Invoke(files[j], root);
            }
        }
        #endregion
    }
}
