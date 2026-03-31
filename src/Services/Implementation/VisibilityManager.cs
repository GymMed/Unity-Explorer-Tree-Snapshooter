using UnityEngine.EventSystems;
using UniverseLib.UI;

namespace UnityExplorerTreeSnapShooter.Services.Implementation
{
    public class VisibilityManager : IVisibilityManager
    {
        private static VisibilityManager _instance;
        public static VisibilityManager Instance => _instance ??= new VisibilityManager();

        public bool IsVisible { get; private set; }

        private UIBase _uiBase;

        private VisibilityManager() { }

        public void Initialize(UIBase uiBase)
        {
            _uiBase = uiBase;
        }

        public void Toggle()
        {
            SetVisible(!IsVisible);
        }

        public void SetVisible(bool visible)
        {
            if (IsVisible == visible) return;

            IsVisible = visible;

            if (_uiBase != null)
            {
                _uiBase.Enabled = visible;
            }

            if (!visible)
            {
                EventSystem.current?.SetSelectedGameObject(null);
            }
        }
    }
}
