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
            this.txb_ip.Location = new System.Drawing.Point(29, 34);
            this.txb_ip.Name = "txb_ip";
            this.txb_ip.Size = new System.Drawing.Size(197, 20);
            this.txb_ip.TabIndex = 1;
            // 
            // RemoteComputerIP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 94);
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
    }
}