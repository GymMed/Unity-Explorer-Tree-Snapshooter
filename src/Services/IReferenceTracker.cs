using UnityEngine;

namespace UnityExplorerTreeSnapShooter.Services
{
    public interface IReferenceTracker
    {
        void AddToSelection(int instanceId);
        bool IsInSelection(int instanceId);
        bool IsInSelection(UnityEngine.Object obj);
        void Clear();
    }
}
