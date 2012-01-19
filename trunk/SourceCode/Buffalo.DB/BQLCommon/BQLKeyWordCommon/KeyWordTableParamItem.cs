using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.BQLCommon.IdentityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.PropertyAttributes;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// ������ֶμ���
    /// </summary>
    public class KeyWordTableParamItem : BQLQuery
    {
        protected List<TableParamItemInfo> _tparams;
        /// <summary>
        /// �ֶ�
        /// </summary>
        public List<TableParamItemInfo> Params
        {
            get { return _tparams; }
            set { _tparams = value; }
        }
        protected string _tableName;
        /// <summary>
        /// ����
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
        }



        /// <summary>
        /// Insert���ֶιؼ�����
        /// </summary>
        /// <param name="paramHandles">�ֶμ���</param>
        /// <param name="previous">��һ���ؼ���</param>
        public KeyWordTableParamItem(string tableName,BQLQuery previous)
            : base(previous) 
        {
            _tableName = tableName;
            this._tparams = new List<TableParamItemInfo>();
        }
        /// <summary>
        /// Insert���ֶιؼ�����
        /// </summary>
        /// <param name="paramHandles">�ֶμ���</param>
        /// <param name="previous">��һ���ؼ���</param>
        public KeyWordTableParamItem(List<TableParamItemInfo> lstParams, string tableName, BQLQuery previous)
            : base(previous)
        {
            _tableName = tableName;
            this._tparams = lstParams;
        }


        /// <summary>
        /// ����ֶ�
        /// </summary>
        /// <param name="paramName">�ֶ���</param>
        /// <param name="dbType">���ݿ�����</param>
        /// <param name="allowNull">�����</param>
        /// <param name="type">����</param>
        /// <param name="length">����</param>
        /// <returns></returns>
        public KeyWordTableParamItem _(string paramName, DbType dbType, bool allowNull, 
            EntityPropertyType type,int length)
        {
            TableParamItemInfo info = new TableParamItemInfo(paramName,
                dbType, allowNull, type, length);
            _tparams.Add(info);
            return this;
        }

        /// <summary>
        /// ���һ��set��
        /// </summary>
        /// <param name="parameter">�ֶ�</param>
        /// <param name="valueItem">ֵ</param>
        /// <returns></returns>
        public KeyWordTableParamItem _(List<TableParamItemInfo> lstParam)
        {
            _tparams.AddRange(lstParam);
            return this;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {

        }

        internal override void Tran(KeyWordInfomation info)
        {
            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < _tparams.Count; i++)
            {
                TableParamItemInfo item = _tparams[i];
                sb.Append(item.DisplayInfo(info,TableName));


                if (i < _tparams.Count - 1)
                {
                    sb.Append(",");
                }
            }

            

            info.Condition.SqlParams.Append(sb.ToString());
        }
    }

    
}
