using Realsphere.Spirit;
using Realsphere.Spirit.SceneManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEdit
{
    public partial class GOSelect : Form
    {
        public GameObject gameobj;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public GOSelect()
        {
            InitializeComponent();
        }

        private void GOSelect_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.Spirit_Logo1;
        }

        public GameObject SelectObj(Form owner)
        {
            foreach (var go in Data.Scene.GameObjects)
                listBox1.Items.Add(go.Name);

            if (listBox1.Items.Count == 0)
            {
                Close();
                return null;
            }

            Show();
            this.Owner = owner;

            while (gameobj == null)
                Application.DoEvents();

            Close();

            return gameobj;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;

            gameobj = Data.Scene.GetByName(listBox1.SelectedItem.ToString());
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;

            gameobj = Data.Scene.GetByName(listBox1.SelectedItem.ToString());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (var go in Data.Scene.GameObjects.Where(x => x.Name.Contains(textBox1.Text)))
                listBox1.Items.Add(go.Name);
        }
    }
}
