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

        public IDCardEventArgs(IDCardClass idCard)
        {
            _idCard = idCard;
        }
    }
}
