using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.Collecter
{
    public class IDCardClass
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string sex;

        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }
        private string nation;

        public string Nation
        {
            get { return nation; }
            set { nation = value; }
        }
        private string birth;

        public string Birth
        {
            get { return birth; }
            set { birth = value; }
        }
        private string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string signGov;

        public string SignGov
        {
            get { return signGov; }
            set { signGov = value; }
        }
        private string startDate;

        public string StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        private string limitDate;

        public string LimitDate
        {
            get { return limitDate; }
            set { limitDate = value; }
        }

        private string photoFile;

        public string PhotoFile
        {
            get { return photoFile; }
            set { photoFile = value; }
        }
    }
}
