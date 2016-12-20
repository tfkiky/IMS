using IMS.Common.Database;
using Li.Access.Core;
using Li.Access.Core.WGAccesses;
using SmartAccess.Common.Datas;
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


        public static bool SetIMSConfig(string DataType, string DataKey,string DataValue)
        {
            Maticsoft.BLL.IMS_DATA_CONFIG imsConfigBll = new Maticsoft.BLL.IMS_DATA_CONFIG();
            List<Maticsoft.Model.IMS_DATA_CONFIG> imsConfigModelList = new List<Maticsoft.Model.IMS_DATA_CONFIG>();
            imsConfigModelList = imsConfigBll.GetModelList("DataType='" + DataType + "' AND DataKey='" + DataKey + "'");
            if (imsConfigModelList != null && imsConfigModelList.Count > 0)
            {
                imsConfigModelList[0].DataValue = DataValue;
                return imsConfigBll.Update(imsConfigModelList[0]);
            }
            else return false;
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


        public static bool TestController()
        {
            try
            {
                decimal ctrlid=decimal.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "Controller"));
                Maticsoft.BLL.SMT_CONTROLLER_INFO ctrlBll = new Maticsoft.BLL.SMT_CONTROLLER_INFO();
                Maticsoft.Model.SMT_CONTROLLER_INFO ctrl = ctrlBll.GetModel(ctrlid);
                decimal doorid = decimal.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "Door"));
                Maticsoft.BLL.SMT_DOOR_INFO doorBll = new Maticsoft.BLL.SMT_DOOR_INFO();
                Maticsoft.Model.SMT_DOOR_INFO door = doorBll.GetModel(doorid);

                if (ctrl != null && door != null)
                {
                    using (IAccessCore acc = new WGAccess())
                    {
                        Controller c = ControllerHelper.ToController(ctrl);
                        ControllerState cs = acc.GetControllerState(c);
                        if (cs!=null)
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                }
                else
                    return false;
            }
            catch {
                return false;
            }
        }

        public static bool TestCamera()
        {
            try
            {
                 Maticsoft.BLL.IMS_FACE_CAMERA faceCameraBll = new Maticsoft.BLL.IMS_FACE_CAMERA();
                Maticsoft.Model.IMS_FACE_CAMERA faceCamera;
                int faceCameraID = int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "FaceCamera"));
                faceCamera = faceCameraBll.GetModelList("ID=" + faceCameraID)[0];
                HikSDK.HikonComDevice hikCam = new HikSDK.HikonComDevice();
                bool bRet=hikCam.Login(faceCamera.CameraIP, int.Parse(faceCamera.CameraPort), faceCamera.CameraUser, faceCamera.CameraPwd);
                return bRet;
            }
            catch
            {
                return false;
            }
        }
    }

   
}
