﻿/**  版本信息模板在安装目录下，可自行修改。
* SMT_STAFF_CARD.cs
*
* 功 能： N/A
* 类 名： SMT_STAFF_CARD
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/10/9 19:15:45   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace Maticsoft.Model
{
	/// <summary>
	/// 员工卡关联表
	/// </summary>
	[Serializable]
	public partial class SMT_STAFF_CARD
	{
		public SMT_STAFF_CARD()
		{}
		#region Model
		private decimal _staff_id;
		private decimal _card_id;
		private DateTime _access_starttime;
		private DateTime _access_endtime;
		/// <summary>
		/// 
		/// </summary>
		public decimal STAFF_ID
		{
			set{ _staff_id=value;}
			get{return _staff_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal CARD_ID
		{
			set{ _card_id=value;}
			get{return _card_id;}
		}
		/// <summary>
		/// 门禁有效开始时间
		/// </summary>
		public DateTime ACCESS_STARTTIME
		{
			set{ _access_starttime=value;}
			get{return _access_starttime;}
		}
		/// <summary>
		/// 门禁有效结束时间
		/// </summary>
		public DateTime ACCESS_ENDTIME
		{
			set{ _access_endtime=value;}
			get{return _access_endtime;}
		}
		#endregion Model

	}
}

