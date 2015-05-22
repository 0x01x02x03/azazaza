namespace Wexy
{
    partial class RemoteComputerIP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteComputerIP));
            this.btn_hack = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txb_ip = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txb_port = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.SuspendLayout();
            // 
            // btn_hack
            // 
            this.btn_hack.Location = new System.Drawing.Point(80, 60);
            this.btn_hack.Name = "btn_hack";
            this.btn_hack.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Black;
            this.btn_hack.Size = new System.Drawing.Size(90, 25);
            this.btn_hack.TabIndex = 0;
            this.btn_hack.Values.Text = "Hack";
            this.btn_hack.Click += new System.EventHandler(this.btn_hack_Click);
            // 
            // txb_ip
            // 
            this.txb_ip.Location = new System.Drawing.Point(24, 34);
            this.txb_ip.Name = "txb_ip";
            this.txb_ip.Size = new System.Drawing.Size(146, 20);
            this.txb_ip.TabIndex = 1;
            // 
            // txb_port
            // 
            this.txb_port.Location = new System.Drawing.Point(176, 34);
            this.txb_port.Name = "txb_port";
            this.txb_port.Size = new System.Drawing.Size(50, 20);
            this.txb_port.TabIndex = 2;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(41, 8);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(185, 20);
            this.kryptonLabel1.TabIndex = 3;
            this.kryptonLabel1.Values.Text = "Enter the IP Adress and the port";
            // 
            // RemoteComputerIP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 94);
            this.Controls.Add(this.kryptonLabel1);
            this.Controls.Add(this.txb_port);
            this.Controls.Add(this.txb_ip);
            this.Controls.Add(this.btn_hack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RemoteComputerIP";
            this.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Black;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IP Adress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btn_hack;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txb_ip;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txb_port;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
    }
}