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
    public partial class DownloadFileForm : KryptonForm
    {
        private string url;
        private string filename;

        public DownloadFileForm()
        {
            InitializeComponent();
        }

        public string GetUrl()
        {
            return url;
        }
        public string GetFilename()
        {
            return filename;
        }

        private void btn_remote_download_Click(object sender, EventArgs e)
        {
            url = txb_url.Text;
            filename = txb_filename.Text;
            this.Close();
        }
        
    }
}
