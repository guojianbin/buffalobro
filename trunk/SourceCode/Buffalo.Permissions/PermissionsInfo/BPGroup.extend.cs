using System;
using System.Data;
using System.Collections.Generic;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.QueryConditions;
using Buffalo.Permissions.PermissionsInfo.BQLEntity;
using Buffalo.DB.CommBase.BusinessBases;
namespace Buffalo.Permissions.PermissionsInfo
{
	
	/// <summary>
	///  Ȩ����
	/// </summary>
	public partial class BPGroup
	{
        public bool Exists() 
        {
            ScopeList lstScope = new ScopeList();
            lstScope.Add(PermissionDB.BPGroup.GroupID == this.GroupID);
            if (this.Id != null) 
            {
                lstScope.Add(PermissionDB.BPGroup.Id != this.Id);
            }
            return GetContext().ExistsRecord(lstScope);
        }

        /// <summary>
        /// ͨ����ID��ȡ����Ϣ
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static BPGroup GetByGroupId(string groupId) 
        {
            ScopeList lstScope = new ScopeList();
            lstScope.Add(PermissionDB.BPGroup.GroupID == groupId);
            return GetContext().GetUnique(lstScope);
        }

        public override int Insert(bool fillPrimaryKey)
        {
            if (Exists()) 
            {
                return -1;
            }
            using (DBTransation tran = PermissionDB.StartTransation())
            {
                int ret =base.Insert(fillPrimaryKey);
                if (ret <= 0)
                {
                    return ret;
                }

                //��ӷ����Ĭ��Ȩ��Ϊȫ��
                BPSysModel root = BPSysModel.RootModel();
                BPGroupModelRight modelRight = new BPGroupModelRight();
                modelRight.BelongModel = root;
                modelRight.BelongGroup = this;
                modelRight.ModelRight = BPModelRights.Enable;
                modelRight.Save();

                tran.Commit();
                return ret;
            }
            
        }
        public override int Update(bool optimisticConcurrency)
        {
            if (Exists())
            {
                return -1;
            }
            return base.Update(optimisticConcurrency);
        }
        
	}
}
