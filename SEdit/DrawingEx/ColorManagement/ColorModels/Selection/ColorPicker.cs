using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.Serialization;

using DrawingEx.ColorManagement.ColorModels;
using DrawingEx.ColorManagement.ColorModels.Selection;
using Realsphere.Spirit;

namespace DrawingEx.ColorManagement
{
	/// <summary>
	/// Zusammenfassung für Form1.
	/// </summary>
	public sealed class ColorPicker : System.Windows.Forms.Form
	{
		private DrawingEx.ColorManagement.ColorModels.Selection.ColorSelectionPlane colorSelectionPlane1;
		private DrawingEx.ColorManagement.ColorModels.Selection.ColorSelectionFader colorSelectionFader1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.RadioButton rdHSV_H;
		private System.Windows.Forms.RadioButton rdHSV_S;
		private System.Windows.Forms.RadioButton rdHSV_V;
		private System.Windows.Forms.RadioButton rdSecond_1;
		private System.Windows.Forms.RadioButton rdSecond_2;
		private System.Windows.Forms.RadioButton rdSecond_3;
		private System.Windows.Forms.TextBox tbHSV_H;
		private System.Windows.Forms.TextBox tbHSV_S;
		private System.Windows.Forms.TextBox tbHSV_V;
		private System.Windows.Forms.TextBox tbSecond_1;
		private System.Windows.Forms.TextBox tbSecond_2;
		private System.Windows.Forms.TextBox tbSecond_3;
		private System.Windows.Forms.Label lblHSV_H;
		private System.Windows.Forms.Label lblHSV_S;
		private System.Windows.Forms.Label lblHSV_V;
		private System.Windows.Forms.Label lblSecond_1;
		private System.Windows.Forms.Label lblSecond_2;
		private System.Windows.Forms.Label lblSecond_3;
		public DrawingEx.ColorManagement.ColorLabel lblColorOut;
		private ToolTip toolTip;
		private IContainer components;

		public ColorPicker():this(Mode.HSV_RGB,Fader.HSV_H){}
		public ColorPicker(Mode mode, Fader fader)
		{
			_mode=mode;
			_fader=fader;

			InitializeComponent();

			UpdateUI();
			filter=new ShiftKeyFilter();
			filter.ShiftStateChanged+=new EventHandler(filter_ShiftStateChanged);
			Application.AddMessageFilter(filter);
        }
		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			if(filter!=null)
			{
				Application.RemoveMessageFilter(filter);
				filter=null;
			}
			base.Dispose( disposing );
		}
		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPicker));
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.rdHSV_H = new System.Windows.Forms.RadioButton();
            this.rdHSV_S = new System.Windows.Forms.RadioButton();
            this.rdHSV_V = new System.Windows.Forms.RadioButton();
            this.rdSecond_1 = new System.Windows.Forms.RadioButton();
            this.rdSecond_2 = new System.Windows.Forms.RadioButton();
            this.rdSecond_3 = new System.Windows.Forms.RadioButton();
            this.tbHSV_H = new System.Windows.Forms.TextBox();
            this.tbHSV_S = new System.Windows.Forms.TextBox();
            this.tbHSV_V = new System.Windows.Forms.TextBox();
            this.tbSecond_1 = new System.Windows.Forms.TextBox();
            this.tbSecond_2 = new System.Windows.Forms.TextBox();
            this.tbSecond_3 = new System.Windows.Forms.TextBox();
            this.lblHSV_H = new System.Windows.Forms.Label();
            this.lblHSV_S = new System.Windows.Forms.Label();
            this.lblHSV_V = new System.Windows.Forms.Label();
            this.lblSecond_1 = new System.Windows.Forms.Label();
            this.lblSecond_2 = new System.Windows.Forms.Label();
            this.lblSecond_3 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblColorOut = new DrawingEx.ColorManagement.ColorLabel();
            this.colorSelectionFader1 = new DrawingEx.ColorManagement.ColorModels.Selection.ColorSelectionFader();
            this.colorSelectionPlane1 = new DrawingEx.ColorManagement.ColorModels.Selection.ColorSelectionPlane();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(21, 475);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(766, 3);
            this.label1.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(559, 497);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(208, 67);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(330, 497);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(208, 67);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // rdHSV_H
            // 
            this.rdHSV_H.Checked = true;
            this.rdHSV_H.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdHSV_H.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdHSV_H.Location = new System.Drawing.Point(520, 22);
            this.rdHSV_H.Name = "rdHSV_H";
            this.rdHSV_H.Size = new System.Drawing.Size(83, 56);
            this.rdHSV_H.TabIndex = 6;
            this.rdHSV_H.TabStop = true;
            this.rdHSV_H.Text = "H";
            this.rdHSV_H.CheckedChanged += new System.EventHandler(this.UpdaterdFaderedChanged);
            // 
            // rdHSV_S
            // 
            this.rdHSV_S.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdHSV_S.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdHSV_S.Location = new System.Drawing.Point(520, 89);
            this.rdHSV_S.Name = "rdHSV_S";
            this.rdHSV_S.Size = new System.Drawing.Size(83, 55);
            this.rdHSV_S.TabIndex = 7;
            this.rdHSV_S.Text = "S";
            this.rdHSV_S.CheckedChanged += new System.EventHandler(this.UpdaterdFaderedChanged);
            // 
            // rdHSV_V
            // 
            this.rdHSV_V.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdHSV_V.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdHSV_V.Location = new System.Drawing.Point(520, 155);
            this.rdHSV_V.Name = "rdHSV_V";
            this.rdHSV_V.Size = new System.Drawing.Size(83, 55);
            this.rdHSV_V.TabIndex = 8;
            this.rdHSV_V.Text = "V";
            this.rdHSV_V.CheckedChanged += new System.EventHandler(this.UpdaterdFaderedChanged);
            // 
            // rdSecond_1
            // 
            this.rdSecond_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdSecond_1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdSecond_1.Location = new System.Drawing.Point(520, 244);
            this.rdSecond_1.Name = "rdSecond_1";
            this.rdSecond_1.Size = new System.Drawing.Size(83, 55);
            this.rdSecond_1.TabIndex = 9;
            this.rdSecond_1.Text = "R";
            this.rdSecond_1.CheckedChanged += new System.EventHandler(this.UpdaterdFaderedChanged);
            // 
            // rdSecond_2
            // 
            this.rdSecond_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdSecond_2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdSecond_2.Location = new System.Drawing.Point(520, 310);
            this.rdSecond_2.Name = "rdSecond_2";
            this.rdSecond_2.Size = new System.Drawing.Size(83, 56);
            this.rdSecond_2.TabIndex = 10;
            this.rdSecond_2.Text = "G";
            this.rdSecond_2.CheckedChanged += new System.EventHandler(this.UpdaterdFaderedChanged);
            // 
            // rdSecond_3
            // 
            this.rdSecond_3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdSecond_3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdSecond_3.Location = new System.Drawing.Point(520, 377);
            this.rdSecond_3.Name = "rdSecond_3";
            this.rdSecond_3.Size = new System.Drawing.Size(83, 55);
            this.rdSecond_3.TabIndex = 11;
            this.rdSecond_3.Text = "B";
            this.rdSecond_3.CheckedChanged += new System.EventHandler(this.UpdaterdFaderedChanged);
            // 
            // tbHSV_H
            // 
            this.tbHSV_H.Location = new System.Drawing.Point(603, 22);
            this.tbHSV_H.MaxLength = 6;
            this.tbHSV_H.Name = "tbHSV_H";
            this.tbHSV_H.Size = new System.Drawing.Size(125, 43);
            this.tbHSV_H.TabIndex = 0;
            this.tbHSV_H.Text = "0";
            this.tbHSV_H.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbValue_KeyUp);
            this.tbHSV_H.Leave += new System.EventHandler(this.tbValue_Leave);
            // 
            // tbHSV_S
            // 
            this.tbHSV_S.Location = new System.Drawing.Point(603, 89);
            this.tbHSV_S.MaxLength = 6;
            this.tbHSV_S.Name = "tbHSV_S";
            this.tbHSV_S.Size = new System.Drawing.Size(125, 43);
            this.tbHSV_S.TabIndex = 1;
            this.tbHSV_S.Text = "0";
            this.tbHSV_S.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbValue_KeyUp);
            this.tbHSV_S.Leave += new System.EventHandler(this.tbValue_Leave);
            // 
            // tbHSV_V
            // 
            this.tbHSV_V.Location = new System.Drawing.Point(603, 155);
            this.tbHSV_V.MaxLength = 6;
            this.tbHSV_V.Name = "tbHSV_V";
            this.tbHSV_V.Size = new System.Drawing.Size(125, 43);
            this.tbHSV_V.TabIndex = 2;
            this.tbHSV_V.Text = "0";
            this.tbHSV_V.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbValue_KeyUp);
            this.tbHSV_V.Leave += new System.EventHandler(this.tbValue_Leave);
            // 
            // tbSecond_1
            // 
            this.tbSecond_1.Location = new System.Drawing.Point(603, 244);
            this.tbSecond_1.MaxLength = 6;
            this.tbSecond_1.Name = "tbSecond_1";
            this.tbSecond_1.Size = new System.Drawing.Size(125, 43);
            this.tbSecond_1.TabIndex = 3;
            this.tbSecond_1.Text = "0";
            this.tbSecond_1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbValue_KeyUp);
            this.tbSecond_1.Leave += new System.EventHandler(this.tbValue_Leave);
            // 
            // tbSecond_2
            // 
            this.tbSecond_2.Location = new System.Drawing.Point(603, 310);
            this.tbSecond_2.MaxLength = 6;
            this.tbSecond_2.Name = "tbSecond_2";
            this.tbSecond_2.Size = new System.Drawing.Size(125, 43);
            this.tbSecond_2.TabIndex = 4;
            this.tbSecond_2.Text = "0";
            this.tbSecond_2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbValue_KeyUp);
            this.tbSecond_2.Leave += new System.EventHandler(this.tbValue_Leave);
            // 
            // tbSecond_3
            // 
            this.tbSecond_3.Location = new System.Drawing.Point(603, 377);
            this.tbSecond_3.MaxLength = 6;
            this.tbSecond_3.Name = "tbSecond_3";
            this.tbSecond_3.Size = new System.Drawing.Size(125, 43);
            this.tbSecond_3.TabIndex = 5;
            this.tbSecond_3.Text = "0";
            this.tbSecond_3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbValue_KeyUp);
            this.tbSecond_3.Leave += new System.EventHandler(this.tbValue_Leave);
            // 
            // lblHSV_H
            // 
            this.lblHSV_H.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblHSV_H.Location = new System.Drawing.Point(728, 22);
            this.lblHSV_H.Name = "lblHSV_H";
            this.lblHSV_H.Size = new System.Drawing.Size(42, 56);
            this.lblHSV_H.TabIndex = 7;
            this.lblHSV_H.Text = "°";
            this.lblHSV_H.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHSV_S
            // 
            this.lblHSV_S.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblHSV_S.Location = new System.Drawing.Point(728, 89);
            this.lblHSV_S.Name = "lblHSV_S";
            this.lblHSV_S.Size = new System.Drawing.Size(42, 55);
            this.lblHSV_S.TabIndex = 7;
            this.lblHSV_S.Text = "%";
            this.lblHSV_S.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHSV_V
            // 
            this.lblHSV_V.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblHSV_V.Location = new System.Drawing.Point(728, 155);
            this.lblHSV_V.Name = "lblHSV_V";
            this.lblHSV_V.Size = new System.Drawing.Size(42, 55);
            this.lblHSV_V.TabIndex = 7;
            this.lblHSV_V.Text = "%";
            this.lblHSV_V.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSecond_1
            // 
            this.lblSecond_1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSecond_1.Location = new System.Drawing.Point(728, 244);
            this.lblSecond_1.Name = "lblSecond_1";
            this.lblSecond_1.Size = new System.Drawing.Size(42, 55);
            this.lblSecond_1.TabIndex = 7;
            this.lblSecond_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSecond_2
            // 
            this.lblSecond_2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSecond_2.Location = new System.Drawing.Point(728, 310);
            this.lblSecond_2.Name = "lblSecond_2";
            this.lblSecond_2.Size = new System.Drawing.Size(42, 56);
            this.lblSecond_2.TabIndex = 7;
            this.lblSecond_2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSecond_3
            // 
            this.lblSecond_3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSecond_3.Location = new System.Drawing.Point(728, 377);
            this.lblSecond_3.Name = "lblSecond_3";
            this.lblSecond_3.Size = new System.Drawing.Size(42, 55);
            this.lblSecond_3.TabIndex = 7;
            this.lblSecond_3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 1000;
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 1000;
            this.toolTip.ReshowDelay = 200;
            // 
            // lblColorOut
            // 
            this.lblColorOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblColorOut.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblColorOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblColorOut.Location = new System.Drawing.Point(21, 497);
            this.lblColorOut.Name = "lblColorOut";
            this.lblColorOut.OldColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblColorOut.Size = new System.Drawing.Size(187, 67);
            this.lblColorOut.TabIndex = 0;
            this.toolTip.SetToolTip(this.lblColorOut, "Right Click here for more options.\r\nLeft Click and drag outside to pick a color f" +
        "rom screen.");
            this.lblColorOut.ColorChanged += new System.EventHandler(this.lblColorOut_ColorChanged);
            this.lblColorOut.Click += new System.EventHandler(this.lblColorOut_Click);
            // 
            // colorSelectionFader1
            // 
            this.colorSelectionFader1.Location = new System.Drawing.Point(416, 8);
            this.colorSelectionFader1.Name = "colorSelectionFader1";
            this.colorSelectionFader1.Size = new System.Drawing.Size(62, 438);
            this.colorSelectionFader1.TabIndex = 1;
            this.colorSelectionFader1.TabStop = false;
            this.toolTip.SetToolTip(this.colorSelectionFader1, "Hold down Shift for snap at 10% steps");
            // 
            // colorSelectionPlane1
            // 
            this.colorSelectionPlane1.Location = new System.Drawing.Point(21, 22);
            this.colorSelectionPlane1.Name = "colorSelectionPlane1";
            this.colorSelectionPlane1.Size = new System.Drawing.Size(385, 410);
            this.colorSelectionPlane1.TabIndex = 0;
            this.colorSelectionPlane1.TabStop = false;
            this.toolTip.SetToolTip(this.colorSelectionPlane1, "Hold Down Shift for snap to grid");
            this.colorSelectionPlane1.Click += new System.EventHandler(this.colorSelectionPlane1_Click);
            this.colorSelectionPlane1.DoubleClick += new System.EventHandler(this.colorSelectionPlane1_DoubleClick);
            // 
            // ColorPicker
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(13, 36);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(793, 586);
            this.Controls.Add(this.lblColorOut);
            this.Controls.Add(this.lblHSV_H);
            this.Controls.Add(this.tbSecond_3);
            this.Controls.Add(this.tbSecond_2);
            this.Controls.Add(this.tbSecond_1);
            this.Controls.Add(this.tbHSV_V);
            this.Controls.Add(this.tbHSV_S);
            this.Controls.Add(this.tbHSV_H);
            this.Controls.Add(this.rdHSV_H);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.colorSelectionFader1);
            this.Controls.Add(this.colorSelectionPlane1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.rdHSV_S);
            this.Controls.Add(this.rdHSV_V);
            this.Controls.Add(this.rdSecond_1);
            this.Controls.Add(this.rdSecond_2);
            this.Controls.Add(this.rdSecond_3);
            this.Controls.Add(this.lblHSV_S);
            this.Controls.Add(this.lblHSV_V);
            this.Controls.Add(this.lblSecond_1);
            this.Controls.Add(this.lblSecond_2);
            this.Controls.Add(this.lblSecond_3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorPicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Color Picker";
            this.Load += new System.EventHandler(this.ColorPicker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
		#endregion

		#region types
		private class ShiftKeyFilter:IMessageFilter
		{
			private const int WM_KEYDOWN = 0x100;
			private const int WM_KEYUP = 0x101;

			public bool PreFilterMessage(ref Message m)
			{
				switch(m.Msg)
				{
					case WM_KEYDOWN:
						if(m.WParam.ToInt32()==(int)Keys.ShiftKey)
						{
							RaiseShiftStateChanged();
							return true;
						}break;
					case WM_KEYUP:
						if(m.WParam.ToInt32()==(int)Keys.ShiftKey)
						{
							RaiseShiftStateChanged();
							return true;
						}break;
				}
				return false;
			}
			private void RaiseShiftStateChanged()
			{
				if(ShiftStateChanged!=null)
							ShiftStateChanged(this,EventArgs.Empty);
			}
			public event EventHandler ShiftStateChanged;
		}

		public enum Fader
		{
			HSV_H=0,
			HSV_S=1,
			HSV_V=2,

			Second_1=3,
			Second_2=4,
			Second_3=5
		}
		public enum Mode
		{
			HSV_RGB=0,
			HSV_LAB=1
		}
		#endregion
		#region variables
		private ShiftKeyFilter filter;
		private ColorSelectionModule _module;
		private XYZ _color=XYZ.FromRGB(new(0d, 0d, 0d));
		private Mode _mode=Mode.HSV_RGB;
		private Fader _fader=Fader.HSV_H;
		#endregion
		#region ui updating
		public void UpdateUI()
		{
			ChangeModule();
			ChangeDescriptions();
			UpdaterdFader();
			UpdatectxOptions();
			UpdatetbValue(null);

            _color = XYZ.FromRGB(new(0d, 0d, 0d));
            _module.XYZ=_color;
			lblColorOut.Color=
				lblColorOut.OldColor=_color.ToRGB();
            Game.ActiveScene.SkyBoxColor = new(lblColorOut.Color.R, lblColorOut.Color.G, lblColorOut.Color.B, lblColorOut.Color.A);
        }
		#region module
		private void ChangeModule(ColorSelectionModule value)
		{
			if(value==_module) return;
			if(_module!=null)
			{
				_module.ColorChanged-=new EventHandler(_module_ColorChanged);
				_module.ColorSelectionFader=null;
				_module.ColorSelectionPlane=null;
			}
			_module=value;
			if(_module!=null)
			{
				_module.ColorChanged+=new EventHandler(_module_ColorChanged);
				_module.XYZ=_color;
				_module.ColorSelectionFader=colorSelectionFader1;
				_module.ColorSelectionPlane=colorSelectionPlane1;
            }
            Game.ActiveScene.SkyBoxColor = new(lblColorOut.Color.R, lblColorOut.Color.G, lblColorOut.Color.B, lblColorOut.Color.A);
        }
		private void ChangeModule()
		{
			switch(_fader)
			{
				case Fader.HSV_H: ChangeModule(new ColorSelectionModuleHSV_H()); break;
				case Fader.HSV_S: ChangeModule(new ColorSelectionModuleHSV_S()); break;
				case Fader.HSV_V: ChangeModule(new ColorSelectionModuleHSV_V()); break;
				case Fader.Second_1:
					if(_mode==Mode.HSV_RGB)
						ChangeModule(new ColorSelectionModuleRGB_R());
					else
						ChangeModule(new ColorSelectionModuleLAB_L());
					break;
				case Fader.Second_2:
					if(_mode==Mode.HSV_RGB)
						ChangeModule(new ColorSelectionModuleRGB_G());
					else
						ChangeModule(new ColorSelectionModuleLAB_a());
					break;
				default:
					if(_mode==Mode.HSV_RGB)
						ChangeModule(new ColorSelectionModuleRGB_B());
					else
						ChangeModule(new ColorSelectionModuleLAB_b()); break;
            }
            Game.ActiveScene.SkyBoxColor = new(lblColorOut.Color.R, lblColorOut.Color.G, lblColorOut.Color.B, lblColorOut.Color.A);
        }
		private void ChangeDescriptions()
		{
			switch(_mode)
			{
				case Mode.HSV_RGB:
					rdSecond_1.Text="R";
					rdSecond_2.Text="G";
					rdSecond_3.Text="B";
					break;
				default:
					rdSecond_1.Text="L";
					rdSecond_2.Text="a*";
					rdSecond_3.Text="b*";
					break;
            }
            Game.ActiveScene.SkyBoxColor = new(lblColorOut.Color.R, lblColorOut.Color.G, lblColorOut.Color.B, lblColorOut.Color.A);
        }
		#endregion
		#region contextmenu
		private void ctxOptions_Click(object sender, System.EventArgs e)
		{
			UpdatectxOptions();
			ChangeDescriptions();
			ChangeModule();
			UpdatetbValue(null);
            Game.ActiveScene.SkyBoxColor = new(lblColorOut.Color.R, lblColorOut.Color.G, lblColorOut.Color.B, lblColorOut.Color.A);
        }
		private void UpdatectxOptions()
		{
		}
		#endregion
		#region rdFader
		private void UpdaterdFaderedChanged(object sender, System.EventArgs e)
		{
			if(sender==rdHSV_H)
				_fader=Fader.HSV_H;
			else if(sender==rdHSV_S)
				_fader=Fader.HSV_S;
			else if(sender==rdHSV_V)
				_fader=Fader.HSV_V;
				//secondary faders
			else if(sender==rdSecond_1)
				_fader=Fader.Second_1;
			else if(sender==rdSecond_2)
				_fader=Fader.Second_2;
			else//(sender==rdSecond_3)
				_fader=Fader.Second_3;

            ChangeModule();
		}
		private void UpdaterdFader()
		{
			if(_fader==Fader.HSV_H)
				rdHSV_H.Checked=true;
			else if(_fader==Fader.HSV_S)
				rdHSV_S.Checked=true;
			else if(_fader==Fader.HSV_V)
				rdHSV_V.Checked=true;
			else if(_fader==Fader.Second_1)
				rdSecond_1.Checked=true;
			else if(_fader==Fader.Second_2)
				rdSecond_2.Checked=true;
			else if(_fader==Fader.Second_3)
				rdSecond_3.Checked=true;
		}
		#endregion
		#region tbValue
		private void tbValue_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(!(sender is TextBox)) return;
			if(e.KeyCode==Keys.Return)
			{
				UpdatetbValue(null);
				e.Handled=true;
				return;
			}
			double value;
			if(!double.TryParse(((TextBox)sender).Text,
				System.Globalization.NumberStyles.Integer,
				null,out value)) return;
			#region hsv  textboxes
			if(sender==tbHSV_H)
			{
				HSV chsv=HSV.FromRGB(_color.ToRGB());
				chsv.H=value/360.0;
				_color=XYZ.FromRGB(chsv.ToRGB());
			}
			else if(sender==tbHSV_S)
			{
				HSV chsv=HSV.FromRGB(_color.ToRGB());
				chsv.S=value/100.0;
				_color=XYZ.FromRGB(chsv.ToRGB());
			}
			else if(sender==tbHSV_V)
			{
				HSV chsv=HSV.FromRGB(_color.ToRGB());
				chsv.V=value/100.0;
				_color=XYZ.FromRGB(chsv.ToRGB());
			}
			#endregion
			#region secondary textboxes
			else if(_mode==Mode.HSV_RGB)
			{
				RGB crgb=_color.ToRGB();
				if(sender==tbSecond_1)
				{
					crgb.R=value/255.0;
				}
				else if(sender==tbSecond_2)
				{
					crgb.G=value/255.0;
				}
				else //sender==tbSecond_3
				{
					crgb.B=value/255.0;
				}
				_color=XYZ.FromRGB(crgb);
			}
			else if(_mode==Mode.HSV_LAB)
			{
				LAB clab=LAB.FromXYZ(_color);
				if(sender==tbSecond_1)
				{
					clab.L=value;
				}
				else if(sender==tbSecond_2)
				{
					clab.a=value;
				}
				else //sender==tbSecond_3
				{
					clab.b=value;
				}
				_color=clab.ToXYZ();
			}
			#endregion
			//update ui
			_module.XYZ=_color;
			lblColorOut.Color=_color.ToRGB();
			UpdatetbValue((TextBox)sender);
		}		
		private void tbValue_Leave(object sender, System.EventArgs e)
		{
			UpdatetbValue(null);
		}
		int UpdatetbValuecalls = -1;

        private void UpdatetbValue(TextBox skipupdate)
		{
			UpdatetbValuecalls++;
            #region hsv textboxes
            HSV chsv=HSV.FromRGB(_color.ToRGB());
			if(skipupdate!=tbHSV_H)
				tbHSV_H.Text=(chsv.H*360.0).ToString("0");
			if(skipupdate!=tbHSV_S)
				tbHSV_S.Text=(chsv.S*100.0).ToString("0");
			if(skipupdate!=tbHSV_V)
				tbHSV_V.Text=(chsv.V*100.0).ToString("0");
			#endregion
			#region secondary textboxes
			if(_mode==Mode.HSV_RGB)
			{
				RGB crgb=_color.ToRGB();
                if (UpdatetbValuecalls < 2)
					crgb = new(0d, 0d, 0d);
                if (skipupdate!=tbSecond_1)
					tbSecond_1.Text=(crgb.R*255.0).ToString("0");
				if(skipupdate!=tbSecond_2)
					tbSecond_2.Text=(crgb.G*255.0).ToString("0");
				if(skipupdate!=tbSecond_3)
					tbSecond_3.Text=(crgb.B*255.0).ToString("0");
			}
			else//(_mode==Mode.HSV_LAB)
			{
				LAB clab=LAB.FromXYZ(_color);
                if (UpdatetbValuecalls < 2)
                    clab = new(0d, 0d, 0d);
                if (skipupdate!=tbSecond_1)
					tbSecond_1.Text=clab.L.ToString("0");
				if(skipupdate!=tbSecond_2)
					tbSecond_2.Text=clab.a.ToString("0");
				if(skipupdate!=tbSecond_3)
					tbSecond_3.Text=clab.b.ToString("0");
			}
			#endregion
		}
		#endregion
		#region module & lbl
		private void _module_ColorChanged(object sender, EventArgs e)
		{
			if(_module==null) return;
			_color=_module.XYZ;
			lblColorOut.Color=_color.ToRGB();
			UpdatetbValue(null);
            Game.ActiveScene.SkyBoxColor = new(lblColorOut.Color.R, lblColorOut.Color.G, lblColorOut.Color.B, lblColorOut.Color.A);
        }

		private void lblColorOut_ColorChanged(object sender, System.EventArgs e)
		{
			_color=XYZ.FromRGB(lblColorOut.Color);
			_module.XYZ=_color;
			UpdatetbValue(null);
			Game.ActiveScene.SkyBoxColor = new(lblColorOut.Color.R, lblColorOut.Color.G, lblColorOut.Color.B, lblColorOut.Color.A);
        }
		#endregion
		#endregion
		#region properties
		/// <summary>
		/// gets or sets the color as device-independent CIE-XYZ color
		/// </summary>
		[Description("gets or sets the color as device-independent CIE-XYZ color")]
		public XYZ Color
		{
			get{return _color;}
			set
			{
				if(value==_color) return;
				_color=_module.XYZ=value;
				lblColorOut.Color=
					lblColorOut.OldColor=value.ToRGB();
				UpdatetbValue(null);
                Game.ActiveScene.SkyBoxColor = new(lblColorOut.Color.R, lblColorOut.Color.G, lblColorOut.Color.B, lblColorOut.Color.A);
            }
		}
		[Browsable(false)]
		public Fader PrimaryFader
		{
			get{return _fader;}
//			set
//			{
//				if(value==_fader) return;
//				_fader=value;
//				UpdaterdFader();
//				ChangeModule();
//			}
		}
		[Browsable(false)]
		public Mode SecondaryMode
		{
			get{return _mode;}
//			set
//			{
//				if(value==_mode) return;
//				_mode=value;
//				UpdatectxOptions();
//				ChangeModule();
//				UpdatetbValue(null);
//			}
		}
		#endregion
		private void filter_ShiftStateChanged(object sender, EventArgs e)
		{
			colorSelectionPlane1.Refresh();
			colorSelectionFader1.Refresh();
			//Game.ActiveScene.SkyBoxColor = new(lblColorOut.Color.R, lblColorOut.Color.G, lblColorOut.Color.B, lblColorOut.Color.A);
		}

		private void ColorPicker_Load(object sender, EventArgs e)
        {
			Icon = SEdit.Properties.Resources.Spirit_Logo1;
        }

        private void lblColorOut_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void colorSelectionPlane1_Click(object sender, EventArgs e)
        {

        }

        private void colorSelectionPlane1_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}
