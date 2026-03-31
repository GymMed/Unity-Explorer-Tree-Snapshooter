using System.Text;
using UnityExplorerTreeSnapShooter.Models;

namespace UnityExplorerTreeSnapShooter.Services.Implementation
{
    public class PlainTextSnapshotFormatter : ISnapshotFormatter
    {
        private readonly StringBuilder _output = new();

        public string Format(SnapshotResult snapshot)
        {
            _output.Clear();

            AddHeader(snapshot);
            AddParentHierarchy(snapshot.ParentChain);
            AddHierarchy(snapshot.Hierarchy);
            AddComponents(snapshot.Components);
            AddGameObjectReflection(snapshot.GameObjectSnapshots);

            return _output.ToString();
        }

        private void AddHeader(SnapshotResult snapshot)
        {
            AppendLine("========================================");
            AppendLine($"TREE SNAPSHOT - {snapshot.RootName}");
            AppendLine($"Root Instance ID: {snapshot.RootInstanceId}");
            AppendLine($"Generated: {snapshot.GeneratedAt:yyyy-MM-dd HH:mm:ss}");
            AppendLine("========================================");
            AppendLine("");
        }

        private void AddParentHierarchy(System.Collections.Generic.List<SnapshotNode> parentChain)
        {
            AppendLine("=== PARENT HIERARCHY ===");
            AppendLine("");

            if (parentChain.Count == 0)
            {
                AppendLine("  (No parents - already at root)");
            }
            else
            {
                foreach (var node in parentChain)
                {
                    string indent = new string(' ', node.Depth * 2);
                    string activeIndicator = node.IsActive ? "" : " [INACTIVE]";
                    AppendLine($"{indent}{node.Name} (InstanceID: {node.InstanceId}){activeIndicator}");
                }
            }

            AppendLine("");
        }

        private void AddHierarchy(System.Collections.Generic.List<SnapshotNode> hierarchy)
        {
            AppendLine("=== CHILDREN HIERARCHY ===");
            AppendLine("");

            foreach (var node in hierarchy)
            {
                string indent = new string(' ', node.Depth * 2);
                string activeIndicator = node.IsActive ? "" : " [INACTIVE]";
                AppendLine($"{indent}[{node.Depth}] {node.Name} (InstanceID: {node.InstanceId}){activeIndicator}");
            }
        }

        private void AddComponents(System.Collections.Generic.List<ComponentSnapshot> components)
        {
            AppendLine("");
            AppendLine("=== COMPONENTS ===");
            AppendLine("");

            foreach (var comp in components)
            {
                AppendLine($"  {comp.TypeName} (InstanceID: {comp.InstanceId})");
            }

            AddComponentDetails(components);
        }

        private void AddComponentDetails(System.Collections.Generic.List<ComponentSnapshot> components)
        {
            AppendLine("");
            AppendLine("=== UNITYEXPLORER DISCOVERED STATE (REFLECTION) ===");
            AppendLine("");

            foreach (var comp in components)
            {
                AppendLine($"--- {comp.TypeName} (InstanceID: {comp.InstanceId}) ---");

                foreach (var member in comp.Members)
                {
                    string indent = "  ";
                    AppendLine($"{indent}{member.MemberType} {member.MemberName}: {member.FormattedValue}");
                }

                AppendLine("");
            }
        }

        private void AddGameObjectReflection(System.Collections.Generic.List<GameObjectSnapshot> gameObjectSnapshots)
        {
            AppendLine("");
            AppendLine("=== GAME OBJECT REFLECTION (ALL CHILDREN) ===");
            AppendLine("");

            foreach (var goSnapshot in gameObjectSnapshots)
            {
                string selectedMarker = goSnapshot.IsSelected ? " [SELECTED]" : "";
                string duplicateMarker = goSnapshot.IsDuplicate ? " [DUPLICATE - skipped]" : "";

                AppendLine($"--- {goSnapshot.Name} (InstanceID: {goSnapshot.InstanceId}){selectedMarker}{duplicateMarker} ---");

                if (goSnapshot.IsDuplicate || goSnapshot.Components.Count == 0)
                {
                    AppendLine("");
                    continue;
                }

                foreach (var comp in goSnapshot.Components)
                {
                    AppendLine($"  {comp.TypeName} (InstanceID: {comp.InstanceId}):");

                    foreach (var member in comp.Members)
                    {
                        string indent = "    ";
                        AppendLine($"{indent}{member.MemberType} {member.MemberName}: {member.FormattedValue}");
                    }

                    AppendLine("");
                }

                AppendLine("");
            }
        }

        private void AppendLine(string line)
        {
            _output.AppendLine(line);
        }
    }
}
