using UnityEngine;

namespace UnityExplorerTreeSnapShooter.Services
{
    public interface ISnapShooterService
    {
        string ExecuteSnapShoot(GameObject root);
        string SaveSnapshot(string content, string gameObjectName);
        string SaveSnapshot(string content, string gameObjectName, string userProvidedPath);
        string GetDefaultDirectory();
    }
}
