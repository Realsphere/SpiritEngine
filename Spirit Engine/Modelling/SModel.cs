using Assimp;
using Newtonsoft.Json;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.RenderingCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
            try
            {
                Convert(file, "tmp.cmo");
                var mesh = FromCMO("tmp.cmo");
                File.Delete("tmp.cmo");
                return mesh;
            }catch(Exception ex)
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\dxmesh.exe");
                File.Delete("tmp.cmo");
                return null;
            }
        }

        static void Convert(string input, string output)
        {
            File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "\\dxmesh.exe", Properties.Resources.dxmesh);
            Process proc = new Process();
            proc.StartInfo = new()
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = AppDomain.CurrentDomain.BaseDirectory + "\\dxmesh.exe",
                Arguments = "-cmo \"" + input + "\" -o \"" + AppDomain.CurrentDomain.BaseDirectory + "\\" + output + "\" -y"
            };
            proc.Start();
            proc.WaitForExit();
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\dxmesh.exe");
        }
    }
}