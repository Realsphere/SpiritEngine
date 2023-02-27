using Realsphere.Spirit.Rendering;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.RGUI
{
    public class RLabel : RControl
    {
        public string Text;
        public string FontName = "Arial";
        public float FontSize = 12f;
        public SColor Color = SColor.White;

        public override void Render(Device device, DeviceContext context)
        {
            if (Text == null) return;

            using(var factory = new Factory())
            {
                using(var dwriteFactory = new SharpDX.DirectWrite.Factory())
                {
                    using(var textFormat = new SharpDX.DirectWrite.TextFormat(dwriteFactory, FontName, FontSize))
                    {
                        context.TextAntialiasMode = TextAntialiasMode.Grayscale;
                        var textLayout = new SharpDX.DirectWrite.TextLayout(dwriteFactory, Text, textFormat, 3840, 2680);
                        var e = new SolidColorBrush(context, new(Color.R, Color.G, Color.B, Color.A));
                        context.DrawTextLayout(new SharpDX.Vector2(Position.X, Position.Y), textLayout, e);
                        e.Dispose();
                    }
                }
            }
        }
    }
}
