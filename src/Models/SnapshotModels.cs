using System.Collections.Generic;

namespace UnityExplorerTreeSnapShooter.Models
{
    public class SnapshotNode
    {
        public string Name { get; set; }
        public int InstanceId { get; set; }
        public string TypeName { get; set; }
        public List<SnapshotNode> Children { get; set; } = new();
        public int Depth { get; set; }
        public bool IsActive { get; set; }
        public bool IsOutsideReference { get; set; }
    }

    public class ComponentSnapshot
    {
        public string TypeName { get; set; }
        public int InstanceId { get; set; }
        public List<MemberSnapshot> Members { get; set; } = new();
    }

    public class GameObjectSnapshot
    {
        public string Name { get; set; }
        public int InstanceId { get; set; }
        public bool IsSelected { get; set; }
        public bool IsDuplicate { get; set; }
        public List<ComponentSnapshot> Components { get; set; } = new();
    }

    public class MemberSnapshot
    {
        public string MemberType { get; set; }
        public string MemberName { get; set; }
        public string ValueType { get; set; }
        public string FormattedValue { get; set; }
        public int? ReferencedInstanceId { get; set; }
        public bool IsOutsideReference { get; set; }
    }

    public class SnapshotResult
    {
        public string RootName { get; set; }
        public int RootInstanceId { get; set; }
        public System.DateTime GeneratedAt { get; set; }
        public List<SnapshotNode> ParentChain { get; set; } = new();
        public List<SnapshotNode> Hierarchy { get; set; } = new();
        public List<ComponentSnapshot> Components { get; set; } = new();
        public List<GameObjectSnapshot> GameObjectSnapshots { get; set; } = new();
    }
}
