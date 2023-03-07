using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.RenderingCommon;
using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Modelling
{
    public class SModel
    {
        internal Mesh[] meshes;
        internal Vector2[] ssuvs;

        public SVector3[] Vertices
        {
            get
            {
                List<SVector3> verts = new List<SVector3>();
                foreach (var mesh in meshes)
                {
                    foreach (var vb in mesh.VertexBuffers)
                    {
                        foreach (var vert in vb)
                        {
                            verts.Add(vert.Position);
                        }
                    }
                }
                return verts.ToArray();
            }
        }

        public float[] Points
        {
            get 
            {
                List<float> verts = new List<float>();
                foreach (var mesh in meshes)
                    verts.AddRange(mesh.points);
                return verts.ToArray();
            }
        }

        public uint[] Indices
        {
            get
            {
                List<uint> verts = new List<uint>();
                foreach (var mesh in meshes)
                {
                    foreach (var vb in mesh.IndexBuffers)
                    {
                        foreach (var indx in vb)
                        {
                            verts.Add(indx);
                        }
                    }
                }
                return verts.ToArray();
            }
        }

        public static SModel FromCMO(string cmo)
        {
            SModel sm = new SModel();
            sm.meshes = Mesh.LoadFromFile(cmo).ToArray();
            return sm;
        }

        public static SModel FromBits(byte[] bits)
        {
            SModel sm = new SModel();
            sm.meshes = Mesh.LoadFromBits(bits).ToArray();
            return sm;
        }
    }
}