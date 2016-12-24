using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace IMS.Main
{
    public partial class SplashForm : Form
    {
        private static SplashForm instance;

        public static SplashForm Instance
        {
            get { return SplashForm.instance; }
            set { SplashForm.instance = value; }
        }

        public SplashForm()
        {
            InitializeComponent();
        }

        public void SetText(string message)
        {
            BackgroundWorker worker = new BackgroundWorker();



            worker.DoWork += (p, q) =>
            {
                this.Invoke(new Action(() =>
                {
                    this.lbLoading.Text = message;
                    this.lbLoading.Refresh();
                }));
            };
            worker.RunWorkerAsync();
            Thread.Sleep(50);
        }

        private void tsmiSysConfig_Click(object sender, EventArgs e)
        {
            SysConfig sysCfg = new SysConfig();
            sysCfg.ShowDialog();
        }
    }
}
