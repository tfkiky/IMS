using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMS
{
    public partial class ClientMainForm : Office2007Form
    {
        private static ClientMainForm instance;

        public static ClientMainForm Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        public ClientMainForm()
        {
            InitializeComponent();
            StyleManager.Style = eStyle.Office2007Black;
            instance = this;
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            CloseClient();
        }

        public void CloseClient()
        {
            this.Close();
        }

        private void ClientMainForm_Load(object sender, EventArgs e)
        {
            MainForm main = new MainForm();
            main.Show();
        }
    }
}
