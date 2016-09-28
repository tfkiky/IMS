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
    public partial class MainForm : Form
    {
        private static MainForm instance;

        public static MainForm Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        public MainForm()
        {
            InitializeComponent();
            instance = this;
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            CloseIMS();
        }

        private void CloseIMS()
        {
            if(ClientMainForm.Instance!=null)
            {
                ClientMainForm.Instance.CloseClient();
            }
            this.Close();
        }
    }
}
