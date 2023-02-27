using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.RenderingCommon;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Modelling
{
    public class SModel
    {
        internal Mesh mesh;
        internal Vector2[] ssuvs;

        public SVector3[] Vertices
        {
            get
            {
                List<SVector3> verts = new List<SVector3>();
                foreach (var vb in mesh.VertexBuffers)
                {
                    foreach (var vert in vb)
                    {
                        verts.Add(vert.Position);
                    }
                }
                return verts.ToArray();
            }
        }

        public float[] Points
        {
            get => mesh.points.ToArray();
        }

        public uint[] Indices
        {
            get
            {
                List<uint> verts = new List<uint>();
                foreach (var vb in mesh.IndexBuffers)
                {
                    foreach (var indx in vb)
                    {
                        verts.Add(indx);
                    }
                }
                return verts.ToArray();
            }
        }

        public static SModel FromCMO(string cmo)
        {
            SModel sm = new SModel();
            sm.mesh = Mesh.LoadFromFile(cmo).First();
            return sm;
        }

        public static SModel FromBits(byte[] bits)
        {
            SModel sm = new SModel();
            sm.mesh = Mesh.LoadFromBits(bits).FirstOrDefault();
            return sm;
        }
    }
}
