namespace UnityExplorerTreeSnapShooter.Services
{
    public interface ISnapshotSaver
    {
        string Save(string content, string gameObjectName);
        string Save(string content, string gameObjectName, string userProvidedPath);
        string GetDefaultDirectory();
    }
}
