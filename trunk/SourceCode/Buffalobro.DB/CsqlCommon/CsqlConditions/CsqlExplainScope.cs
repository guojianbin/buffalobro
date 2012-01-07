using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.QueryConditions;
using System.Data;
using Buffalobro.DB.EntityInfos;
using System.Collections;
using Buffalo.Kernel;

namespace Buffalobro.DB.CsqlCommon.CsqlConditions
{
    internal delegate CsqlCondition CsqlScopeExplainer(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo);

    internal class CsqlExplainScope
    {

        private static CsqlScopeExplainer[] arrExplainer = InitExplainer();

        /// <summary>
        /// 初始化条件
        /// </summary>
        /// <returns></returns>
        private static CsqlScopeExplainer[] InitExplainer()
        {
            arrExplainer = new CsqlScopeExplainer[15];
            arrExplainer[(int)ScopeType.Between] = Between;
            arrExplainer[(int)ScopeType.Condition] = Condition;
            arrExplainer[(int)ScopeType.Contains] = Contains;
            arrExplainer[(int)ScopeType.EndWith] = EndWith;
            arrExplainer[(int)ScopeType.Equal] = Equal;
            arrExplainer[(int)ScopeType.IN] = IN;
            arrExplainer[(int)ScopeType.Less] = Less;
            arrExplainer[(int)ScopeType.LessThen] = LessThen;
            arrExplainer[(int)ScopeType.Like] = Like;
            arrExplainer[(int)ScopeType.More] = More;
            arrExplainer[(int)ScopeType.MoreThen] = MoreThen;
            arrExplainer[(int)ScopeType.NotEqual] = NotEqual;
            arrExplainer[(int)ScopeType.NotIn] = NotIn;
            arrExplainer[(int)ScopeType.Scope] = DoScope;
            arrExplainer[(int)ScopeType.StarWith] = StarWith;

            return arrExplainer;
        }

        /// <summary>
        /// 根据条件获取处理函数
        /// </summary>
        /// <param name="objScope"></param>
        /// <returns></returns>
        internal static CsqlScopeExplainer GetExplainer(Scope objScope)
        {
            int index = (int)objScope.ScopeType;
            if (index >= 0 && index < arrExplainer.Length)
            {
                return arrExplainer[index];
            }
            return null;
        }

        /// <summary>
        /// 解释Between
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition Between(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo) 
        {
            CsqlValueTypeItem cvalue1 = new CsqlValueTypeItem(scope.Value1);
            CsqlValueTypeItem cvalue2 = null;
            if (scope.Value2 != null)
            {
                cvalue2 = new CsqlValueTypeItem(scope.Value2);
            }

            return table[paramName, dbType].Between(cvalue1, cvalue2);
        }

        /// <summary>
        /// 解释IN
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition IN(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            CsqlValueTypeItem cvalue1 = new CsqlValueTypeItem(scope.Value1);

            return table[paramName, dbType].In(scope.Value1 as IEnumerable);
        }

        /// <summary>
        /// 解释NotIn
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition NotIn(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            CsqlValueTypeItem cvalue1 = new CsqlValueTypeItem(scope.Value1);

            return table[paramName, dbType].NotIn(scope.Value1 as IEnumerable);
        }

        /// <summary>
        /// 解释Less
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition Less(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            CsqlValueTypeItem cvalue1 = new CsqlValueTypeItem(scope.Value1);
            return table[paramName, dbType] < cvalue1;
        }


        /// <summary>
        /// 解释LessThen
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition LessThen(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            CsqlValueTypeItem cvalue1 = new CsqlValueTypeItem(scope.Value1);


            return table[paramName, dbType] <= cvalue1;
        }

        /// <summary>
        /// 解释More
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition More(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            CsqlValueTypeItem cvalue1 = new CsqlValueTypeItem(scope.Value1);
            return table[paramName, dbType] > cvalue1;
        }

        /// <summary>
        /// 解释MoreThen
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition MoreThen(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            CsqlValueTypeItem cvalue1 = new CsqlValueTypeItem(scope.Value1);
            return table[paramName, dbType] >= cvalue1;
        }

        /// <summary>
        /// 解释NotEqual
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition NotEqual(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            CsqlValueTypeItem cvalue1 = new CsqlValueTypeItem(scope.Value1);
            return table[paramName, dbType] != cvalue1;
        }

        /// <summary>
        /// 解释Equal
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition Equal(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            CsqlValueTypeItem cvalue1 = new CsqlValueTypeItem(scope.Value1);
            return table[paramName, dbType] == cvalue1;
        }

        /// <summary>
        /// 解释Like
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition Like(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            CsqlValueTypeItem cvalue1 = new CsqlValueTypeItem(scope.Value1);
            return table[paramName, dbType].Like(cvalue1);
        }

        /// <summary>
        /// 解释Contains
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition Contains(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            CsqlValueTypeItem cvalue1 = new CsqlValueTypeItem(scope.Value1);
            return table[paramName, dbType].Contains(cvalue1);
        }

        /// <summary>
        /// 解释StarWith
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition StarWith(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            CsqlValueTypeItem cvalue1 = new CsqlValueTypeItem(scope.Value1);
            return table[paramName, dbType].StarWith(cvalue1);
        }

        /// <summary>
        /// 解释EndWith
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition EndWith(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            CsqlValueTypeItem cvalue1 = new CsqlValueTypeItem(scope.Value1);
            return table[paramName, dbType].EndWith(cvalue1);
        }

        /// <summary>
        /// 解释Condition
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition Condition(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            CsqlCondition fhandle = scope.Value1 as CsqlCondition;
            return fhandle;
        }

        /// <summary>
        /// 解释Scope
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static CsqlCondition DoScope(Scope scope, DbType dbType, string paramName, CsqlCondition handle, CsqlTableHandle table, EntityInfoHandle entityInfo)
        {
            ScopeList lstInnerScope = scope.Value1 as ScopeList;
            handle = CsqlConditionScope.FillCondition(handle, table, lstInnerScope, entityInfo);
            return handle;
        }

    }
}
