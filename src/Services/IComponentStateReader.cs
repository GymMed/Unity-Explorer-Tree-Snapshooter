using System.Collections.Generic;
using UnityExplorerTreeSnapShooter.Models;
using UnityEngine;

namespace UnityExplorerTreeSnapShooter.Services
{
    public interface IComponentStateReader
    {
        List<ComponentSnapshot> ReadComponents(GameObject gameObject, IReferenceTracker tracker);
        List<GameObjectSnapshot> ReadAllGameObjects(List<SnapshotNode> hierarchyNodes, int selectedInstanceId, IReferenceTracker tracker);
    }
}
