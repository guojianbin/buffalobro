using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.Kernel.Defaults;
using Buffalo.Kernel;
using Buffalo.DB.BQLCommon.BQLExtendFunction;
using Buffalo.DB.DBFunction;
using Buffalo.DB.BQLCommon.BQLConditions;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    
    public abstract class BQLValueItem
    {
        //protected Type valueDataType;
        
        ///// <summary>
        ///// 数据库的值类型
        ///// </summary>
        //internal virtual Type ValueDataType
        //{
        //    get
        //    {
        //        return valueDataType;
        //    }
        //    set 
        //    {
        //        valueDataType = value;
        //    }
        //}

        /// <summary>
        /// 给字段定义一个别名
        /// </summary>
        /// <param name="asName">别名</param>
        /// <returns></returns>
        public BQLAliasParamHandle As(string asName)
        {
            BQLAliasParamHandle item = new BQLAliasParamHandle(this, asName);
            return item;
        }
        /// <summary>
        /// 给字段定义一个别名
        /// </summary>
        /// <param name="asName">别名</param>
        /// <returns></returns>
        public BQLAliasParamHandle As()
        {
            return As(null);
        }
        /// <summary>
        /// StarWith条件
        /// </summary>
        /// <param name="item">条件</param>
        /// <returns></returns>
        public BQLConditionItem StarWith(object item)
        {
            BQLValueItem oValue = BQLValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new BQLConditionItem(this, new BQLValueItem[] { oValue }, BQLConditionManager.DoStarWith);
        }

        /// <summary>
        /// EndWith条件
        /// </summary>
        /// <param name="item">条件</param>
        /// <returns></returns>
        public BQLConditionItem EndWith(object item)
        {

            BQLValueItem oValue = BQLValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new BQLConditionItem(this, new BQLValueItem[] { oValue }, BQLConditionManager.DoEndWith);
        }
        /// <summary>
        /// Between条件
        /// </summary>
        /// <param name="star">开始值</param>
        /// <param name="end">结束值</param>
        /// <returns></returns>
        public BQLConditionItem Between(object star, object end)
        {
            BQLValueItem oValue1 = BQLValueItem.ToValueItem(star);
            BQLValueItem oValue2 = BQLValueItem.ToValueItem(end);
            oValue1.ValueDbType = this.ValueDbType;
            oValue2.ValueDbType = this.ValueDbType;
            return new BQLConditionItem(this, new BQLValueItem[] { oValue1, oValue2 }, BQLConditionManager.DoBetween);
        }

        /// <summary>
        /// 全文检索的条件
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public BQLConditionItem Contains(object item)
        {
            BQLValueItem oValue = BQLValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new BQLConditionItem(this, new BQLValueItem[] { oValue }, BQLConditionManager.DoContains);

        }

        /// <summary>
        /// 查找字符
        /// </summary>
        /// <param name="value">要查找的字符</param>
        /// <param name="start">起始位置</param>
        /// <returns></returns>
        public CsqCommonFunction IndexOf(object value, BQLValueItem start)
        {
            BQLValueItem oValue = BQLValueItem.ToValueItem(value); ;

            start.ValueDbType = DbType.Int32;
            if (oValue.ValueDbType == DbType.Object)
            {
                oValue = DbType.String;
            }
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { oValue, this, start }, DBMathFunction.IndexOf, DbType.Int32);
            //handle.ValueDataType = DefaultType.IntType;
            handle.ValueDbType = DbType.Int32;
            return handle;
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="start">字符串起始位置</param>
        /// <param name="length">字符串长度</param>
        /// <returns></returns>
        public CsqCommonFunction SubString(BQLValueItem start, BQLValueItem length)
        {
            start.ValueDbType = DbType.Int32;
            length.ValueDbType = DbType.Int32;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { this, start, length }, DBMathFunction.SubString, DbType.Int32);
            //handle.ValueDataType = DefaultType.IntType;
            handle.ValueDbType = DbType.Int32;
            return handle;
        }
        /// <summary>
        /// Like条件
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public BQLConditionItem Like(object item)
        {

            BQLValueItem oValue = BQLValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new BQLConditionItem(this, new BQLValueItem[] { oValue }, BQLConditionManager.DoLike);
        }
        /// <summary>
        /// FreeText条件
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public BQLConditionItem FreeText(object item)
        {
            BQLValueItem oValue = BQLValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new BQLConditionItem(this, new BQLValueItem[] { oValue }, BQLConditionManager.DoLike);
        }
        /// <summary>
        /// 时间类型按指定格式转换到字符串
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public CsqConvertFunction DateTimeToString(string format)
        {
            CsqConvertFunction handle = new CsqConvertFunction(this, format, DBConvertFunction.DateTimeToString);
            //handle.ValueDataType = DefaultType.StringType;
            handle.ValueDbType = DbType.String;
            return handle;
        }
        /// <summary>
        /// 字符串按指定格式转换到时间类型
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public CsqConvertFunction StringToDateTime(string format)
        {
            CsqConvertFunction handle = new CsqConvertFunction(this, format, DBConvertFunction.StringToDateTime);
            handle.ValueDbType = DbType.DateTime;
            //handle.ValueDataType = DefaultType.DateTimeType;
            return handle;
        }

        /// <summary>
        /// 字符串按指定格式转换到时间类型
        /// </summary>
        /// <param name="dbType">转换到指定类型</param>
        /// <returns></returns>
        public CsqConvertFunction ConvertTo(DbType dbType)
        {
            CsqConvertFunction handle = new CsqConvertFunction(this, dbType, DBConvertFunction.ConvetTo);
            //handle.ValueDataType = DefaultType.DateTimeType;
            handle.ValueDbType = dbType;
            return handle;
        }
        /// <summary>
        /// 是否空值
        /// </summary>
        /// <returns></returns>
        internal virtual bool IsNullValue() 
        {
            return false;
        }

        protected DbType _valueDbType = DbType.Object;

        /// <summary>
        /// 对应的数据库类型
        /// </summary>
        internal DbType ValueDbType
        {
            get { return _valueDbType; }
            set { _valueDbType = value; }
        }
        internal abstract string DisplayValue(KeyWordInfomation info);

        internal abstract void FillInfo(KeyWordInfomation info);
        /// <summary>
        /// 格式化值类型的值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static ValueType FormatValueType(ValueType value)
        {
            if (value.GetType().IsEnum)
            {
                int ret = (int)value;
                return ret;
            }
            return value;
        }
        /// <summary>
        /// 把传进来的值转换成BQL能识别的值项
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BQLValueItem ToValueItem(object value) 
        {
            BQLQuery query = value as BQLQuery;
            if (!CommonMethods.IsNull(query)) 
            {
                return query.AS(null);
            }

            BQLValueItem item = value as BQLValueItem;
            if (CommonMethods.IsNull(item)) 
            {
                item = new BQLValueTypeItem(value);
            }
            return item;
        }
        public static implicit operator BQLValueItem(ValueType value)
        {

            return new BQLValueTypeItem(FormatValueType(value));
        }
        public static implicit operator BQLValueItem(byte[] value)
        {
            return new BQLValueTypeItem(value);
        }
        public static implicit operator BQLValueItem(string value)
        {
            return new BQLValueTypeItem(value);
        }
        
        /// <summary>
        /// 统一数据库值类型
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        protected static void UnityDbType(BQLValueItem value1, BQLValueItem value2)
        {
            if (CommonMethods.IsNull(value1)  || CommonMethods.IsNull(value2)) 
            {
                return;
            }
            DbType type = DbType.Object;
            if (value1.ValueDbType != DbType.Object) 
            {
                type = value1.ValueDbType;
            }
            if (value2.ValueDbType != DbType.Object)
            {
                type = value2.ValueDbType;
            }

            if (type != DbType.Object) 
            {
                if (value1.ValueDbType == DbType.Object) 
                {
                    value1.ValueDbType = type;
                }
                if (value2.ValueDbType == DbType.Object)
                {
                    value2.ValueDbType = type;
                }
            }
        }

        public static BQLOperatorHandle operator +(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLOperatorHandle fHandle = new BQLOperatorHandle(FunctionManager.DoAdd, new BQLValueItem[] { handle, oValue });
            return fHandle;
        }
        public static BQLOperatorHandle operator -(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLOperatorHandle fHandle = new BQLOperatorHandle(FunctionManager.DoSub, new BQLValueItem[] { handle, oValue });
            return fHandle;
        }
        public static BQLOperatorHandle operator *(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLOperatorHandle fHandle = new BQLOperatorHandle(FunctionManager.DoMul, new BQLValueItem[] { handle, oValue });
            return fHandle;
        }
        public static BQLOperatorHandle operator /(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLOperatorHandle fHandle = new BQLOperatorHandle(FunctionManager.DoDiv, new BQLValueItem[] { handle, oValue });
            return fHandle;
        }

        public static BQLComparItem operator ==(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoEqual, new BQLValueItem[] { handle, oValue });
            return fHandle;
        }
        public static BQLComparItem operator !=(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoNotequal, new BQLValueItem[] { handle, oValue });
            return fHandle;
        }
        public static BQLComparItem operator &(BQLValueItem handle, BQLValueItem value)
        {
            UnityDbType(handle, value);
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoAnd, new BQLValueItem[] { handle, value });
            return fHandle;
        }
        public static BQLComparItem operator |(BQLValueItem handle, BQLValueItem value)
        {
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoOr, new BQLValueItem[] { handle, value });
            return fHandle;
        }
        public static BQLComparItem operator >(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoMore, new BQLValueItem[] { handle, oValue });
            return fHandle;
        }
        public static BQLComparItem operator >=(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoMorethen, new BQLValueItem[] { handle, oValue });
            return fHandle;
        }
        public static BQLComparItem operator <(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoLess, new BQLValueItem[] { handle, oValue });
            return fHandle;
        }
        public static BQLComparItem operator <=(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoLessThen, new BQLValueItem[] { handle, oValue });
            return fHandle;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        
    }
}
