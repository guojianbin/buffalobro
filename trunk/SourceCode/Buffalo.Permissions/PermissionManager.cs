using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Permissions.PermissionsInfo.BQLEntity;
using System.Reflection;
using Buffalo.Permissions.PermissionsInfo;
using Buffalo.Kernel;
using Buffalo.DB.CommBase.BusinessBases;

namespace Buffalo.Permissions
{
    /// <summary>
    /// ��ĿȨ�޼��
    /// </summary>
    public class PermissionManager
    {
        /// <summary>
        /// ����û���
        /// </summary>
        /// <param name="groupId">���ʶ(��ID)</param>
        /// <param name="groupName">����</param>
        /// <returns></returns>
        public static bool AddUserGroup(string groupId,string groupName) 
        {
             
            BPGroup objGroup = new BPGroup();
            objGroup.GroupID = groupId;
            objGroup.GroupName = groupName;
            int ret=objGroup.Save();
            return ret>0;
        }

        /// <summary>
        /// ���Ȩ��
        /// </summary>
        public static void CheckPermission(params Assembly[] arrAss) 
        { 
            //������ݿ�ṹ
            PermissionDB.GetDBinfo().UpdateDataBase();

            List<BPSysModel> lstModels = BPSysModel.GetContext().SelectList(new Buffalo.DB.QueryConditions.ScopeList());
            Dictionary<string,BPSysModel> dicModel=CommonMethods.ListToDictionary<string, BPSysModel>(lstModels, "ModelIdentify");
            
            foreach (Assembly objAss in arrAss) 
            {
                foreach (Type objType in objAss.GetTypes()) 
                {
                    if (objType.IsClass) 
                    {
                        MethodInfo mi= objType.GetMethod("GetPermissionsInfo",new Type[]{});
                        if (mi != null && mi.IsStatic) 
                        {
                            object obj=mi.Invoke(null, new object[] { });
                            BPSysModel model = obj as BPSysModel;
                            if (model != null) 
                            {
                                CheckModelInfo(dicModel, model);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ���ģ����Ϣ
        /// </summary>
        /// <param name="dicModel">ϵͳģ��</param>
        /// <param name="model">�ִ�ģ��</param>
        private static void CheckModelInfo(Dictionary<string, BPSysModel> dicModel, BPSysModel model)
        {
            string nameSpace = model.ModelIdentify;
            BPSysModel cmodel = null;
            if (!dicModel.TryGetValue(nameSpace, out cmodel))
            {
                cmodel = new BPSysModel();

                cmodel.ModelIdentify = model.ModelIdentify;

                dicModel[cmodel.ModelIdentify] = cmodel;
            }

            cmodel.ModelDescription = model.ModelDescription;
            cmodel.ModelName = model.ModelName;
            cmodel.ModelType = model.ModelType;
            cmodel.Save();
            foreach (BPSysModelItem item in model.LstModelItem)
            {
                bool exists = false;
                foreach (BPSysModelItem citem in cmodel.LstModelItem)
                {
                    if (citem.ItemIdentify.Equals(item.ItemIdentify, StringComparison.CurrentCultureIgnoreCase)) //����
                    {
                        item.ItemName = citem.ItemName;
                        item.ItemDescription = citem.ItemDescription;
                        item.Save();
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {


                    item.BelongModel = cmodel;
                    item.Save();
                }
            }

        }

    }
}
