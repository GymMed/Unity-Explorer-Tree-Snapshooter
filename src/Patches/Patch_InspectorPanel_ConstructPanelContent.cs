using HarmonyLib;
using UnityExplorerTreeSnapShooter.UI;
using UnityEngine;
using UnityExplorer;
using UnityExplorer.UI.Panels;
using UniverseLib.UI;

namespace UnityExplorerTreeSnapShooter.Patches
{
    [HarmonyPatch(typeof(InspectorPanel), "ConstructPanelContent")]
    public static class Patch_InspectorPanel_ConstructPanelContent 
    {
        private static SnapShooterPanel _snapShooterPanel;

        static void Postfix(InspectorPanel __instance)
        {
            if (_snapShooterPanel == null)
            {
                if (SnapShooterPanel.UIBaseRegistry == null)
                {
                    SnapShooterPanel.UIBaseRegistry = UniversalUI.RegisterUI(
                        SnapShooterPanel.UIBaseGuid,
                        null
                    );
                }

                SnapShooterPanel.UIBaseRegistry.Enabled = false;
                Services.Implementation.VisibilityManager.Instance.Initialize(SnapShooterPanel.UIBaseRegistry);

                _snapShooterPanel = new SnapShooterPanel(SnapShooterPanel.UIBaseRegistry);
            }

            GameObject closeHolder = __instance.TitleBar.transform.Find("CloseHolder").gameObject;

            var snapShootBtn = UIFactory.CreateButton(
                closeHolder,
                "SnapshotBtn",
                "Snapshot",
                new Color(0.2f, 0.2f, 0.35f)
            );

            UIFactory.SetLayoutElement(snapShootBtn.Component.gameObject, minHeight: 25, minWidth: 80);
            snapShootBtn.Component.transform.SetSiblingIndex(0);

            snapShootBtn.OnClick += () =>
            {
                Services.Implementation.VisibilityManager.Instance.Toggle();
            };

            UnityExplorerTreeSnapShooter.LogMessage("[TreeSnapShooter] Snapshoot button added to Inspector panel!");
        }
    }
}
