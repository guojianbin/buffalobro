using System;
using System.Data;
using System.Collections.Generic;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.QueryConditions;
using Buffalo.Permissions.PermissionsInfo.BQLEntity;
namespace Buffalo.Permissions.PermissionsInfo
{
	
	/// <summary>
	///  系统模块
	/// </summary>
	public partial class BPSysModel
	{
        /// <summary>
        /// 获取根权限模块(所有权限)
        /// </summary>
        /// <returns></returns>
        public static BPSysModel RootModel() 
        {
            ScopeList lstScope=new ScopeList();
            lstScope.Add(PermissionDB.BPSysModel.ModelIdentify==".");
            BPSysModel root = BPSysModel.GetContext().GetUnique(lstScope);
            if (root == null) 
            {
                root = new BPSysModel();
                root.ModelIdentify = ".";
                root.ModelName = "所有权限";
                root.Save();
            }
            return root;
        }
	}
}
