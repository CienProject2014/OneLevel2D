namespace OneLevelJson.Model
{
    public class Layer
    {
        public Layer(string name) : this(name, true) {}
        public Layer(string name, bool isLocked)
        {
            Name = name;
            IsLocked = isLocked;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public bool IsLocked { get; private set; }
    }
}
