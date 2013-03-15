
using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Permissions.DataViewInfo;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace TestPerLib.DataView
{
	public class ScStudentDataView:DataViewer
	{
		
		private DataItem _lastDate=null;
		private DataItem LastDate
		{
			get
			{
				return _lastDate;
			}
			set
			{
				_lastDate=value;
			}
		}
		
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
		
		private DataItem _name=null;
		private DataItem Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}
		
		private DataItem _age=null;
		private DataItem Age
		{
			get
			{
				return _age;
			}
			set
			{
				_age=value;
			}
		}
		
		
		/// <summary>
        /// 
        /// </summary>
        /// <param name="entityHandle"></param>
		public ScStudentDataView(BQLEntityTableHandle entityHandle)
		:base(entityHandle,true,true,true)
		{
			
			_lastDate=CreateDataItem("LastDate",typeof(DateTime),true,false,false,SumType.None,null);
			
			_createDate=CreateDataItem("CreateDate",typeof(DateTime),true,true,true,SumType.None,null);
			
			_name=CreateDataItem("Name",typeof(string),true,true,true,SumType.None,null);
			
			_age=CreateDataItem("Age",typeof(int),true,true,true,SumType.None,null);
			
		}
		
	}
}

