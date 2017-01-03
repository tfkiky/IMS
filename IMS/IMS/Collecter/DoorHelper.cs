using IMS.Common.Config;
using Li.Access.Core;
using Li.Access.Core.WGAccesses;
using SmartAccess.Common.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.Collecter
{
    public class DoorHelper
    {
        /// <summary>
        /// 设置所有门禁为常关模式
        /// </summary>
        public static void SetControlType(DoorControlStyle style)
        {
            int faceControllerID, faceDoorID;
            faceControllerID = int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "Controller"));
            faceDoorID = int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "Door"));

            Maticsoft.BLL.SMT_CONTROLLER_INFO ctrlBll = new Maticsoft.BLL.SMT_CONTROLLER_INFO();
            List<Maticsoft.Model.SMT_CONTROLLER_INFO> ctrlList = ctrlBll.GetModelList("ID=" + faceControllerID);
            Maticsoft.BLL.SMT_DOOR_INFO doorBll = new Maticsoft.BLL.SMT_DOOR_INFO();
            Maticsoft.Model.SMT_DOOR_INFO door = new Maticsoft.Model.SMT_DOOR_INFO();
            if (ctrlList != null && ctrlList.Count > 0)
            {
                foreach (Maticsoft.Model.SMT_CONTROLLER_INFO ctrl in ctrlList)
                {
                    Controller c = ControllerHelper.ToController(ctrl);
                    door = doorBll.GetModel((decimal)faceDoorID);

                    if (door != null)
                    {
                        //设置门控制方式
                        using (IAccessCore access = new WGAccess())
                        {
                            bool ret = access.SetDoorControlStyle(c, (int)door.CTRL_DOOR_INDEX, style);
                            if (!ret)
                            {
                                //mlog.Info("设置门禁模式失败");
                                return;
                            }
                        }
                    }
                }
            }
        }

    }
}
