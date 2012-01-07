using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalobro.Kernel.Defaults;
using Buffalobro.DB.CommBase.DataAccessBases;
using Buffalo.Kernel;
using Buffalobro.DB.DataBaseAdapter;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    /// <summary>
    /// 值类型项
    /// </summary>
    public class CsqlValueTypeItem:CsqlValueItem
    {
        private object itemValue;

        public object ItemValue
        {
            get { return itemValue; }
            set { itemValue = value; }
        }

        internal override bool IsNullValue()
        {
            return itemValue==null;
        }

        public CsqlValueTypeItem(object itemValue) 
        {
            if (itemValue != null)
            {
                this.itemValue = itemValue;
                this._valueDbType=DefaultType.ToDbType(itemValue.GetType());
            }
            else 
            {
                this.itemValue = itemValue;
                //this.valueDataType = null;
            }
        }
        
        ///// <summary>
        ///// 此项的值
        ///// </summary>
        //public object ItemValue 
        //{
        //    get 
        //    {
        //        return itemValue;
        //    }
        //}

        internal override void FillInfo(KeyWordInfomation info)
        {
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            //string ret = null;
            if (info.ParamList != null && _valueDbType != DbType.Object) 
            {
                string pName="P"+info.ParamList.Count;
                string pKeyName = info.DBInfo.CurrentDbAdapter.FormatParamKeyName(pName);
                string pValueName = info.DBInfo.CurrentDbAdapter.FormatValueName(pName);
                
                info.ParamList.AddNew(pKeyName, _valueDbType, itemValue);
                
                return pValueName;
            }

            return FormatValueType(info);
        }

        


        /// <summary>
        /// 格式化值项
        /// </summary>
        /// <param name="valueItem"></param>
        /// <returns></returns>
        private string FormatValueType(KeyWordInfomation info)
        {
            Type valueDataType = null;
            if (itemValue != null) 
            {
                valueDataType = itemValue.GetType();
            }
            if (DefaultType.EqualType(valueDataType , DefaultType.StringType) || DefaultType.EqualType(valueDataType , DefaultType.GUIDType))
            {
                return DataAccessCommon.FormatValue(itemValue, DbType.String, info.DBInfo);
            }
            else if (DefaultType.EqualType(valueDataType , DefaultType.DateTimeType))
            {
                return DataAccessCommon.FormatValue(itemValue, DbType.DateTime, info.DBInfo);
            }
            else if (DefaultType.EqualType(valueDataType , DefaultType.BytesType))
            {
                return DataAccessCommon.FormatValue(itemValue, DbType.Binary, info.DBInfo);
            }

            else if (DefaultType.EqualType(valueDataType , DefaultType.BooleanType) )
            {
                return DataAccessCommon.FormatValue(itemValue, DbType.Boolean, info.DBInfo);
            }
            
            else if (valueDataType == null) 
            {
                return "null";
            }
            return itemValue.ToString();
        }
    }
}
