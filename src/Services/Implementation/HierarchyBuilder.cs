using System.Collections.Generic;
using UnityEngine;
using UnityExplorerTreeSnapShooter.Models;

namespace UnityExplorerTreeSnapShooter.Services.Implementation
{
    public class HierarchyBuilder : IHierarchyBuilder
    {
        private readonly List<SnapshotNode> _nodes = new();

        public void BuildHierarchy(Transform root, IReferenceTracker tracker)
        {
            _nodes.Clear();
            BuildNode(root, 0, tracker);
        }

        private void BuildNode(Transform transform, int depth, IReferenceTracker tracker)
        {
            GameObject go = transform.gameObject;
            int instanceId = go.GetInstanceID();

            tracker.AddToSelection(instanceId);

            var node = new SnapshotNode
            {
                Name = go.name,
                InstanceId = instanceId,
                TypeName = typeof(GameObject).Name,
                Depth = depth,
                IsActive = go.activeInHierarchy
            };

            _nodes.Add(node);

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                BuildNode(child, depth + 1, tracker);
            }
        }

        public List<SnapshotNode> GetHierarchyNodes()
        {
            return new List<SnapshotNode>(_nodes);
        }

        public List<SnapshotNode> BuildParentChain(Transform transform, IReferenceTracker tracker)
        {
            var parentChain = new List<SnapshotNode>();
            var parents = new List<Transform>();

            Transform current = transform.parent;
            while (current != null)
            {
                parents.Add(current);
                current = current.parent;
            }

            parents.Reverse();

            for (int i = 0; i < parents.Count; i++)
            {
                Transform parent = parents[i];
                GameObject go = parent.gameObject;
                int instanceId = go.GetInstanceID();

                tracker.AddToSelection(instanceId);

                var node = new SnapshotNode
                {
                    Name = go.name,
                    InstanceId = instanceId,
                    TypeName = typeof(GameObject).Name,
                    Depth = i,
                    IsActive = go.activeInHierarchy
                };

                parentChain.Add(node);
            }

            return parentChain;
        }
    }
}
