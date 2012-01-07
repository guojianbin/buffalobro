using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.CsqlCommon.CsqlAggregateFunctions;
using Buffalobro.DB.DataBaseAdapter.IDbAdapters;
using Buffalobro.DB.CsqlCommon.CsqlExtendFunction;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.Kernel.Defaults;
using Buffalobro.DB.QueryConditions;
using System.Data;
using Buffalobro.DB.DBFunction;
using Buffalo.Kernel;

namespace Buffalobro.DB.CsqlCommon.CsqlBaseFunction
{
    /// <summary>
    /// CSQL关键字
    /// </summary>
    public class CSQL
    {
        /// <summary>
        /// Select表
        /// </summary>
        /// <param name="args">要输出的字段</param>
        /// <returns></returns>
        public static KeyWordSelectItem Select(params CsqlParamHandle[] args)
        {
            KeyWordSelectItem selectItem = new KeyWordSelectItem(args, null);
            return selectItem;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="arg">要被插入的表</param>
        /// <returns></returns>
        public static KeyWordInserItem InsertInto(CsqlTableHandle arg)
        {
            KeyWordInserItem item = new KeyWordInserItem(arg, null);
            return item;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="arg">要被更新的表</param>
        /// <returns></returns>
        public static KeyWordUpdateItem Update(CsqlTableHandle arg)
        {
            KeyWordUpdateItem item = new KeyWordUpdateItem(arg, null);
            return item;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="arg">要被删除数据的表</param>
        /// <returns></returns>
        public static KeyWordDeleteItem DeleteFrom(CsqlTableHandle arg)
        {
            KeyWordDeleteItem item = new KeyWordDeleteItem(arg, null);
            return item;
        }

        /// <summary>
        /// Case
        /// </summary>
        /// <param name="value">要选择的值或表达式</param>
        /// <returns></returns>
        public static KeyWordCaseItem Case(CsqlValueItem value)
        {
            KeyWordCaseItem item = new KeyWordCaseItem(value, null);
            return item;
        }

        /// <summary>
        /// Not
        /// </summary>
        /// <param name="value">Not</param>
        /// <returns></returns>
        public static CsqlComparItem Not(CsqlCondition handle)
        {

            CsqlComparItem fHandle = new CsqlComparItem(FunctionManager.DoNot, new CsqlValueItem[] { handle });
            return fHandle;
        }

        /// <summary>
        /// Case
        /// </summary>
        /// <returns></returns>
        public static KeyWordCaseItem Case()
        {
            KeyWordCaseItem item = new KeyWordCaseItem(null, null);
            return item;
        }


        #region 别名
        /// <summary>
        /// 一个别名表
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="asName">别名</param>
        /// <returns></returns>
        public static CsqlAliasHandle TableAs(CsqlEntityTableHandle table, string asName)
        {
            CsqlAliasHandle item = new CsqlAliasHandle(table, asName);
            return item;
        }
        /// <summary>
        /// 别名查询
        /// </summary>
        /// <param name="query">查询</param>
        /// <param name="asName">别名</param>
        /// <returns></returns>
        public static CsqlAliasHandle TableAs(CsqlQuery query, string asName)
        {
            CsqlAliasHandle item = new CsqlAliasHandle(query, asName);
            return item;
        }
        /// <summary>
        /// 别名字段
        /// </summary>
        /// <param name="param">字段</param>
        /// <param name="asName">别名</param>
        /// <returns></returns>
        public static CsqlAliasParamHandle ParamAs(CsqlParamHandle param, string asName)
        {
            CsqlAliasParamHandle item = new CsqlAliasParamHandle(param, asName);
            return item;
        }
        private static AliasTableCollection tabColl = new AliasTableCollection();
        /// <summary>
        /// 别名表
        /// </summary>
        public static AliasTableCollection Tables
        {
            get
            {
                return tabColl;
            }
        }

        public static CsqlOtherTableHandle ToTable(string tableName)
        {
            return new CsqlOtherTableHandle(tableName);

        }
        
        /// <summary>
        /// 把字段名转为Param类型
        /// </summary>
        public static CsqlOtherParamHandle ToParam(string paramName)
        {
            return new CsqlOtherParamHandle(null, paramName);
        }
        //private static CsqlAlias _alias = new CsqlAlias();
        ///// <summary>
        ///// 别名函数
        ///// </summary>
        //protected static CsqlAlias ALIAS
        //{
        //    get
        //    {
        //        return _alias;
        //    }
        //}
        #endregion

        #region 常用函数

        //ICommonFunction common = DbAdapterLoader.Common;
        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="source">源值</param>
        /// <param name="nullValue">如果为空的话的输出值</param>
        /// <returns></returns>
        public static CsqCommonFunction IsNull(CsqlValueItem source, CsqlValueItem nullValue)
        {

            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { source, nullValue }, DBCommonFunction.IsNull, source.ValueDbType);
            if (!CommonMethods.IsNull(nullValue))
            {
                handle.ValueDbType = nullValue.ValueDbType;
            }
            else if (!CommonMethods.IsNull(source))
            {
                handle.ValueDbType = source.ValueDbType;
            }
            return handle;
        }
        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="source">源值</param>
        /// <returns></returns>
        public static CsqCommonFunction Length(CsqlValueItem source)
        {

            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { source }, DBCommonFunction.Len, DbType.Int32);

            //handle.ValueDataType = DefaultType.IntType;

            return handle;
        }

        /// <summary>
        /// distinct，不重复字段
        /// </summary>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static CsqCommonFunction Distinct(params CsqlParamHandle[] cparams)
        {
            CsqCommonFunction handle = new CsqCommonFunction(cparams, DBCommonFunction.Distinct, DbType.Object);
            return handle;
        }
        /// <summary>
        /// 调用自定义函数
        /// </summary>
        /// <param name="source">源值</param>
        /// <returns></returns>
        public static CsqlCustomizeFunction Call(string functionName, params CsqlValueItem[] values)
        {
            CsqlCustomizeFunction handle = new CsqlCustomizeFunction(functionName, values);
            return handle;
        }
        
        #endregion

        #region 聚合函数
        //IAggregateFunctions agf = DbAdapterLoader.Aggregate;
        /// <summary>
        /// 总计
        /// </summary>
        /// <param name="functionHandle">函数</param>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static CsqlAggregateFunction Sum(CsqlParamHandle param)
        {
            CsqlAggregateFunction item = new CsqlAggregateFunction(DBAggregateFunction.DoSum, param);
            item.ValueDbType = DbType.Double;
            //item.ValueDataType = DefaultType.DoubleType;
            return item;
        }
        /// <summary>
        /// StdDev
        /// </summary>
        /// <param name="functionHandle">函数</param>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static CsqlAggregateFunction StdDev(CsqlParamHandle param)
        {
            CsqlAggregateFunction item = new CsqlAggregateFunction(DBAggregateFunction.DoStdDev, param);
            item.ValueDbType = DbType.Double;
            return item;
        }

        /// <summary>
        /// 最小
        /// </summary>
        /// <param name="functionHandle">函数</param>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static CsqlAggregateFunction Min(CsqlParamHandle param)
        {
            CsqlAggregateFunction item = new CsqlAggregateFunction(DBAggregateFunction.DoMin, param);
            return item;
        }
        /// <summary>
        /// 最大
        /// </summary>
        /// <param name="functionHandle">函数</param>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static CsqlAggregateFunction Max(CsqlParamHandle param)
        {
            CsqlAggregateFunction item = new CsqlAggregateFunction(DBAggregateFunction.DoMax, param);
            return item;
        }
        /// <summary>
        /// 总行数
        /// </summary>
        /// <param name="functionHandle">函数</param>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static CsqlAggregateFunction Count(CsqlParamHandle param)
        {
            CsqlAggregateFunction item = new CsqlAggregateFunction(DBAggregateFunction.DoCount, param);
            item.ValueDbType = DbType.Int32;
            return item;
        }
        /// <summary>
        /// 总行数
        /// </summary>
        /// <param name="functionHandle">函数</param>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static CsqlAggregateFunction Count()
        {

            return Count(null);
        }
        /// <summary>
        /// 平均值
        /// </summary>
        /// <param name="functionHandle">函数</param>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static CsqlAggregateFunction Avg(CsqlParamHandle param)
        {
            CsqlAggregateFunction item = new CsqlAggregateFunction(DBAggregateFunction.DoAvg, param);
            item.ValueDbType = DbType.Double;
            return item;
        }
        


        #endregion

        #region 数学函数
        /// <summary>
        /// 当前时间
        /// </summary>
        /// <returns></returns>
        public static CsqlNowDateHandle NowDate(DbType dbType)
        {
            CsqlNowDateHandle handle = new CsqlNowDateHandle(dbType);
            
            //handle.ValueDbType = DbType.Double;
            return handle;
        }
        /// <summary>
        /// 当前时间
        /// </summary>
        /// <returns></returns>
        public static CsqlNowDateHandle NowDate()
        {

            return NowDate(DbType.DateTime);
        }
        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="value">求绝对值</param>
        /// <returns></returns>
        public static CsqCommonFunction Abs(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoAbs, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }
        /// <summary>
        /// 反余弦函数
        /// </summary>
        /// <param name="value">弧度</param>
        /// <returns></returns>
        public static CsqCommonFunction Acos(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoAcos, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }

        /// <summary>
        /// 反正弦函数
        /// </summary>
        /// <param name="value">弧度</param>
        /// <returns></returns>
        public static CsqCommonFunction Asin(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoAsin, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }

        /// <summary>
        /// 反正切
        /// </summary>
        /// <param name="value">弧度</param>
        /// <returns></returns>
        public static CsqCommonFunction Atan(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoAtan, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }

        /// <summary>
        /// 计算两个变量 x 和 y 的反正切值
        /// </summary>
        /// <param name="y">定点的 y 坐标</param>
        /// <param name="x">定点的 x 坐标</param>
        /// <returns></returns>
        public static CsqCommonFunction Atan2(CsqlValueItem y, CsqlValueItem x)
        {
            x.ValueDbType = DbType.Double;
            y.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { y, x }, DBMathFunction.DoAtan2, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 返回大于或等于此数值的整数
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static CsqCommonFunction Ceil(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoCeil, DbType.Int32);
            //handle.ValueDataType = DefaultType.IntType;
            return handle;
        }
        /// <summary>
        /// 余弦
        /// </summary>
        /// <param name="value">弧度</param>
        /// <returns></returns>
        public static CsqCommonFunction Cos(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoCos, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }
        /// <summary>
        /// 求数值的e为底的幂 
        /// </summary>
        /// <param name="values">数值</param>
        /// <returns></returns>
        public static CsqCommonFunction DoExp(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoExp, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 返回小于或等于此数值的整数
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns></returns>
        public static CsqCommonFunction DoFloor(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoFloor, DbType.Int32);
            //handle.ValueDataType = DefaultType.IntType;
            return handle;
        }

        /// <summary>
        /// 取e为底的对数 
        /// </summary>
        /// <param name="values">数值</param>
        /// <returns></returns>
        public static CsqCommonFunction DoLn(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoLn, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 以10为底数的对数 
        /// </summary>
        /// <param name="values">数值</param>
        /// <returns></returns>
        public static CsqCommonFunction DoLog10(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoLog10, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 求任意数为底的幂 
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="value2">几次方</param>
        /// <returns></returns>
        public static CsqCommonFunction Power(CsqlValueItem value, CsqlValueItem value2)
        {
            value.ValueDbType = DbType.Double;
            value2.ValueDbType = DbType.Int32;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value, value2 }, DBMathFunction.DoPower, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 随机数
        /// </summary>
        /// <returns></returns>
        public static CsqCommonFunction Random()
        {

            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { }, DBMathFunction.DoRandom, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 四舍五入 
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static CsqCommonFunction Round(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoRound, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 取符号
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static CsqCommonFunction Sign(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoSign, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 正弦函数
        /// </summary>
        /// <param name="value">弧度</param>
        /// <returns></returns>
        public static CsqCommonFunction Sin(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoSin, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 平方根
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static CsqCommonFunction Sqrt(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoSqrt, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }
        /// <summary>
        /// 正切
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CsqCommonFunction Tan(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoTan, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        //private static CsqlMaths _csqlMaths = new CsqlMaths();
        ///// <summary>
        /////数学函数
        ///// </summary>
        //protected static CsqlMaths CMath
        //{
        //    get
        //    {
        //        return _csqlMaths;
        //    }
        //}
        #endregion

        #region 排序方式
        /// <summary>
        /// 产生顺序排序的项
        /// </summary>
        /// <param name="arg">字段</param>
        /// <returns></returns>
        public static CsqlOrderByHandle ASC(CsqlParamHandle arg)
        {
            CsqlOrderByHandle orderHandle = new CsqlOrderByHandle(arg, SortType.ASC);
            return orderHandle;
        }
        /// <summary>
        /// 产生倒叙排序的项
        /// </summary>
        /// <param name="arg">字段</param>
        /// <returns></returns>
        public static CsqlOrderByHandle DESC(CsqlParamHandle arg)
        {
            CsqlOrderByHandle orderHandle = new CsqlOrderByHandle(arg, SortType.DESC);
            return orderHandle;
        }

        ///// <summary>
        ///// Csql关键字
        ///// </summary>
        //protected static CsqlKeyWords CSQL
        //{
        //    get
        //    {
        //        return _csql;
        //    }
        //}
        //private static CsqlSorts _csqlsort = new CsqlSorts();

        ///// <summary>
        ///// 排序方式
        ///// </summary>
        //protected static CsqlSorts SORT
        //{
        //    get
        //    {
        //        return _csqlsort;
        //    }
        //}
        #endregion
    }
}
