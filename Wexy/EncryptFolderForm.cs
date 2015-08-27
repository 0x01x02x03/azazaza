using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wexy
{
    public partial class EncryptFolderForm : Form
    {
        private string folderpath = "";
        private string webServerUrl = "";
        public EncryptFolderForm()
        {
            InitializeComponent();
        }

        public string getFolderPath()
        {
            return folderpath;
        }
        public string getWebServerUrl()
        {
            return webServerUrl;
        }
        private void btn_confirm_Click(object sender, EventArgs e)
        {
            folderpath = txb_folderpath.Text;
            webServerUrl = txb_webserverurl.Text;
            webServerUrl = "http://" + webServerUrl + "/rcslock/unlock.php?key=";
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
