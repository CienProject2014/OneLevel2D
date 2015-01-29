using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneLevelJson
{
    public class Command
    {
        protected string name;
    }

    public class MoveCommand : Command
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

    }
}
