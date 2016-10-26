using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.Collecter
{
    public class AccessEventArgs:EventArgs
    {
        private Maticsoft.Model.SMT_STAFF_INFO _staffInfo;

        public Maticsoft.Model.SMT_STAFF_INFO StaffInfo
        {
            get { return _staffInfo; }
        }

        private string _passTime;

        public string PassTime
        {
            get { return _passTime; }
        }

        private bool _isAllow;

        public bool IsAllow
        {
            get { return _isAllow; }
            set { _isAllow = value; }
        }

        public AccessEventArgs(Maticsoft.Model.SMT_STAFF_INFO staffInfo,string passTime,bool isAllow)
        {
            _staffInfo = staffInfo;
            _passTime = passTime;
            _isAllow = isAllow;
        }
    }
}
