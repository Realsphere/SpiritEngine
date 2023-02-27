using Realsphere.Spirit.RenderingCommon;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Internal
{
    internal class FpsCounter : SDroppable
    {
        TextRenderer rendrr;
        DateTime _lastTime;
        int _framesRendered;
        internal bool show;
        int _fps;

        public override void Drop()
        {
            rendrr.Dispose();
        }


        internal FpsCounter(D3DApplicationDesktop devmg)
        {
            rendrr = new TextRenderer("Calibri", Color.White, new Point(8, 8));
            rendrr.Initialize(devmg);
        }

        internal void Render()
        {
            if (!show) return;
            _framesRendered++;

            if ((DateTime.Now - _lastTime).TotalSeconds >= 1)
            {
                _fps = _framesRendered;
                _framesRendered = 0;
                _lastTime = DateTime.Now;
            }

            rendrr.Text = _fps + " FPS";
            rendrr.Render();
        }
    }
}
