using System;
using System.IO;
using UnityExplorerTreeSnapShooter.Services;

namespace UnityExplorerTreeSnapShooter.Services.Implementation
{
    public class FileSnapshotSaver : ISnapshotSaver
    {
        private readonly string _snapshotsDirectory;

        public FileSnapshotSaver(IPathsManager pathsManager)
        {
            _snapshotsDirectory = pathsManager.GetSnapshotsDirectory();
        }

        public string Save(string content, string gameObjectName)
        {
            return Save(content, gameObjectName, null);
        }

        public string Save(string content, string gameObjectName, string userProvidedPath)
        {
            string targetDirectory;
            string filename;
            string date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            if (!string.IsNullOrEmpty(userProvidedPath))
            {
                if (userProvidedPath.EndsWith(".txt") || userProvidedPath.EndsWith(".json"))
                {
                    targetDirectory = Path.GetDirectoryName(userProvidedPath);
                    filename = Path.GetFileName(userProvidedPath);
                }
                else
                {
                    targetDirectory = userProvidedPath;
                    filename = $"TreeSnapShooter_{gameObjectName}_{date}.txt";
                }
            }
            else
            {
                targetDirectory = _snapshotsDirectory;
                filename = $"TreeSnapShooter_{gameObjectName}_{date}.txt";
            }

            string fullPath = Path.Combine(targetDirectory, filename);

            try
            {
                EnsureDirectoryExists(targetDirectory);
                File.WriteAllText(fullPath, content);
                return fullPath;
            }
            catch (Exception ex)
            {
                UnityExplorerTreeSnapShooter.LogMessage($"Error saving snapshot: {ex.Message}");
                throw;
            }
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public string GetDefaultDirectory() => _snapshotsDirectory;
    }
}
