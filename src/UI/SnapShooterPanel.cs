using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityExplorer;
using UnityExplorer.Inspectors;
using UnityExplorer.UI.Panels;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Panels;
using UniverseLib.UI.Widgets;

namespace UnityExplorerTreeSnapShooter.UI
{
    public class SnapShooterPanel : PanelBase
    {
        public static SnapShooterPanel Instance { get; private set; }

        public static readonly string UIBaseGuid = "gymmed.unity_tree_snap_shooter.SnapShooterInspector";
        public static UIBase UIBaseRegistry { get; set; }

        public override string Name => "Tree Snap Shooter";
        public override int MinWidth => 420;
        public override int MinHeight => 350;
        public override Vector2 DefaultAnchorMin => new Vector2(0.5f, 0.5f);
        public override Vector2 DefaultAnchorMax => new Vector2(0.5f, 0.5f);

        public InputFieldRef SavePath { get; private set; }
        public Text SelectionText { get; private set; }

        private Text _logText;
        private Services.ISnapShooterService _snapShooterService;

        public override bool CanDragAndResize => true;

        public SnapShooterPanel(UIBase owner) : base(owner)
        {
            Instance = this;
        }

        protected override void ConstructPanelContent()
        {
            if (_snapShooterService == null)
            {
                var pathsManager = Services.Implementation.PathsManager.Instance;
                var saver = new Services.Implementation.FileSnapshotSaver(pathsManager);
                _snapShooterService = new Services.TreeSnapShooterService(saver);
            }

            SelectionText = UIFactory.CreateLabel(ContentRoot, "Header", GetSelectionMessage(), TextAnchor.MiddleCenter);

            UIFactory.CreateLabel(ContentRoot, "SaveLabel", "Save Path:");
            SavePath = UIFactory.CreateInputField(ContentRoot, "SavePathField", "Save to path...");
            UIFactory.SetLayoutElement(SavePath.Component.gameObject, minHeight: 25, flexibleWidth: 9999);
            SavePath.Text = GetDefaultSavePath();

            var snapShootBtn = UIFactory.CreateButton(
                ContentRoot, 
                "SnapshotBtn", 
                "Snapshot Selection", 
                new Color(0.2f, 0.2f, 0.35f)
            );
            UIFactory.SetLayoutElement(snapShootBtn.Component.gameObject, minHeight: 30, flexibleWidth: 9999);
            snapShootBtn.OnClick += OnSnapShoot;

            GameObject scrollContent;
            AutoSliderScrollbar autoScrollbar;

            UIFactory.CreateLabel(ContentRoot, "LogLabel", "Output:");
            GameObject scrollView = UIFactory.CreateScrollView(
                ContentRoot, 
                "LogScroll", 
                out scrollContent, 
                out autoScrollbar, 
                new Color(0.2f, 0.2f, 0.2f)
            );
            UIFactory.SetLayoutElement(scrollView, flexibleHeight: 9999, minHeight: 120);

            _logText = scrollContent.AddComponent<Text>();
            _logText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            _logText.fontSize = 12;
            _logText.color = Color.white;
            _logText.supportRichText = true;
            _logText.horizontalOverflow = HorizontalWrapMode.Wrap;
            _logText.verticalOverflow = VerticalWrapMode.Overflow;
            _logText.text = "Ready...";

            InspectorManager.OnInspectedTabsChanged += OnInspectorChanged;
        }

        public string GetDefaultSavePath()
        {
            string gameObjectName = GetSelectionName();
            string date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string filename = $"TreeSnapShooter_{gameObjectName}_{date}.txt";
            return Path.Combine(_snapShooterService.GetDefaultDirectory(), filename);
        }

        private void OnInspectorChanged()
        {
            if (!this.Enabled) return;
            UpdateSelection();
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (!active) return;
            UpdateSelection();
        }

        protected override void OnClosePanelClicked()
        {
            Services.Implementation.VisibilityManager.Instance.Toggle();
        }

        public void UpdateSelection()
        {
            if (SelectionText == null) return;
            SelectionText.text = GetSelectionMessage();
            
            if (SavePath?.Component != null)
            {
                SavePath.Text = GetDefaultSavePath();
            }
        }

        public string GetSelectionMessage()
        {
            return $"<b>Selection: {GetSelectionName()}</b>";
        }

        public string GetSelectionName()
        {
            GameObject target = GetInspectedGameObject();
            if (target != null) return target.name;
            return "None";
        }

        private void OnSnapShoot()
        {
            if (SavePath == null || SavePath.Component == null)
            {
                Log("ERROR: SavePath is null!");
                return;
            }

            string userPath = SavePath.Component.text;

            GameObject gameObj = GetInspectedGameObject();
            if (gameObj != null)
            {
                try
                {
                    string output = _snapShooterService.ExecuteSnapShoot(gameObj);
                    string savedPath = _snapShooterService.SaveSnapshot(output, gameObj.name, userPath);
                    Log($"Successfully created snapshot: {savedPath}");
                }
                catch (Exception ex)
                {
                    Log($"Snapshot failed: {ex.Message}");
                }
            }
            else
            {
                Log("Target is not a GameObject!");
            }
        }

        private void Log(string msg)
        {
            if (_logText != null)
            {
                _logText.text = msg + "\n" + _logText.text;
            }
        }

        private GameObject GetInspectedGameObject()
        {
            var activeInspector = InspectorManager.ActiveInspector;
            if (activeInspector != null)
            {
                return TryGetGameObjectFromInspector(activeInspector);
            }

            if (InspectorManager.Inspectors != null)
            {
                for (int i = 0; i < InspectorManager.Inspectors.Count; i++)
                {
                    var inspector = InspectorManager.Inspectors[i];
                    GameObject go = TryGetGameObjectFromInspector(inspector);
                    if (go != null) return go;
                }
            }

            return null;
        }

        private GameObject TryGetGameObjectFromInspector(InspectorBase inspector)
        {
            if (inspector == null) return null;

            object target = inspector.Target;
            if (target == null || target.ToString() == "null") return null;

            if (target is GameObject go) return go;
            if (target is Component comp && comp.gameObject != null) return comp.gameObject;

            return null;
        }

        public override void Destroy()
        {
            base.Destroy();
            InspectorManager.OnInspectedTabsChanged -= OnInspectorChanged;
        }
    }
}
