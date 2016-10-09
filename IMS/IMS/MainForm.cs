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
            FillDataGrid();
        }

        private void FillDataGrid()
        {
            dataGridViewX1.RowTemplate.Height = dataGridViewX1.Height / 3-2;
            dataGridViewX1.Rows.Add(3);
            dataGridViewX2.RowTemplate.Height = dataGridViewX1.Height / 3-2;
            dataGridViewX2.Rows.Add(3);
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

        private void tsmiSysConfig_Click(object sender, EventArgs e)
        {
            SysConfig sysCfg = new SysConfig();
            sysCfg.ShowDialog();
        }
    }
}
