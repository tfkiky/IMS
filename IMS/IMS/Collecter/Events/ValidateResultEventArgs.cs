using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.Collecter
{
    public class ValidateResultEventArgs : EventArgs
    {
        private string _captruePic;
        private string _localPic;
        private string _blackPic;
        private string _staffName;

       
        private int _validateValue;
        private ValidateResult _validateResult;

        public ValidateResultEventArgs(string staffName,string captruePic, string localPic,string blackPic, int validateValue, ValidateResult validateResult)
        {
            _staffName = staffName;
            _captruePic = captruePic;
            _localPic = localPic;
            _blackPic = blackPic;
            _validateValue = validateValue;
            _validateResult = validateResult;
        }

        public string StaffName
        {
            get { return _staffName; }
        }

        public string CaptruePic
        {
            get
            {
                return _captruePic;
            }
        }

        public string LocalPic
        {
            get
            {
                return _localPic;
            }
        }

        public string BlackPic
        {
            get
            {
                return _blackPic;
            }
        }

        public int ValidateValue
        {
            get
            {
                return _validateValue;
            }
        }

        public ValidateResult ValidateResult
        {
            get
            {
                return _validateResult;
            }
        }
    }
    
    public enum ValidateResult
    {
        Success=0,
        NoPerson,
        Black,
        Error
    }
}
