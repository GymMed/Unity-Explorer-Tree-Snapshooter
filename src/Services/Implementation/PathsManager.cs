using System;
using System.IO;

namespace UnityExplorerTreeSnapShooter.Services.Implementation
{
    public class PathsManager : IPathsManager
    {
        private static PathsManager _instance;
        public static PathsManager Instance => _instance ??= new PathsManager();

        private readonly string _snapshotsDirectory;

        private PathsManager()
        {
            string baseDirectory = UnityExplorerTreeSnapShooter.GetProjectLocation();
            _snapshotsDirectory = Path.Combine(baseDirectory, "Snapshots");

            EnsureDirectoryExists(_snapshotsDirectory);
        }

        private void EnsureDirectoryExists(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    UnityExplorerTreeSnapShooter.LogMessage($"Created directory: {path}");
                }
            }
            catch (Exception ex)
            {
                UnityExplorerTreeSnapShooter.LogMessage($"Error creating directory {path}: {ex.Message}");
            }
        }

        public string GetSnapshotsDirectory() => _snapshotsDirectory;
    }
}
