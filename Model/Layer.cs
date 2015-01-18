namespace OneLevelJson.Model
{
    public class Layer
    {
        public Layer(string name) : this(name, true, false) {}
        public Layer(string name, bool isVisible, bool isLocked)
        {
            Name = name;
            IsVisible = isVisible;
            IsLocked = isLocked;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetVisible(bool value)
        {
            IsVisible = value;
        }

        public string Name { get; private set; }
        public bool IsVisible { get; private set; }
        public bool IsLocked { get; private set; }
    }
}
