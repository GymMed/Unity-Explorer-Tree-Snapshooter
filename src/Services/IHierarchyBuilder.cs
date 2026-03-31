using UnityExplorerTreeSnapShooter.Models;
using UnityEngine;
using System.Collections.Generic;

namespace UnityExplorerTreeSnapShooter.Services
{
    public interface IHierarchyBuilder
    {
        void BuildHierarchy(Transform root, IReferenceTracker tracker);
        List<SnapshotNode> GetHierarchyNodes();
        List<SnapshotNode> BuildParentChain(Transform transform, IReferenceTracker tracker);
    }
}
