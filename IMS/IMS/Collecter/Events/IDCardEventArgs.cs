using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.Collecter
{
    public class IDCardEventArgs : EventArgs
    {
        private IDCardClass _idCard;

        public IDCardClass IDCard
        {
            get { return _idCard; }
        }

        private Maticsoft.Model.SMT_STAFF_INFO _staffInfo;

        public Maticsoft.Model.SMT_STAFF_INFO StaffInfo
        {
            get { return _staffInfo; }
        }

        private bool _isAllow;

        public bool IsAllow
        {
            get { return _isAllow; }
            set { _isAllow = value; }
        }

        public IDCardEventArgs(IDCardClass idCard,Maticsoft.Model.SMT_STAFF_INFO staffInfo,bool isAllow)
        {
            _idCard = idCard;
            _staffInfo = staffInfo;
            _isAllow = isAllow;
        }
    }
}
