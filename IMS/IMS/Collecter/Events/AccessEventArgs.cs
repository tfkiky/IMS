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

        private Maticsoft.Model.SMT_CARD_RECORDS _cardRecord;

        public Maticsoft.Model.SMT_CARD_RECORDS CardRecord
        {
            get { return _cardRecord; }
        }

        public AccessEventArgs(Maticsoft.Model.SMT_STAFF_INFO staffInfo, Maticsoft.Model.SMT_CARD_RECORDS cardRecord)
        {
            _staffInfo = staffInfo;
            _cardRecord = cardRecord;
        }
    }
}