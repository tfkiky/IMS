﻿/**  版本信息模板在安装目录下，可自行修改。
* SMT_CONTROLLER_INFO_EX.cs
*
* 功 能： N/A
* 类 名： SMT_CONTROLLER_INFO
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/7/21 22:57:19   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
namespace Maticsoft.Model
{
	/// <summary>
	/// 控制器表
	/// </summary>
	public partial class SMT_CONTROLLER_INFO
	{
		#region Model
        private string _area_name="";//所属区域名称
        private List<SMT_DOOR_INFO> _door_infos = null;//所关联的门
        public string AREA_NAME
        {
            get { return _area_name; }
            set { _area_name = value; }
        }
        public List<SMT_DOOR_INFO> DOOR_INFOS
        {
            get { return _door_infos; }
            set { _door_infos = value; }
        }
		#endregion Model
	}
}

