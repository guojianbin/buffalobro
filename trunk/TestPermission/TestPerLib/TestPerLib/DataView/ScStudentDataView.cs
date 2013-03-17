
using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Permissions.DataViewInfo;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace TestPerLib.DataView
{
	public class ScStudentDataView:DataViewer
	{
		
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
		
		private DataItem _belongClassName=null;
		private DataItem BelongClassName
		{
			get
			{
				return _belongClassName;
			}
			set
			{
				_belongClassName=value;
			}
		}
		
		
		/// <summary>
        /// 
        /// </summary>
        /// <param name="entityHandle"></param>
		public ScStudentDataView(BQLEntityTableHandle entityHandle)
		:base(entityHandle,true,true,true)
		{
			
			_name=CreateDataItem("Name",typeof(string),true,true,true,SumType.None,null);
			
			_age=CreateDataItem("Age",typeof(int),true,true,true,SumType.Avg,null);
			
			_belongClassName=CreateDataItem("BelongClassName",typeof(string),true,false,false,SumType.None,null);
			
		}
		
	}
}

