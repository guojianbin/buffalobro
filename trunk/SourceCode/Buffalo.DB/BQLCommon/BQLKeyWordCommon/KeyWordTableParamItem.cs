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
    /// 建表的字段集合
    /// </summary>
    public class KeyWordTableParamItem : BQLQuery
    {
        protected List<EntityParam> _tparams;
        /// <summary>
        /// 字段
        /// </summary>
        public List<EntityParam> Params
        {
            get { return _tparams; }
            set { _tparams = value; }
        }
        protected string _tableName;
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
        }
        private List<TableRelationAttribute> _relationItems;

        /// <summary>
        /// 关系集合
        /// </summary>
        public List<TableRelationAttribute> RelationItems
        {
            get { return _relationItems; }
            set { _relationItems = value; }
        }


        /// <summary>
        /// Insert的字段关键字项
        /// </summary>
        /// <param name="paramHandles">字段集合</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordTableParamItem(string tableName,BQLQuery previous)
            : base(previous) 
        {
            _tableName = tableName;
            this._tparams = new List<EntityParam>();
        }
        /// <summary>
        /// 表信息
        /// </summary>
        /// <param name="paramHandles">字段集合</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordTableParamItem(List<EntityParam> lstParams,List<TableRelationAttribute> relationItems, string tableName, BQLQuery previous)
            : base(previous)
        {
            _tableName = tableName;
            this._relationItems = relationItems;
            this._tparams = lstParams;
        }


        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="paramName">字段名</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="allowNull">允许空</param>
        /// <param name="type">类型</param>
        /// <param name="length">长度</param>
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
        /// 添加一个set项
        /// </summary>
        /// <param name="parameter">字段</param>
        /// <param name="valueItem">值</param>
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
