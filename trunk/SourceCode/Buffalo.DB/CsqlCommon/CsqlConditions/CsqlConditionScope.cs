using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.CsqlCommon.CsqlKeyWordCommon;
using Buffalo.DB.CsqlCommon.CsqlExtendFunction;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;
using System.Collections;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;
using Buffalo.Kernel;

namespace Buffalo.DB.CsqlCommon.CsqlConditions
{
    public class CsqlConditionScope
    {

        /// <summary>
        /// 填充查询条件并返回条件的SQL语句( and 开头)
        /// </summary>
        /// <param name="lstParam">参数列表</param>
        /// <param name="lstScope">范围查询集合</param>
        /// <returns></returns>
        internal static CsqlCondition FillCondition(CsqlCondition condition, CsqlTableHandle table, ScopeList lstScope, EntityInfoHandle entityInfo)
        {
            CsqlCondition ret = condition;
            if (lstScope == null)
            {
                return ret;
            }
            CsqlCondition curHandle = null;
            for (int i = 0; i < lstScope.Count; i++)
            {
                Scope objScope = lstScope[i];
                EntityPropertyInfo info = null;
                if (entityInfo != null)
                {
                    if (objScope.ScopeType == ScopeType.Condition)
                    {
                        curHandle = objScope.Value1 as CsqlCondition;
                    }
                    else
                    {
                        //info = entityInfo.PropertyInfo[objScope.PropertyName];
                        curHandle = FormatScorp(objScope, DbType.Object, objScope.PropertyName, ret, table, entityInfo);
                    }
                }
                else
                {
                    curHandle = FormatScorp(objScope, DbType.Object, objScope.PropertyName, ret, table, entityInfo);
                }

                if (!Buffalo.Kernel.CommonMethods.IsNull(curHandle))
                {
                    if (objScope.ConnectType == ConnectType.And)
                    {
                        ret = ret & curHandle;
                    }
                    else
                    {
                        ret = ret | curHandle;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取排序列表
        /// </summary>
        /// <param name="lstScort"></param>
        /// <param name="table"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        internal static CsqlParamHandle[] GetSort(SortList lstScort, CsqlTableHandle table, EntityInfoHandle entityInfo) 
        {
            List<CsqlParamHandle> lstHandles = new List<CsqlParamHandle>();
            if (lstScort == null)
            {
                lstHandles.ToArray();
            }
            
            
            for (int i = 0; i < lstScort.Count; i++)
            {
                Sort objSort = lstScort[i];
                
                CsqlParamHandle handle = null;
                if (!CommonMethods.IsNull(objSort.OrderHandle)) 
                {
                    handle = objSort.OrderHandle;
                }
                else if (entityInfo != null)
                {
                    //EntityPropertyInfo info = entityInfo.PropertyInfo[objSort.PropertyName];
                    if (objSort.SortType == SortType.ASC)
                    {
                        handle = table[objSort.PropertyName].ASC;
                    }
                    else 
                    {
                        handle = table[objSort.PropertyName].DESC;
                    }
                }
                else
                {
                    if (objSort.SortType == SortType.ASC)
                    {
                        handle = table[objSort.PropertyName].ASC;
                    }
                    else
                    {
                        handle = table[objSort.PropertyName].DESC;
                    }
                }
                lstHandles.Add(handle);
            }
            return lstHandles.ToArray();
        }

        private static CsqlCondition FormatScorp(Scope scope, DbType dbType, string pro, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            ScopeType ctype = scope.ScopeType;
            CsqlScopeExplainer delFun=CsqlExplainScope.GetExplainer(scope);
            if (delFun != null) 
            {
                handle = delFun(scope, dbType, pro, handle, table, entityInfo);
            }
            
            return handle;
        }

    }
}
