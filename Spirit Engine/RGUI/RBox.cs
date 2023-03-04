using Realsphere.Spirit.Rendering;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.RGUI
{
    public class RBox : RControl
    {
        public SColor Color;
        public bool Fill = true;
        public bool Round = false;
        public float RoundRadius = 0f;

        public override void Render(IntPtr device, IntPtr context)
        {
            Device d = new(device);
            DeviceContext c = new(context);
            Render(d, c);
            d.Dispose();
            c.Dispose();
        }

        internal void Render(Device device, DeviceContext context)
        {
            SolidColorBrush scb = new(context, Color.sharpdxcolor);

            if (Fill)
                if (Round) context.FillRoundedRectangle(new RoundedRectangle()
                {
                    Rect = new SharpDX.RectangleF(Position.X, Position.Y, Size.X, Size.Y),
                    RadiusX = RoundRadius,
                    RadiusY = RoundRadius
                }, scb);
                else context.FillRectangle(new SharpDX.RectangleF(Position.X, Position.Y, Size.X, Size.Y), scb);
            else
                if (Round) context.DrawRoundedRectangle(new RoundedRectangle()
                {
                    Rect = new SharpDX.RectangleF(Position.X, Position.Y, Size.X, Size.Y),
                    RadiusX = RoundRadius,
                    RadiusY = RoundRadius
                }, scb);
            else context.DrawRectangle(new SharpDX.RectangleF(Position.X, Position.Y, Size.X, Size.Y), scb);

            scb.Dispose();
        }
    }
}
