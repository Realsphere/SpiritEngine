using Realsphere.Spirit.Internal;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.Rendering;

namespace Realsphere.Spirit.SceneManagement
{
    public abstract class Light
    {
        public SColor LightColor { get; set; }
        internal uint type;
    }

    public class PointLight : Light
    {
        public SVector3 Position { get; set; }
        public float Intensity { get; set; }

        public PointLight()
        {
            type = 2;
        }
    }

    public class DirectionalLight : Light
    {
        public SVector3 Direction { get; set; }

        public DirectionalLight()
        {
            type = 1;
        }
    }
}
