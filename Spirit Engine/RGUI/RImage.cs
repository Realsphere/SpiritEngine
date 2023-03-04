using Realsphere.Spirit.Rendering;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.RGUI
{
    public class RImage : RControl
    {
        public STexture Image;
        public float Opacity = 1f;

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
            if(Image != null && Image.d2dtext != null) context.DrawBitmap(Image.d2dtext, Opacity, BitmapInterpolationMode.Linear);
        }
    }
}
