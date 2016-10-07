using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            int iret=IDCardDll.IDCard.InitCommExt();

            iret = IDCardDll.IDCard.Authenticate();

            try
            {
                iret = IDCardDll.IDCard.Read_Content(1);

            }
            catch { }
            FileInfo fi =new FileInfo("./wz.txt");
            if (fi.Exists)
            {
                FileStream fsRead = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                int fsLen = (int)fsRead.Length;
                byte[] heByte = new byte[fsLen];
                int r = fsRead.Read(heByte, 0, heByte.Length);
                string myStr = System.Text.Encoding.Unicode.GetString(heByte);
            }
            iret = IDCardDll.IDCard.CloseComm();
        }
    }
}
