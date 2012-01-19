using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.PropertyAttributes;
using System.Data;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordAlterTableItem : BQLQuery
    {
        private string _tableName;

        /// <summary>
        /// ����
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
        }
        /// <summary>
        /// Insert�ؼ�����
        /// </summary>
        /// <param name="tableHandle">Ҫ����ı�</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordAlterTableItem(string tableName, BQLQuery previous)
            : base(previous) 
        {
            this._tableName = tableName;
        }

        /// <summary>
        /// �ֶ�
        /// </summary>
        /// <param name="paramName">�ֶ���</param>
        /// <param name="dbType">���ݿ�����</param>
        /// <param name="allowNull">�����</param>
        /// <param name="type">����</param>
        /// <param name="length">����</param>
        /// <returns></returns>
        public KeyWordAddParamItem AddParam(string paramName, 
            DbType dbType, bool allowNull, EntityPropertyType type,int length)
        {
            TableParamItemInfo info = new TableParamItemInfo(paramName,
                dbType, allowNull, type, length);
            KeyWordAddParamItem item = new KeyWordAddParamItem(info,_tableName, this);
            return item;
        }
        /// <summary>
        /// �ֶ�
        /// </summary>
        /// <param name="paramName">�ֶ���</param>
        /// <param name="dbType">���ݿ�����</param>
        /// <param name="allowNull">�����</param>
        /// <param name="type">����</param>
        /// <param name="length">����</param>
        /// <returns></returns>
        public KeyWordAddParamItem AddParam(TableParamItemInfo info)
        {
            KeyWordAddParamItem item = new KeyWordAddParamItem(info,_tableName, this);
            return item;
        }
        
        internal override void LoadInfo(KeyWordInfomation info)
        {
            
        }
        internal override void Tran(KeyWordInfomation info)
        {
            if (info.Condition == null)
            {
                info.Condition = new Buffalo.DB.QueryConditions.AlterTableCondition(info.DBInfo);
            }
            if (info.ParamList == null)
            {
                info.ParamList = new Buffalo.DB.DbCommon.ParamList();
            }
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            info.Condition.Tables.Append(idba.FormatTableName(_tableName));
            
        }
    }
}
