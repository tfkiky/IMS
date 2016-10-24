using IMS.Common.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.Common.Config
{
    /// <summary>
    /// 系统配置管理类
    /// </summary>
    public class SysConfigClass
    {
        private static DatabaseConfigClass datavbaseConfig = null;
        /// <summary>
        /// 获取Sql Server数据库连接
        /// </summary>
        /// <returns></returns>
        public static string GetSqlServerConnectString()
        {
            if (datavbaseConfig!=null)
            {
                return datavbaseConfig.ToString();
            }
            datavbaseConfig = GetSqlServerServerConfig();
            if (datavbaseConfig == null)
            {
                return null;
            }
            return datavbaseConfig.ToString();
        }

        public static DatabaseConfigClass GetSqlServerServerConfig()
        {
            if (datavbaseConfig != null)
            {
                return datavbaseConfig;
            }
            DatabaseConfigClass configCls = DatabaseConfigClass.GetConfig("SqlServerConnectString");
            return configCls;
        }
        public static bool SetSqlServerConfig(DatabaseConfigClass configcls)
        {
            datavbaseConfig = configcls;
            return configcls.SaveConfig("SqlServerConnectString");
        }

        /// <summary>
        /// 获取数据字典配置项
        /// </summary>
        /// <param name="DataType"></param>
        /// <param name="DataKey"></param>
        /// <returns></returns>
        public static string GetIMSConfig(string DataType, string DataKey)
        {
            Maticsoft.BLL.IMS_DATA_CONFIG imsConfigBll = new Maticsoft.BLL.IMS_DATA_CONFIG();
            List<Maticsoft.Model.IMS_DATA_CONFIG> imsConfigModelList = new List<Maticsoft.Model.IMS_DATA_CONFIG>();
            imsConfigModelList = imsConfigBll.GetModelList("DataType='" + DataType + "' AND DataKey='" + DataKey + "'");
            if (imsConfigModelList != null && imsConfigModelList.Count > 0)
            {
                return imsConfigModelList[0].DataValue;
            }
            else return null;
        }

        public static bool TestDBConn()
        {
            try
            {
                Maticsoft.BLL.IMS_DATA_CONFIG imsConfigBll = new Maticsoft.BLL.IMS_DATA_CONFIG();
                List<Maticsoft.Model.IMS_DATA_CONFIG> imsConfigModelList = new List<Maticsoft.Model.IMS_DATA_CONFIG>();
                imsConfigModelList = imsConfigBll.GetModelList("1=1");
                if (imsConfigModelList != null && imsConfigModelList.Count > 0)
                {
                    return true;
                }
                else return true;
            }
            catch
            {
                return false;
            }
        }

    }

   
}
