
using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Permissions.DataViewInfo;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace TestPerLib.DataView
{
	public class ScClassDataView:DataViewer
	{
		
		private DataItem _createDate=null;
		private DataItem CreateDate
		{
			get
			{
				return _createDate;
			}
			set
			{
				_createDate=value;
			}
		}
		
		private DataItem _className=null;
		private DataItem ClassName
		{
			get
			{
				return _className;
			}
			set
			{
				_className=value;
			}
		}
		
		
		/// <summary>
        /// 
        /// </summary>
        /// <param name="entityHandle"></param>
		public ScClassDataView(BQLEntityTableHandle entityHandle)
		:base(entityHandle,true,true,true)
		{
			
			_createDate=CreateDataItem("CreateDate",typeof(DateTime),true,true,true,SumType.None,null);
			
			_className=CreateDataItem("ClassName",typeof(string),true,true,true,SumType.None,null);
			
		}
		
	}
}

