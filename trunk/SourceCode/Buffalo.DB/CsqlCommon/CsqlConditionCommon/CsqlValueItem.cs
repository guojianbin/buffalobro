using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.Kernel.Defaults;
using Buffalo.Kernel;
using Buffalo.DB.CsqlCommon.CsqlExtendFunction;
using Buffalo.DB.DBFunction;
using Buffalo.DB.CsqlCommon.CsqlConditions;

namespace Buffalo.DB.CsqlCommon.CsqlConditionCommon
{
    
    public abstract class CsqlValueItem
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
        public CsqlAliasParamHandle As(string asName)
        {
            CsqlAliasParamHandle item = new CsqlAliasParamHandle(this, asName);
            return item;
        }
        /// <summary>
        /// StarWith条件
        /// </summary>
        /// <param name="item">条件</param>
        /// <returns></returns>
        public CsqlConditionItem StarWith(object item)
        {
            CsqlValueItem oValue = CsqlValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new CsqlConditionItem(this, new CsqlValueItem[] { oValue }, CsqlConditionManager.DoStarWith);
        }

        /// <summary>
        /// EndWith条件
        /// </summary>
        /// <param name="item">条件</param>
        /// <returns></returns>
        public CsqlConditionItem EndWith(object item)
        {

            CsqlValueItem oValue = CsqlValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new CsqlConditionItem(this, new CsqlValueItem[] { oValue }, CsqlConditionManager.DoEndWith);
        }
        /// <summary>
        /// Between条件
        /// </summary>
        /// <param name="star">开始值</param>
        /// <param name="end">结束值</param>
        /// <returns></returns>
        public CsqlConditionItem Between(object star, object end)
        {
            CsqlValueItem oValue1 = CsqlValueItem.ToValueItem(star);
            CsqlValueItem oValue2 = CsqlValueItem.ToValueItem(end);
            oValue1.ValueDbType = this.ValueDbType;
            oValue2.ValueDbType = this.ValueDbType;
            return new CsqlConditionItem(this, new CsqlValueItem[] { oValue1, oValue2 }, CsqlConditionManager.DoBetween);
        }

        /// <summary>
        /// 全文检索的条件
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public CsqlConditionItem Contains(object item)
        {
            CsqlValueItem oValue = CsqlValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new CsqlConditionItem(this, new CsqlValueItem[] { oValue }, CsqlConditionManager.DoContains);

        }

        /// <summary>
        /// 查找字符
        /// </summary>
        /// <param name="value">要查找的字符</param>
        /// <param name="start">起始位置</param>
        /// <returns></returns>
        public CsqCommonFunction IndexOf(object value, CsqlValueItem start)
        {
            CsqlValueItem oValue = CsqlValueItem.ToValueItem(value); ;

            start.ValueDbType = DbType.Int32;
            if (oValue.ValueDbType == DbType.Object)
            {
                oValue = DbType.String;
            }
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { oValue, this, start }, DBMathFunction.IndexOf, DbType.Int32);
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
        public CsqCommonFunction SubString(CsqlValueItem start, CsqlValueItem length)
        {
            start.ValueDbType = DbType.Int32;
            length.ValueDbType = DbType.Int32;
            CsqCommonFunction handle = new CsqCommonFunction(new CsqlValueItem[] { this, start, length }, DBMathFunction.SubString, DbType.Int32);
            //handle.ValueDataType = DefaultType.IntType;
            handle.ValueDbType = DbType.Int32;
            return handle;
        }
        /// <summary>
        /// Like条件
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public CsqlConditionItem Like(object item)
        {

            CsqlValueItem oValue = CsqlValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new CsqlConditionItem(this, new CsqlValueItem[] { oValue }, CsqlConditionManager.DoLike);
        }
        /// <summary>
        /// FreeText条件
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public CsqlConditionItem FreeText(object item)
        {
            CsqlValueItem oValue = CsqlValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new CsqlConditionItem(this, new CsqlValueItem[] { oValue }, CsqlConditionManager.DoLike);
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

        public static CsqlValueItem ToValueItem(object value) 
        {
            CsqlValueItem item = value as CsqlValueItem;
            if (CommonMethods.IsNull(item)) 
            {
                item = new CsqlValueTypeItem(value);
            }
            return item;
        }
        public static implicit operator CsqlValueItem(ValueType value)
        {

            return new CsqlValueTypeItem(FormatValueType(value));
        }
        public static implicit operator CsqlValueItem(byte[] value)
        {
            return new CsqlValueTypeItem(value);
        }
        public static implicit operator CsqlValueItem(string value)
        {
            return new CsqlValueTypeItem(value);
        }
        
        /// <summary>
        /// 统一数据库值类型
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        protected static void UnityDbType(CsqlValueItem value1, CsqlValueItem value2)
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

        public static CsqlOperatorHandle operator +(CsqlValueItem handle, object value)
        {
            CsqlValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            CsqlOperatorHandle fHandle = new CsqlOperatorHandle(FunctionManager.DoAdd, new CsqlValueItem[] { handle, oValue });
            return fHandle;
        }
        public static CsqlOperatorHandle operator -(CsqlValueItem handle, object value)
        {
            CsqlValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            CsqlOperatorHandle fHandle = new CsqlOperatorHandle(FunctionManager.DoSub, new CsqlValueItem[] { handle, oValue });
            return fHandle;
        }
        public static CsqlOperatorHandle operator *(CsqlValueItem handle, object value)
        {
            CsqlValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            CsqlOperatorHandle fHandle = new CsqlOperatorHandle(FunctionManager.DoMul, new CsqlValueItem[] { handle, oValue });
            return fHandle;
        }
        public static CsqlOperatorHandle operator /(CsqlValueItem handle, object value)
        {
            CsqlValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            CsqlOperatorHandle fHandle = new CsqlOperatorHandle(FunctionManager.DoDiv, new CsqlValueItem[] { handle, oValue });
            return fHandle;
        }

        public static CsqlComparItem operator ==(CsqlValueItem handle, object value)
        {
            CsqlValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            CsqlComparItem fHandle = new CsqlComparItem(FunctionManager.DoEqual, new CsqlValueItem[] { handle, oValue });
            return fHandle;
        }
        public static CsqlComparItem operator !=(CsqlValueItem handle, object value)
        {
            CsqlValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            CsqlComparItem fHandle = new CsqlComparItem(FunctionManager.DoNotequal, new CsqlValueItem[] { handle, oValue });
            return fHandle;
        }
        public static CsqlComparItem operator &(CsqlValueItem handle, CsqlValueItem value)
        {
            UnityDbType(handle, value);
            CsqlComparItem fHandle = new CsqlComparItem(FunctionManager.DoAnd, new CsqlValueItem[] { handle, value });
            return fHandle;
        }
        public static CsqlComparItem operator |(CsqlValueItem handle, CsqlValueItem value)
        {
            CsqlComparItem fHandle = new CsqlComparItem(FunctionManager.DoOr, new CsqlValueItem[] { handle, value });
            return fHandle;
        }
        public static CsqlComparItem operator >(CsqlValueItem handle, object value)
        {
            CsqlValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            CsqlComparItem fHandle = new CsqlComparItem(FunctionManager.DoMore, new CsqlValueItem[] { handle, oValue });
            return fHandle;
        }
        public static CsqlComparItem operator >=(CsqlValueItem handle, object value)
        {
            CsqlValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            CsqlComparItem fHandle = new CsqlComparItem(FunctionManager.DoMorethen, new CsqlValueItem[] { handle, oValue });
            return fHandle;
        }
        public static CsqlComparItem operator <(CsqlValueItem handle, object value)
        {
            CsqlValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            CsqlComparItem fHandle = new CsqlComparItem(FunctionManager.DoLess, new CsqlValueItem[] { handle, oValue });
            return fHandle;
        }
        public static CsqlComparItem operator <=(CsqlValueItem handle, object value)
        {
            CsqlValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            CsqlComparItem fHandle = new CsqlComparItem(FunctionManager.DoLessThen, new CsqlValueItem[] { handle, oValue });
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
