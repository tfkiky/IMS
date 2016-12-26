using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.Collecter
{
    public class ValidateResultEventArgs : EventArgs
    {
        private string _blackPic;

        private Maticsoft.Model.IMS_PEOPLE_RECORD _record;

        public Maticsoft.Model.IMS_PEOPLE_RECORD Record
        {
            get { return _record; }
            set { _record = value; }
        }
       
        private ValidateResult _validateResult;

        public ValidateResultEventArgs(Maticsoft.Model.IMS_PEOPLE_RECORD record,string blackPic, ValidateResult validateResult)
        {
            _record = record;
            _blackPic = blackPic;
            _validateResult = validateResult;
        }

      
        public string BlackPic
        {
            get
            {
                return _blackPic;
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
        Success = 0,
        NoPerson,
        Black,
        Error,
        BelowValue
    }
}
