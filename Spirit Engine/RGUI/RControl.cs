using Realsphere.Spirit.Mathematics;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.RGUI
{
    public abstract class RControl
    {
        public EventHandler LeftClick;
        public EventHandler RightClick;
        public EventHandler HoverStart;
        public EventHandler HoverEnd;
        internal bool hovering = false;
        public SVector2 Position = new();
        public SVector2 Size = new(1f, 1f);

        public abstract void Render();
    }
}
