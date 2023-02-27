using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.SceneManagement;

namespace Realsphere.Spirit.Physics
{
    public static class SPhysics
    {
        public static GameObject RayCast(SVector3 pos, SVector3 direction, float distance = 100f)
        {
            return Internal.PhysicsEngine.raycast(pos, direction, distance);
        }
    }
}
