﻿using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.RGUI
{
    public class RGUI
    {
        public List<RControl> Controls { get; } = new();

        public RGUI(Device device) 
        {
        }

        internal void Render(DeviceContext context)
        {
            context.BeginDraw();
            try
            {
                Controls.ForEach(x => x.Render());
            }
            catch (InvalidOperationException) { }
            context.EndDraw();
        }
    }
}
