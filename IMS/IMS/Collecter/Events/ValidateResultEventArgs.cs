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
        private int _validateValue;
        private ValidateResult _validateResult;

        public ValidateResultEventArgs(string captruePic, string localPic, int validateValue, ValidateResult validateResult)
        {
            _captruePic = captruePic;
            _localPic = localPic;
            _validateValue = validateValue;
            _validateResult = validateResult;
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
        Error
    }
}
