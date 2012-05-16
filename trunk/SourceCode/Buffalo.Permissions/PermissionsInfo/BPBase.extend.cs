using System;
using System.Data;
using System.Collections.Generic;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
namespace Buffalo.Permissions.PermissionsInfo
{
	
	/// <summary>
	///  权限基类
	/// </summary>
	public partial class BPBase
	{
        public int Save() 
        {
            if (Id == null) 
            {
                return Insert(true);
            }
            return Update();
        }
        public override int Update(bool optimisticConcurrency)
        {
            this.LaseUpdate = DateTime.Now;
            return base.Update(optimisticConcurrency);
        }
        public override int Insert(bool fillPrimaryKey)
        {
            this.CreateDate = DateTime.Now;
            this.LaseUpdate = DateTime.Now;
            return base.Insert(fillPrimaryKey);
        }
	}
}
