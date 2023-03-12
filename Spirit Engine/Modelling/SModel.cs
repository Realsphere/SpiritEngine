using Newtonsoft.Json;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.RenderingCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Mesh = Realsphere.Spirit.RenderingCommon.Mesh;

namespace Realsphere.Spirit.Modelling
{
    public class SModel
    {
        internal Mesh[] meshes;

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
                            verts.Add((uint)indx);
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

        public static SModel FromOBJ(string file)
        {
            if (!Path.IsPathRooted(file))
                file = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), file);
            string args = "-cmo:SPE:" + file + ":SPE:-cmo:SPE:-y:SPE:-o:SPE:" + Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + ".cmo";
            if (APIMain(args.Split(":SPE:").Length, args.Split(":SPE:")) == 0)
            {
                var model = FromCMO(Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + ".cmo");
                File.Delete(Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + ".cmo");
                return model;
            }
            else
            {
                throw new Exception("Could not load OBJ file: " + file);
            }
        }

        [DllImport("rsdx.dll", CharSet = CharSet.Unicode)]
        internal static extern int APIMain(int argc, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] argv);
    }
}