using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneLevel2D.CustomList;
using OneLevel2D.Model;

namespace OneLevel2D
{
    public abstract class Command
    {
        public string Name { get; private set; }
        public const string MOVE = "COMMAND_MOVE";
        public const string ADD = "COMMAND_ADD";
        public const string REMOVE = "COMMAND_REMOVE";

        protected Command(string name)
        {
            this.Name = name;
        }
    }

    public class MoveCommand : Command
    {
        public List<CienBaseComponent> ComponentList { get; set; } 
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public MoveCommand(string name, List<CienBaseComponent> list) : base(name)
        {
            ComponentList = new List<CienBaseComponent>(list);
        }

        public bool IsMoved()
        {
            var offset = EndPoint - (Size) StartPoint;
            return !offset.IsEmpty;
        }
    }

    public class AddCommand : Command
    {
        public List<CienBaseComponent> ComponentList { get; set; }

        public AddCommand(string name, List<CienBaseComponent> list)
            : base(name)
        {
            ComponentList = new List<CienBaseComponent>(list);
        }
    }

    public class RemoveCommand : Command
    {
        public List<CienBaseComponent> ComponentList { get; set; }

        public RemoveCommand(string name, List<CienBaseComponent> list) : base(name)
        {
            ComponentList = new List<CienBaseComponent>(list);
        }
    }
}
