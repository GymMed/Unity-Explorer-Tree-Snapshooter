using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityExplorerTreeSnapShooter.Models;
using UnityExplorerTreeSnapShooter.Services;

namespace UnityExplorerTreeSnapShooter.Services.Implementation
{
    public class ComponentStateReader : IComponentStateReader
    {
        private readonly IReferenceTracker _tracker;

        public ComponentStateReader(IReferenceTracker tracker)
        {
            _tracker = tracker;
        }

        public List<ComponentSnapshot> ReadComponents(GameObject gameObject, IReferenceTracker tracker)
        {
            var snapshots = new List<ComponentSnapshot>();
            Component[] components = gameObject.GetComponents<Component>();

            foreach (Component comp in components)
            {
                if (comp == null) continue;

                var snapshot = new ComponentSnapshot
                {
                    TypeName = comp.GetType().Name,
                    InstanceId = comp.GetInstanceID(),
                    Members = ReadMembers(comp, tracker)
                };

                snapshots.Add(snapshot);
            }

            return snapshots;
        }

        public List<GameObjectSnapshot> ReadAllGameObjects(List<SnapshotNode> hierarchyNodes, int selectedInstanceId, IReferenceTracker tracker)
        {
            var result = new List<GameObjectSnapshot>();
            var processedGameObjects = new HashSet<int>();

            foreach (var node in hierarchyNodes)
            {
                GameObject go = GetGameObjectByInstanceId(node.InstanceId);
                if (go == null) continue;

                bool isSelected = node.InstanceId == selectedInstanceId;
                var goSnapshot = new GameObjectSnapshot
                {
                    Name = go.name,
                    InstanceId = node.InstanceId,
                    IsSelected = isSelected,
                    Components = new List<ComponentSnapshot>()
                };

                if (processedGameObjects.Contains(node.InstanceId))
                {
                    goSnapshot.IsDuplicate = true;
                    result.Add(goSnapshot);
                    continue;
                }
                processedGameObjects.Add(node.InstanceId);

                Component[] components = go.GetComponents<Component>();
                foreach (Component comp in components)
                {
                    if (comp == null) continue;

                    int compInstanceId = comp.GetInstanceID();

                    var compSnapshot = new ComponentSnapshot
                    {
                        TypeName = comp.GetType().Name,
                        InstanceId = compInstanceId,
                        Members = ReadMembers(comp, tracker)
                    };

                    goSnapshot.Components.Add(compSnapshot);
                }

                result.Add(goSnapshot);
            }

            return result;
        }

        private GameObject GetGameObjectByInstanceId(int instanceId)
        {
            foreach (GameObject go in UnityEngine.Object.FindObjectsOfType<GameObject>())
            {
                if (go.GetInstanceID() == instanceId)
                    return go;
            }
            return null;
        }

        private List<MemberSnapshot> ReadMembers(Component component, IReferenceTracker tracker)
        {
            var members = new List<MemberSnapshot>();
            Type type = component.GetType();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (FieldInfo field in type.GetFields(flags))
            {
                try
                {
                    object value = field.GetValue(component);
                    var memberSnapshot = CreateMemberSnapshot("field", field.Name, value, field.FieldType, tracker);
                    members.Add(memberSnapshot);
                }
                catch
                {
                    members.Add(new MemberSnapshot
                    {
                        MemberType = "field",
                        MemberName = field.Name,
                        FormattedValue = "[Error reading value]"
                    });
                }
            }

            foreach (PropertyInfo prop in type.GetProperties(flags))
            {
                try
                {
                    if (prop.GetIndexParameters().Length > 0) continue;

                    object value = prop.GetValue(component, null);
                    var memberSnapshot = CreateMemberSnapshot("property", prop.Name, value, prop.PropertyType, tracker);
                    members.Add(memberSnapshot);
                }
                catch
                {
                    members.Add(new MemberSnapshot
                    {
                        MemberType = "property",
                        MemberName = prop.Name,
                        FormattedValue = "[Error reading value]"
                    });
                }
            }

            return members;
        }

        private MemberSnapshot CreateMemberSnapshot(string memberType, string memberName, object value, Type valueType, IReferenceTracker tracker)
        {
            var snapshot = new MemberSnapshot
            {
                MemberType = memberType,
                MemberName = memberName,
                ValueType = valueType?.Name ?? "null"
            };

            if (value == null)
            {
                snapshot.FormattedValue = "null";
                return snapshot;
            }

            if (value is UnityEngine.Object unityObj)
            {
                if (unityObj == null)
                {
                    snapshot.FormattedValue = "null";
                    return snapshot;
                }

                int instanceId = unityObj.GetInstanceID();
                snapshot.ReferencedInstanceId = instanceId;

                if (tracker.IsInSelection(instanceId))
                {
                    snapshot.FormattedValue = $"[InstanceID: {instanceId}] {unityObj.name} ({valueType.Name})";
                    snapshot.IsOutsideReference = false;
                }
                else
                {
                    snapshot.FormattedValue = $"[OUTSIDE_REFERENCE: InstanceID={instanceId}, Type={valueType.Name}] {unityObj.name}";
                    snapshot.IsOutsideReference = true;
                }

                return snapshot;
            }

            snapshot.FormattedValue = FormatPrimitiveValue(value);
            return snapshot;
        }

        private string FormatPrimitiveValue(object value)
        {
            if (value is string str)
            {
                return $"\"{str}\"";
            }

            if (value.GetType().IsPrimitive || value.GetType().IsEnum)
            {
                return value.ToString();
            }

            if (value is Array arr)
            {
                return $"Array[{arr.Length}]";
            }

            if (value is System.Collections.IEnumerable enumerable && !(value is string))
            {
                int count = 0;
                foreach (var _ in enumerable) count++;
                return $"IEnumerable[{count}]";
            }

            return $"({value.GetType().Name}) {value}";
        }
    }
}
