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
        ///// ���ݿ��ֵ����
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
        /// ���ֶζ���һ������
        /// </summary>
        /// <param name="asName">����</param>
        /// <returns></returns>
        public CsqlAliasParamHandle As(string asName)
        {
            CsqlAliasParamHandle item = new CsqlAliasParamHandle(this, asName);
            return item;
        }
        /// <summary>
        /// StarWith����
        /// </summary>
        /// <param name="item">����</param>
        /// <returns></returns>
        public CsqlConditionItem StarWith(object item)
        {
            CsqlValueItem oValue = CsqlValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new CsqlConditionItem(this, new CsqlValueItem[] { oValue }, CsqlConditionManager.DoStarWith);
        }

        /// <summary>
        /// EndWith����
        /// </summary>
        /// <param name="item">����</param>
        /// <returns></returns>
        public CsqlConditionItem EndWith(object item)
        {

            CsqlValueItem oValue = CsqlValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new CsqlConditionItem(this, new CsqlValueItem[] { oValue }, CsqlConditionManager.DoEndWith);
        }
        /// <summary>
        /// Between����
        /// </summary>
        /// <param name="star">��ʼֵ</param>
        /// <param name="end">����ֵ</param>
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
        /// ȫ�ļ���������
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
        /// �����ַ�
        /// </summary>
        /// <param name="value">Ҫ���ҵ��ַ�</param>
        /// <param name="start">��ʼλ��</param>
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
        /// ��ȡ�ַ���
        /// </summary>
        /// <param name="start">�ַ�����ʼλ��</param>
        /// <param name="length">�ַ�������</param>
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
        /// Like����
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
        /// FreeText����
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
        /// ʱ�����Ͱ�ָ����ʽת�����ַ���
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
        /// �ַ�����ָ����ʽת����ʱ������
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
        /// �ַ�����ָ����ʽת����ʱ������
        /// </summary>
        /// <param name="dbType">ת����ָ������</param>
        /// <returns></returns>
        public CsqConvertFunction ConvertTo(DbType dbType)
        {
            CsqConvertFunction handle = new CsqConvertFunction(this, dbType, DBConvertFunction.ConvetTo);
            //handle.ValueDataType = DefaultType.DateTimeType;
            handle.ValueDbType = dbType;
            return handle;
        }
        /// <summary>
        /// �Ƿ��ֵ
        /// </summary>
        /// <returns></returns>
        internal virtual bool IsNullValue() 
        {
            return false;
        }

        protected DbType _valueDbType = DbType.Object;

        /// <summary>
        /// ��Ӧ�����ݿ�����
        /// </summary>
        internal DbType ValueDbType
        {
            get { return _valueDbType; }
            set { _valueDbType = value; }
        }
        internal abstract string DisplayValue(KeyWordInfomation info);

        internal abstract void FillInfo(KeyWordInfomation info);
        /// <summary>
        /// ��ʽ��ֵ���͵�ֵ
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
        /// ͳһ���ݿ�ֵ����
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
