using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
using System.Data;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.BQLCommon;

namespace Buffalo.Permissions.DataViewInfo
{
    /// <summary>
    /// 视图的统计类
    /// </summary>
    public class DataViewerSum
    {
        /// <summary>
        /// 获取统计的查询语句
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        public static DataTable GetSumQuery(DataViewer dv,ScopeList lstScope,DBInfo db)
        {
            List<BQLParamHandle> lstParams = new List<BQLParamHandle>();
            List<DataItem> outputItem = new List<DataItem>();
            foreach (DataItem item in dv) 
            {
                if (item.SumType == SumType.None) 
                {
                    continue;
                }
                lstParams.Add(GetParam(item,dv));
                outputItem.Add(item);
            }
            if (lstParams.Count <= 0) 
            {
                return new DataTable();
            }
            BQLDbBase dao=new BQLDbBase(db);

            BQLCondition where=BQLCondition.TrueValue;
            where = dao.FillCondition(where, dv.GetEntityHandle(), lstScope);

            BQLQuery query=BQL.Select(lstParams.ToArray()).
                From(dv.GetEntityHandle()).Where(where);

            DataTable dtRet = new DataTable(dv.GetEntityHandle().GetEntityInfo().TableName + "Sum");
            FillColumns(dtRet, outputItem);
            dtRet.BeginLoadData();
            using (IDataReader reader = dao.QueryReader(query)) 
            {
                DataRow dr = dtRet.NewRow();
                if (reader.Read())
                {
                    for (int i = 0; i < outputItem.Count; i++)
                    {
                        dr[i] = reader[i];
                        dtRet.Rows.Add(dr);
                    }
                }
            }
            dtRet.EndLoadData();
            return dtRet;
        }

        /// <summary>
        /// 填充列
        /// </summary>
        /// <param name="dtRet"></param>
        /// <param name="outputItem"></param>
        private static void FillColumns(DataTable dtRet, List<DataItem> outputItem) 
        {
            foreach (DataItem di in outputItem) 
            {
                DataColumn col=new DataColumn(di.PropertyName,di.PropertyType);
                col.AllowDBNull = true;
                dtRet.Columns.Add(col);
            }

        }

        /// <summary>
        /// 获取查询的统计字段
        /// </summary>
        /// <returns></returns>
        private static BQLParamHandle GetParam(DataItem item,DataViewer dv) 
        {
            BQLParamHandle handle = dv.GetEntityHandle()[item.PropertyName];
            switch (item.SumType) 
            {
                case SumType.Avg:
                    return BQL.Avg(handle);
                case SumType.Count:
                    return BQL.Count(handle);
                case SumType.Sum:
                    return BQL.Sum(handle);
                case SumType.Custom:
                    return item.CustomSum;
                default:
                    return null;
            }
        }
    }
}
