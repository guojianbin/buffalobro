using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalo.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordUpdateItem : CsqlQuery
    {
        private CsqlTableHandle table;
        
        /// <summary>
        /// Update�ؼ�����
        /// </summary>
        /// <param name="table">��</param>
        /// <param name="previous">��һ���ؼ���</param>
        public KeyWordUpdateItem(CsqlTableHandle table, CsqlQuery previous)
            : base(previous) 
        {
            this.table = table;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            table.FillInfo(info);
        }
        /// <summary>
        /// Ҫ��ѯ���ֶ�
        /// </summary>
        internal CsqlTableHandle Table
        {
            get 
            {
                return table;
            }
        }
        /// <summary>
        /// ���һ��set��
        /// </summary>
        /// <param name="parameter">�ֶ�</param>
        /// <param name="valueItem">ֵ</param>
        /// <returns></returns>
        public KeyWordUpdateSetItem Set(CsqlParamHandle parameter, CsqlValueItem valueItem)
        {
            KeyWordUpdateSetItem item = new KeyWordUpdateSetItem(parameter, valueItem, this);
            return item;
        }
        /// <summary>
        /// ���һ��set��
        /// </summary>
        /// <param name="parameter">�ֶ�</param>
        /// <param name="valueItem">ֵ</param>
        /// <returns></returns>
        public KeyWordUpdateSetItem Set(UpdateSetParamItemList lstSetItem)
        {
            KeyWordUpdateSetItem item = new KeyWordUpdateSetItem(lstSetItem, this);
            return item;
        }
        internal override void Tran(KeyWordInfomation info)
        {
            if (info.Condition == null)
            {
                info.Condition = new Buffalo.DB.QueryConditions.UpdateCondition(info.DBInfo);
                //info.ParamList = newBuffalo.DB.DbCommon.ParamList();
            }
            if (info.ParamList == null)
            {
                info.ParamList = new Buffalo.DB.DbCommon.ParamList();
            }
            info.Condition.Tables.Append(table.DisplayValue(info));
            //return "update " + table.DisplayValue(info);
        }
    }
}
