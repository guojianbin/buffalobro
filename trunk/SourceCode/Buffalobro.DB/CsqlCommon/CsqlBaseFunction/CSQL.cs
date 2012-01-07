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
    /// CSQL�ؼ���
    /// </summary>
    public class CSQL
    {
        /// <summary>
        /// Select��
        /// </summary>
        /// <param name="args">Ҫ������ֶ�</param>
        /// <returns></returns>
        public static KeyWordSelectItem Select(params CsqlParamHandle[] args)
        {
            KeyWordSelectItem selectItem = new KeyWordSelectItem(args, null);
            return selectItem;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="arg">Ҫ������ı�</param>
        /// <returns></returns>
        public static KeyWordInserItem InsertInto(CsqlTableHandle arg)
        {
            KeyWordInserItem item = new KeyWordInserItem(arg, null);
            return item;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="arg">Ҫ�����µı�</param>
        /// <returns></returns>
        public static KeyWordUpdateItem Update(CsqlTableHandle arg)
        {
            KeyWordUpdateItem item = new KeyWordUpdateItem(arg, null);
            return item;
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="arg">Ҫ��ɾ�����ݵı�</param>
        /// <returns></returns>
        public static KeyWordDeleteItem DeleteFrom(CsqlTableHandle arg)
        {
            KeyWordDeleteItem item = new KeyWordDeleteItem(arg, null);
            return item;
        }

        /// <summary>
        /// Case
        /// </summary>
        /// <param name="value">Ҫѡ���ֵ����ʽ</param>
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


        #region ����
        /// <summary>
        /// һ��������
        /// </summary>
        /// <param name="table">��</param>
        /// <param name="asName">����</param>
        /// <returns></returns>
        public static CsqlAliasHandle TableAs(CsqlEntityTableHandle table, string asName)
        {
            CsqlAliasHandle item = new CsqlAliasHandle(table, asName);
            return item;
        }
        /// <summary>
        /// ������ѯ
        /// </summary>
        /// <param name="query">��ѯ</param>
        /// <param name="asName">����</param>
        /// <returns></returns>
        public static CsqlAliasHandle TableAs(CsqlQuery query, string asName)
        {
            CsqlAliasHandle item = new CsqlAliasHandle(query, asName);
            return item;
        }
        /// <summary>
        /// �����ֶ�
        /// </summary>
        /// <param name="param">�ֶ�</param>
        /// <param name="asName">����</param>
        /// <returns></returns>
        public static CsqlAliasParamHandle ParamAs(CsqlParamHandle param, string asName)
        {
            CsqlAliasParamHandle item = new CsqlAliasParamHandle(param, asName);
            return item;
        }
        private static AliasTableCollection tabColl = new AliasTableCollection();
        /// <summary>
        /// ������
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
        /// ���ֶ���תΪParam����
        /// </summary>
        public static CsqlOtherParamHandle ToParam(string paramName)
        {
            return new CsqlOtherParamHandle(null, paramName);
        }
        //private static CsqlAlias _alias = new CsqlAlias();
        ///// <summary>
        ///// ��������
        ///// </summary>
        //protected static CsqlAlias ALIAS
        //{
        //    get
        //    {
        //        return _alias;
        //    }
        //}
        #endregion

        #region ���ú���

        //ICommonFunction common = DbAdapterLoader.Common;
        /// <summary>
        /// �ж��Ƿ�Ϊ��
        /// </summary>
        /// <param name="source">Դֵ</param>
        /// <param name="nullValue">���Ϊ�յĻ������ֵ</param>
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
        /// �ж��Ƿ�Ϊ��
        /// </summary>
        /// <param name="source">Դֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction Length(CsqlValueItem source)
        {

            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { source }, DBCommonFunction.Len, DbType.Int32);

            //handle.ValueDataType = DefaultType.IntType;

            return handle;
        }

        /// <summary>
        /// distinct�����ظ��ֶ�
        /// </summary>
        /// <param name="param">�ֶ�</param>
        /// <returns></returns>
        public static CsqCommonFunction Distinct(params CsqlParamHandle[] cparams)
        {
            CsqCommonFunction handle = new CsqCommonFunction(cparams, DBCommonFunction.Distinct, DbType.Object);
            return handle;
        }
        /// <summary>
        /// �����Զ��庯��
        /// </summary>
        /// <param name="source">Դֵ</param>
        /// <returns></returns>
        public static CsqlCustomizeFunction Call(string functionName, params CsqlValueItem[] values)
        {
            CsqlCustomizeFunction handle = new CsqlCustomizeFunction(functionName, values);
            return handle;
        }
        
        #endregion

        #region �ۺϺ���
        //IAggregateFunctions agf = DbAdapterLoader.Aggregate;
        /// <summary>
        /// �ܼ�
        /// </summary>
        /// <param name="functionHandle">����</param>
        /// <param name="param">�ֶ�</param>
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
        /// <param name="functionHandle">����</param>
        /// <param name="param">�ֶ�</param>
        /// <returns></returns>
        public static CsqlAggregateFunction StdDev(CsqlParamHandle param)
        {
            CsqlAggregateFunction item = new CsqlAggregateFunction(DBAggregateFunction.DoStdDev, param);
            item.ValueDbType = DbType.Double;
            return item;
        }

        /// <summary>
        /// ��С
        /// </summary>
        /// <param name="functionHandle">����</param>
        /// <param name="param">�ֶ�</param>
        /// <returns></returns>
        public static CsqlAggregateFunction Min(CsqlParamHandle param)
        {
            CsqlAggregateFunction item = new CsqlAggregateFunction(DBAggregateFunction.DoMin, param);
            return item;
        }
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="functionHandle">����</param>
        /// <param name="param">�ֶ�</param>
        /// <returns></returns>
        public static CsqlAggregateFunction Max(CsqlParamHandle param)
        {
            CsqlAggregateFunction item = new CsqlAggregateFunction(DBAggregateFunction.DoMax, param);
            return item;
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="functionHandle">����</param>
        /// <param name="param">�ֶ�</param>
        /// <returns></returns>
        public static CsqlAggregateFunction Count(CsqlParamHandle param)
        {
            CsqlAggregateFunction item = new CsqlAggregateFunction(DBAggregateFunction.DoCount, param);
            item.ValueDbType = DbType.Int32;
            return item;
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="functionHandle">����</param>
        /// <param name="param">�ֶ�</param>
        /// <returns></returns>
        public static CsqlAggregateFunction Count()
        {

            return Count(null);
        }
        /// <summary>
        /// ƽ��ֵ
        /// </summary>
        /// <param name="functionHandle">����</param>
        /// <param name="param">�ֶ�</param>
        /// <returns></returns>
        public static CsqlAggregateFunction Avg(CsqlParamHandle param)
        {
            CsqlAggregateFunction item = new CsqlAggregateFunction(DBAggregateFunction.DoAvg, param);
            item.ValueDbType = DbType.Double;
            return item;
        }
        


        #endregion

        #region ��ѧ����
        /// <summary>
        /// ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public static CsqlNowDateHandle NowDate(DbType dbType)
        {
            CsqlNowDateHandle handle = new CsqlNowDateHandle(dbType);
            
            //handle.ValueDbType = DbType.Double;
            return handle;
        }
        /// <summary>
        /// ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public static CsqlNowDateHandle NowDate()
        {

            return NowDate(DbType.DateTime);
        }
        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <param name="value">�����ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction Abs(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoAbs, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }
        /// <summary>
        /// �����Һ���
        /// </summary>
        /// <param name="value">����</param>
        /// <returns></returns>
        public static CsqCommonFunction Acos(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoAcos, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }

        /// <summary>
        /// �����Һ���
        /// </summary>
        /// <param name="value">����</param>
        /// <returns></returns>
        public static CsqCommonFunction Asin(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoAsin, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="value">����</param>
        /// <returns></returns>
        public static CsqCommonFunction Atan(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoAtan, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }

        /// <summary>
        /// ������������ x �� y �ķ�����ֵ
        /// </summary>
        /// <param name="y">����� y ����</param>
        /// <param name="x">����� x ����</param>
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
        /// ���ش��ڻ���ڴ���ֵ������
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction Ceil(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoCeil, DbType.Int32);
            //handle.ValueDataType = DefaultType.IntType;
            return handle;
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="value">����</param>
        /// <returns></returns>
        public static CsqCommonFunction Cos(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoCos, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }
        /// <summary>
        /// ����ֵ��eΪ�׵��� 
        /// </summary>
        /// <param name="values">��ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction DoExp(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoExp, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// ����С�ڻ���ڴ���ֵ������
        /// </summary>
        /// <param name="value">��ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction DoFloor(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoFloor, DbType.Int32);
            //handle.ValueDataType = DefaultType.IntType;
            return handle;
        }

        /// <summary>
        /// ȡeΪ�׵Ķ��� 
        /// </summary>
        /// <param name="values">��ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction DoLn(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoLn, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// ��10Ϊ�����Ķ��� 
        /// </summary>
        /// <param name="values">��ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction DoLog10(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoLog10, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// ��������Ϊ�׵��� 
        /// </summary>
        /// <param name="value">��ֵ</param>
        /// <param name="value2">���η�</param>
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
        /// �����
        /// </summary>
        /// <returns></returns>
        public static CsqCommonFunction Random()
        {

            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { }, DBMathFunction.DoRandom, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// �������� 
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction Round(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoRound, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// ȡ����
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction Sign(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoSign, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// ���Һ���
        /// </summary>
        /// <param name="value">����</param>
        /// <returns></returns>
        public static CsqCommonFunction Sin(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoSin, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// ƽ����
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction Sqrt(CsqlValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { value }, DBMathFunction.DoSqrt, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }
        /// <summary>
        /// ����
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
        /////��ѧ����
        ///// </summary>
        //protected static CsqlMaths CMath
        //{
        //    get
        //    {
        //        return _csqlMaths;
        //    }
        //}
        #endregion

        #region ����ʽ
        /// <summary>
        /// ����˳���������
        /// </summary>
        /// <param name="arg">�ֶ�</param>
        /// <returns></returns>
        public static CsqlOrderByHandle ASC(CsqlParamHandle arg)
        {
            CsqlOrderByHandle orderHandle = new CsqlOrderByHandle(arg, SortType.ASC);
            return orderHandle;
        }
        /// <summary>
        /// ���������������
        /// </summary>
        /// <param name="arg">�ֶ�</param>
        /// <returns></returns>
        public static CsqlOrderByHandle DESC(CsqlParamHandle arg)
        {
            CsqlOrderByHandle orderHandle = new CsqlOrderByHandle(arg, SortType.DESC);
            return orderHandle;
        }

        ///// <summary>
        ///// Csql�ؼ���
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
        ///// ����ʽ
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
