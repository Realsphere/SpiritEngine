using DrawingEx.ColorManagement;
using Microsoft.VisualBasic;
using Realsphere.Spirit;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.Physics;
using Realsphere.Spirit.Rendering;
using Realsphere.Spirit.SceneManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace SEdit
{
    public partial class EditorWindow : Form
    {
        public static GameObject selObj;

        public EditorWindow()
        {
            InitializeComponent();
            Closed += EditorWindow_Closed;
        }

        private void EditorWindow_Closed(object? sender, EventArgs e)
        {

        }

        private void MDIPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Game.ActiveScene.GameObjects.Count > 0 || Game.ActiveScene.SkyBoxColor != new SColor(120f, 120f, 120f, 255f)) 
                if (MessageBox.Show("Are you sure?\nAll unsafed progress will be lost!", "SEdit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            Data.Scene = new();
            Game.ActiveScene = Data.Scene;
            Data.Scene.SkyBoxColor = new SColor(120f, 120f, 120f, 255f);
        }

        private void skyboxColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialogEx color = new();
            color.ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new();
            sfd.Filter = "SEdit Scene Files | *.sescene";
            sfd.FileName = "";
            sfd.InitialDirectory = $"C:\\Users\\{Environment.UserName}\\Desktop";
            sfd.RestoreDirectory = true;
            sfd.Title = "SEdit - Save Scene";
            sfd.ShowDialog();

            if (!IsDirectoryWritable(Path.GetDirectoryName(sfd.FileName))) return;

            msg.Text = "Saving to " + sfd.FileName;
            BinarySerialization.WriteToBinaryFile(sfd.FileName, Data.Scene);
            msg.Text = "Saved!";
        }

        public bool IsDirectoryWritable(string dirPath, bool throwIfFails = false)
        {
            if (dirPath == null) return false;
            if (!Directory.Exists(dirPath)) return false;
            try
            {
                using (FileStream fs = File.Create(
                    Path.Combine(
                        dirPath,
                        Path.GetRandomFileName()
                    ),
                    1,
                    FileOptions.DeleteOnClose)
                )
                { }
                return true;
            }
            catch
            {
                if (throwIfFails)
                    throw;
                else
                    return false;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new();
            sfd.Filter = "SEdit Scene Files | *.sescene";
            sfd.FileName = "";
            sfd.InitialDirectory = $"C:\\Users\\{Environment.UserName}\\Desktop";
            sfd.RestoreDirectory = true;
            sfd.Title = "SEdit - Open Scene";
            sfd.ShowDialog();

            if (!File.Exists(sfd.FileName)) return;

            Data.Scene = BinarySerialization.ReadFromBinaryFile<Scene>(sfd.FileName);
            Game.ActiveScene = Data.Scene;
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new();
            sfd.Filter = "C# Code files | *.cs";
            sfd.FileName = "";
            sfd.InitialDirectory = $"C:\\Users\\{Environment.UserName}\\Desktop";
            sfd.RestoreDirectory = true;
            sfd.Title = "SEdit - Export Scene";
            sfd.ShowDialog();

            if (!IsDirectoryWritable(Path.GetDirectoryName(sfd.FileName))) return;

            msg.Text = "Exporting to " + sfd.FileName;

            using(StreamWriter sw = new(sfd.FileName))
            {
                string sceneName = Path.GetFileNameWithoutExtension(sfd.FileName);
                sw.WriteLine("using System;");
                sw.WriteLine("using Realsphere.Spirit;");
                sw.WriteLine("using Realsphere.Spirit.Mathematics;");
                sw.WriteLine("using Realsphere.Spirit.SceneManagement;");
                sw.WriteLine("");
                sw.WriteLine("public static class " + sceneName.Replace(" ", "_") + " {");
                sw.WriteLine("public static Scene Generate() {");
                sw.WriteLine("Scene val = new Scene();");
                sw.WriteLine("val.SkyBoxColor = new SColor(" + Data.Scene.SkyBoxColor.R + "f, " + Data.Scene.SkyBoxColor.G + "f, " + Data.Scene.SkyBoxColor.B + "f, " + Data.Scene.SkyBoxColor.A + "f)");
                foreach (GameObject go in Data.Scene.GameObjects)
                {
                    sw.WriteLine("GameObject go_" + go.Name.Trim() + " = new(\"" + go.Name + "\");");
                    sw.WriteLine("go.Weight = " + go.Weight + "f;");
                    sw.WriteLine("go.Force = new SVector3(" + go.Force.X + "f, " + go.Force.Y + "f, " + go.Force.Z + "f);");
                    sw.WriteLine("go.Transform.Position = new SVector3(" + go.Transform.Position.X + "f, " + go.Transform.Position.Y + "f, " + + go.Transform.Position.Z + "f);");
                    sw.WriteLine("go.Transform.Rotation = new SQuaternion(" + go.Transform.Rotation.X + "f, " + go.Transform.Rotation.Y + "f, " + + go.Transform.Rotation.Z + "f, " + go.Transform.Rotation.W + "f);");
                    sw.WriteLine("go.Transform.Scale = new SVector3(" + go.Transform.Scale.X + "f, " + go.Transform.Scale.Y + "f, " + + go.Transform.Scale.Z + "f);");
                    sw.WriteLine("val.GameObjects.Add(go);");
                }
                sw.WriteLine("return val;");
                sw.WriteLine("}");
                sw.WriteLine("}");
            }

            msg.Text = "Exported!";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameObject obj = GameObject.CreateUsingMesh(StandarizedShapes.Cube, "Unnamed Game Object " + Data.Scene.GameObjects.Where(x => x.Name.StartsWith("Unnamed Game Object")).Count());
            obj.Transform.Scale = new SVector3(1f, 1f, 1f);
            Data.Scene.GameObjects.Add(obj);
            Game.ActiveScene = Data.Scene;
        }

        private void button5_MouseHover(object sender, EventArgs e)
        {

        }

        private void EditorWindow_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 0;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(this.button5, "Select a Game Object.");
            toolTip1.SetToolTip(this.button1, "Create a Game Object.");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GOSelect gos = new GOSelect();
            selObj = gos.SelectObj(this);
            gos.Dispose();
            if (selObj == null) return;
            selobjToolStripMenuItem.Text = selObj.Name;
            selobjToolStripMenuItem.Enabled = true;
        }

        private void transformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GOTransform got = new();
            got.Show();
        }

        private void dataToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selobjToolStripMenuItem.Text = "No Object Selected";
            selobjToolStripMenuItem.Enabled = false;
            Data.Scene.GameObjects.Remove(selObj);
            Game.ActiveScene = Data.Scene;
            selObj = null;
        }
    }
}