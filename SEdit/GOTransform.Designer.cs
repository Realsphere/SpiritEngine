namespace SEdit
{
    partial class GOTransform
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.posX = new System.Windows.Forms.NumericUpDown();
            this.posY = new System.Windows.Forms.NumericUpDown();
            this.posZ = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.scaleZ = new System.Windows.Forms.NumericUpDown();
            this.scaleY = new System.Windows.Forms.NumericUpDown();
            this.scaleX = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.rotZ = new System.Windows.Forms.NumericUpDown();
            this.rotY = new System.Windows.Forms.NumericUpDown();
            this.rotX = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.posX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotX)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 52);
            this.label1.TabIndex = 0;
            this.label1.Text = "Position";
            // 
            // posX
            // 
            this.posX.DecimalPlaces = 4;
            this.posX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.posX.Location = new System.Drawing.Point(12, 91);
            this.posX.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.posX.Minimum = new decimal(new int[] {
            2147483647,
            0,
            0,
            -2147483648});
            this.posX.Name = "posX";
            this.posX.Size = new System.Drawing.Size(270, 43);
            this.posX.TabIndex = 1;
            // 
            // posY
            // 
            this.posY.DecimalPlaces = 4;
            this.posY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.posY.Location = new System.Drawing.Point(288, 91);
            this.posY.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.posY.Minimum = new decimal(new int[] {
            2147483647,
            0,
            0,
            -2147483648});
            this.posY.Name = "posY";
            this.posY.Size = new System.Drawing.Size(270, 43);
            this.posY.TabIndex = 2;
            // 
            // posZ
            // 
            this.posZ.DecimalPlaces = 4;
            this.posZ.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.posZ.Location = new System.Drawing.Point(564, 91);
            this.posZ.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.posZ.Minimum = new decimal(new int[] {
            2147483647,
            0,
            0,
            -2147483648});
            this.posZ.Name = "posZ";
            this.posZ.Size = new System.Drawing.Size(270, 43);
            this.posZ.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(564, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 37);
            this.label2.TabIndex = 4;
            this.label2.Text = "Z";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(288, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 37);
            this.label3.TabIndex = 5;
            this.label3.Text = "Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 37);
            this.label4.TabIndex = 6;
            this.label4.Text = "X";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 37);
            this.label5.TabIndex = 13;
            this.label5.Text = "X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(288, 194);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 37);
            this.label6.TabIndex = 12;
            this.label6.Text = "Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(564, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 37);
            this.label7.TabIndex = 11;
            this.label7.Text = "Z";
            // 
            // scaleZ
            // 
            this.scaleZ.DecimalPlaces = 4;
            this.scaleZ.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.scaleZ.Location = new System.Drawing.Point(564, 234);
            this.scaleZ.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.scaleZ.Minimum = new decimal(new int[] {
            2147483647,
            0,
            0,
            -2147483648});
            this.scaleZ.Name = "scaleZ";
            this.scaleZ.Size = new System.Drawing.Size(270, 43);
            this.scaleZ.TabIndex = 10;
            // 
            // scaleY
            // 
            this.scaleY.DecimalPlaces = 4;
            this.scaleY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.scaleY.Location = new System.Drawing.Point(288, 234);
            this.scaleY.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.scaleY.Minimum = new decimal(new int[] {
            2147483647,
            0,
            0,
            -2147483648});
            this.scaleY.Name = "scaleY";
            this.scaleY.Size = new System.Drawing.Size(270, 43);
            this.scaleY.TabIndex = 9;
            // 
            // scaleX
            // 
            this.scaleX.DecimalPlaces = 4;
            this.scaleX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.scaleX.Location = new System.Drawing.Point(12, 234);
            this.scaleX.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.scaleX.Minimum = new decimal(new int[] {
            2147483647,
            0,
            0,
            -2147483648});
            this.scaleX.Name = "scaleX";
            this.scaleX.Size = new System.Drawing.Size(270, 43);
            this.scaleX.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(12, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 52);
            this.label8.TabIndex = 7;
            this.label8.Text = "Scale";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 356);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 37);
            this.label9.TabIndex = 20;
            this.label9.Text = "X";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(288, 356);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 37);
            this.label10.TabIndex = 19;
            this.label10.Text = "Y";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(564, 356);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 37);
            this.label11.TabIndex = 18;
            this.label11.Text = "Z";
            // 
            // rotZ
            // 
            this.rotZ.DecimalPlaces = 4;
            this.rotZ.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.rotZ.Location = new System.Drawing.Point(566, 396);
            this.rotZ.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.rotZ.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.rotZ.Name = "rotZ";
            this.rotZ.Size = new System.Drawing.Size(270, 43);
            this.rotZ.TabIndex = 17;
            // 
            // rotY
            // 
            this.rotY.DecimalPlaces = 4;
            this.rotY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.rotY.Location = new System.Drawing.Point(288, 396);
            this.rotY.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.rotY.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.rotY.Name = "rotY";
            this.rotY.Size = new System.Drawing.Size(270, 43);
            this.rotY.TabIndex = 16;
            // 
            // rotX
            // 
            this.rotX.DecimalPlaces = 4;
            this.rotX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.rotX.Location = new System.Drawing.Point(12, 396);
            this.rotX.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.rotX.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.rotX.Name = "rotX";
            this.rotX.Size = new System.Drawing.Size(270, 43);
            this.rotX.TabIndex = 15;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label12.Location = new System.Drawing.Point(12, 314);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(167, 52);
            this.label12.TabIndex = 14;
            this.label12.Text = "Rotation";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(665, 456);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(169, 52);
            this.button1.TabIndex = 23;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 5;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // GOTransform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 526);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.rotZ);
            this.Controls.Add(this.rotY);
            this.Controls.Add(this.rotX);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.scaleZ);
            this.Controls.Add(this.scaleY);
            this.Controls.Add(this.scaleX);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.posZ);
            this.Controls.Add(this.posY);
            this.Controls.Add(this.posX);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "GOTransform";
            this.Load += new System.EventHandler(this.GOTransform_Load);
            ((System.ComponentModel.ISupportInitialize)(this.posX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown posX;
        private System.Windows.Forms.NumericUpDown posY;
        private System.Windows.Forms.NumericUpDown posZ;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown scaleZ;
        private System.Windows.Forms.NumericUpDown scaleY;
        private System.Windows.Forms.NumericUpDown scaleX;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown rotZ;
        private System.Windows.Forms.NumericUpDown rotY;
        private System.Windows.Forms.NumericUpDown rotX;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
    }
}