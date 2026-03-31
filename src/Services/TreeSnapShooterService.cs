using System;
using UnityEngine;
using UnityExplorerTreeSnapShooter.Models;
using UnityExplorerTreeSnapShooter.Services;
using UnityExplorerTreeSnapShooter.Services.Implementation;

namespace UnityExplorerTreeSnapShooter.Services
{
    public class TreeSnapShooterService : ISnapShooterService
    {
        private readonly IReferenceTracker _referenceTracker;
        private readonly IHierarchyBuilder _hierarchyBuilder;
        private readonly IComponentStateReader _componentReader;
        private readonly ISnapshotFormatter _formatter;
        private readonly ISnapshotSaver _saver;

        public TreeSnapShooterService(ISnapshotSaver saver)
        {
            _referenceTracker = new ReferenceTracker();
            _hierarchyBuilder = new HierarchyBuilder();
            _componentReader = new ComponentStateReader(_referenceTracker);
            _formatter = new PlainTextSnapshotFormatter();
            _saver = saver;
        }

        public string ExecuteSnapShoot(GameObject root)
        {
            _referenceTracker.Clear();

            var snapshot = new SnapshotResult
            {
                RootName = root.name,
                RootInstanceId = root.GetInstanceID(),
                GeneratedAt = System.DateTime.Now
            };

            _hierarchyBuilder.BuildHierarchy(root.transform, _referenceTracker);
            snapshot.Hierarchy = _hierarchyBuilder.GetHierarchyNodes();

            snapshot.ParentChain = _hierarchyBuilder.BuildParentChain(root.transform, _referenceTracker);

            snapshot.Components = _componentReader.ReadComponents(root, _referenceTracker);
            snapshot.GameObjectSnapshots = _componentReader.ReadAllGameObjects(snapshot.Hierarchy, root.GetInstanceID(), _referenceTracker);

            return _formatter.Format(snapshot);
        }

        public string SaveSnapshot(string content, string gameObjectName)
        {
            return _saver.Save(content, gameObjectName);
        }

        public string SaveSnapshot(string content, string gameObjectName, string userProvidedPath)
        {
            return _saver.Save(content, gameObjectName, userProvidedPath);
        }

        public string GetDefaultDirectory()
        {
            return _saver.GetDefaultDirectory();
        }
    }
}
