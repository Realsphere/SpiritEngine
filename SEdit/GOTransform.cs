using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEdit
{
    public partial class GOTransform : Form
    {
        public GOTransform()
        {
            InitializeComponent();
        }

        private void GOTransform_Load(object sender, EventArgs e)
        {
            Text = EditorWindow.selObj.Name + " - Transform";
            Icon = Properties.Resources.Spirit_Logo1;
            posX.Value = (decimal)EditorWindow.selObj.Transform.Position.X;
            posY.Value = (decimal)EditorWindow.selObj.Transform.Position.Y;
            posZ.Value = (decimal)EditorWindow.selObj.Transform.Position.Z;
            scaleX.Value = (decimal)EditorWindow.selObj.Transform.Scale.X;
            scaleY.Value = (decimal)EditorWindow.selObj.Transform.Scale.Y;
            scaleZ.Value = (decimal)EditorWindow.selObj.Transform.Scale.Z;
            rotX.Value = (decimal)EditorWindow.selObj.Transform.Rotation.X;
            rotY.Value = (decimal)EditorWindow.selObj.Transform.Rotation.Y;
            rotZ.Value = (decimal)EditorWindow.selObj.Transform.Rotation.Z;
            timer1.Start();
        }

        public void Apply()
        {
            try
            {
                EditorWindow.selObj.Transform.Position = new((float)posX.Value, (float)posY.Value, (float)posZ.Value);
                EditorWindow.selObj.Transform.Scale = new((float)scaleX.Value, (float)scaleY.Value, (float)scaleZ.Value);
                EditorWindow.selObj.Transform.Rotation = new((float)rotX.Value, (float)rotY.Value, (float)rotZ.Value, 0f);
            }catch(NullReferenceException) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            Apply();
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            EditorWindow.selObj.Transform.Position = new((float)posX.Value, (float)posY.Value, (float)posZ.Value);
            EditorWindow.selObj.Transform.Scale = new((float)scaleX.Value, (float)scaleY.Value, (float)scaleZ.Value);
            EditorWindow.selObj.Transform.Rotation = new((float)rotX.Value, (float)rotY.Value, (float)rotZ.Value, 0f);
        }
    }
}
