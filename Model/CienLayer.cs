namespace OneLevelJson.Model
{
    public class CienLayer
    {
        public CienLayer(string name) : this(name, true, false) {}
        public CienLayer(string name, bool isVisible, bool isLocked)
        {
            Name = name;
            IsVisible = isVisible;
            IsLocked = isLocked;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void VisibleToggle()
        {
            IsVisible = !IsVisible;
        }

        public void LockToggle()
        {
            IsLocked = !IsLocked;
        }

        public string Name { get; private set; }
        public bool IsVisible { get; private set; }
        public bool IsLocked { get; private set; }

        public static CienLayer Empty = new CienLayer(EmptyLayerName);

        public const string DefaultLayerName = "Default";
        private const string EmptyLayerName = "Empty";
    }
}
