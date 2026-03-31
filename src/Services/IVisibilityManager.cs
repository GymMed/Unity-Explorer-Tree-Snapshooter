using UniverseLib.UI;

namespace UnityExplorerTreeSnapShooter.Services
{
    public interface IVisibilityManager
    {
        bool IsVisible { get; }
        void Initialize(UIBase uiBase);
        void Toggle();
        void SetVisible(bool visible);
    }
}
