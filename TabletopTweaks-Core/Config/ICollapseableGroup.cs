namespace TabletopTweaks.Core.Config {
    public interface ICollapseableGroup {
        ref bool IsExpanded();
        void SetExpanded(bool value);
    }
}
