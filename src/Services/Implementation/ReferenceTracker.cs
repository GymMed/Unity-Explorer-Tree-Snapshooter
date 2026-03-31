using System.Collections.Generic;
using UnityEngine;
using UnityExplorerTreeSnapShooter.Services;

namespace UnityExplorerTreeSnapShooter.Services.Implementation
{
    public class ReferenceTracker : IReferenceTracker
    {
        private readonly HashSet<int> _selectionInstanceIds = new();

        public void AddToSelection(int instanceId)
        {
            _selectionInstanceIds.Add(instanceId);
        }

        public bool IsInSelection(int instanceId)
        {
            return _selectionInstanceIds.Contains(instanceId);
        }

        public bool IsInSelection(UnityEngine.Object obj)
        {
            if (obj == null) return false;
            return _selectionInstanceIds.Contains(obj.GetInstanceID());
        }

        public void Clear()
        {
            _selectionInstanceIds.Clear();
        }
    }
}
