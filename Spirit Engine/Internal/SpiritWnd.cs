using Realsphere.Spirit.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Realsphere.Spirit.Internal
{
    internal class SpiritWnd : Form
    {
        public SpiritWnd()
        {
            InitializeComponent();
        }

        private void SpiritWnd_Load(object sender, EventArgs e)
        {
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SpiritWnd
            // 
            this.ClientSize = new System.Drawing.Size(272, 221);
            this.Name = "SpiritWnd";
            this.Load += new System.EventHandler(this.SpiritWnd_Load_1);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpiritWnd_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SpiritWnd_KeyUp);
            this.ResumeLayout(false);

        }

        private void SpiritWnd_Load_1(object sender, EventArgs e)
        {

        }

        private void SpiritWnd_KeyDown(object sender, KeyEventArgs e)
        {
            if (Game.KeyDown != null) Game.KeyDown.Invoke(null, (int)e.KeyData);
        }

        private void SpiritWnd_KeyUp(object sender, KeyEventArgs e)
        {
            if (Game.KeyUp != null) Game.KeyUp.Invoke(null, (int)e.KeyData);
        }
    }
}
