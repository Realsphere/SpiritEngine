using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.Rendering;

namespace Realsphere.Spirit.SceneManagement
{
    public class Light
    {
        public SColor LightColor { get; set; }
        public SVector3 LightDirection { get; set; }

        public Light()
        {
            LightColor = new SColor(255f, 255f, 255f, 1f);
            LightDirection = new SVector3(1f, -1f, -1f);
        }
    }
}
