using System;
using System.Data;
using System.Collections.Generic;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.QueryConditions;
using Buffalo.Permissions.PermissionsInfo.BQLEntity;
using Buffalo.Kernel;
using Buffalo.DB.CommBase.BusinessBases;
namespace Buffalo.Permissions.PermissionsInfo
{
	
	/// <summary>
	///  用户组的模块权限
	/// </summary>
	public partial class BPGroupModelRight
	{
        /// <summary>
        /// 获取组权限
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public static List<BPGroupModelRight> GetGroupModelRights(string groupID) 
        {
            BPGroup objGroup=BPGroup.GetByGroupId(groupID);

            if(objGroup==null)
            {
                return null;
            }

            ScopeList lstScope = new ScopeList();
            lstScope.OrderBy.Add(PermissionDB.BPSysModel.Id.ASC);
            List<BPSysModel> lstModelRights = BPSysModel.GetContext().SelectList(lstScope);

            lstScope = new ScopeList();
            lstScope.Add(PermissionDB.BPGroupModelRight.BelongGroup.GroupID == objGroup.GroupID);
            lstScope.Add(PermissionDB.BPGroupModelRight.BelongModel.Id >= 0);
            List<BPGroupModelRight> lstGMRight = BPGroupModelRight.GetContext().SelectList(lstScope);

            CheckModelRight(lstGMRight, lstModelRights, objGroup);

        }

        /// <summary>
        /// 检测组权限
        /// </summary>
        /// <param name="lstGMRight"></param>
        /// <param name="lstModelRights"></param>
        private static void CheckModelRight(List<BPGroupModelRight> lstGMRight, List<BPSysModel> lstModelRights,BPGroup group) 
        {
            Dictionary<string, BPGroupModelRight> dicModelRights = CommonMethods.ListToDictionary<string, BPGroupModelRight>(
                lstGMRight, "ModelId");
            BPGroupModelRight curRight=null;
            foreach (BPSysModel model in lstModelRights) 
            {
                string id = model.Id.ToString();
                if(!dicModelRights.TryGetValue(id,out curRight))
                {
                    curRight = new BPGroupModelRight();
                    curRight.BelongGroup = group;
                    curRight.BelongModel = model;
                    curRight.ModelRight = model.DefaultModelRight;
                    lstGMRight.Add(curRight);
                    dicModelRights[id] = curRight;
                }

                List<BPGroupItemRight> lstGroupItemRight=curRight.
                    CheckModelItemRight(
            }
        }

        /// <summary>
        /// 检测子项权限
        /// </summary>
        /// <param name="lstGroupItemRight"></param>
        /// <param name="lstItemRight"></param>
        /// <param name="?"></param>
        /// <param name="group"></param>
        private static void CheckModelItemRight(List<BPGroupItemRight> lstGroupItemRight,
            List<BPSysModelItem> lstItemRight,BPGroup group) 
        {
            Dictionary<string, BPGroupItemRight> dicModelRights = CommonMethods.ListToDictionary<string, BPGroupItemRight>(
                lstGroupItemRight, "ModelItemId");
            BPGroupItemRight curRight = null;
            foreach (BPSysModelItem item in lstItemRight)
            {
                string id = item.Id.ToString;
                if (!dicModelRights.TryGetValue(id, out curRight))
                {
                    curRight = new BPGroupItemRight();
                    curRight.BelongGroup = group;
                    curRight.BelongItem = item;
                    curRight.ItemRight = item.DefaultRight;
                    lstGroupItemRight.Add(curRight);
                    dicModelRights[id] = curRight;
                }
            }
        }


	}
}
