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
        protected List<EntityParam> _tparams;
        /// <summary>
        /// �ֶ�
        /// </summary>
        public List<EntityParam> Params
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
        private List<TableRelationAttribute> _relationItems;

        /// <summary>
        /// ��ϵ����
        /// </summary>
        public List<TableRelationAttribute> RelationItems
        {
            get { return _relationItems; }
            set { _relationItems = value; }
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
            this._tparams = new List<EntityParam>();
        }
        /// <summary>
        /// ����Ϣ
        /// </summary>
        /// <param name="paramHandles">�ֶμ���</param>
        /// <param name="previous">��һ���ؼ���</param>
        public KeyWordTableParamItem(List<EntityParam> lstParams,List<TableRelationAttribute> relationItems, string tableName, BQLQuery previous)
            : base(previous)
        {
            _tableName = tableName;
            this._relationItems = relationItems;
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
            EntityParam info = new EntityParam("",paramName,"",
                dbType, type, length, false);
            _tparams.Add(info);
            return this;
        }

        /// <summary>
        /// ���һ��set��
        /// </summary>
        /// <param name="parameter">�ֶ�</param>
        /// <param name="valueItem">ֵ</param>
        /// <returns></returns>
        public KeyWordTableParamItem _(List<EntityParam> lstParam)
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
                EntityParam item = _tparams[i];
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
