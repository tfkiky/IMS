using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.Common.Data
{
    public class SexHelper
    {
        public static string GetSex(string sexCode)
        {
            switch (sexCode)
            {
                case "0":
                    return "未知";
                case "1":
                    return "男";
                case "2":
                    return "女";
                case "9":
                    return "未说明";
                default:
                    return "男";
            }
        }
    }
}
