using UnityExplorerTreeSnapShooter.Models;

namespace UnityExplorerTreeSnapShooter.Services
{
    public interface ISnapshotFormatter
    {
        string Format(SnapshotResult snapshot);
    }
}
