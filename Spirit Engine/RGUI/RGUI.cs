using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.RGUI
{
    public class RGUI
    {
        Device device;

        public List<RControl> Controls { get; } = new();

        public RGUI(Device device) 
        {
            this.device = device;
        }

        internal void Render(DeviceContext context)
        {
            context.BeginDraw();
            try
            {
                Controls.ForEach(x => x.Render(device, context));
            }
            catch (InvalidOperationException) { }
            context.EndDraw();
        }
    }
}
