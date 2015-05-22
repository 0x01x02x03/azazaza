using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace Wexy
{
    public partial class RemoteComputerIP : KryptonForm
    {
        public RemoteComputerIP()
        {
            InitializeComponent();
        }
        public string GetIp()
        {
            string ip = txb_ip.Text;
            return ip;
        }
        public string GetPort()
        {
            string port = txb_port.Text;
            return port;
        }
        private void btn_hack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
