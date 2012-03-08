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
using Buffalo.DB.BQLCommon.BQLBaseFunction;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// 外键
    /// </summary>
    public class KeyWordAddForeignkeyItem : BQLQuery
    {
        private TableRelationAttribute _item;
        /// <summary>
        /// 外键值
        /// </summary>
        public TableRelationAttribute Item
        {
            get { return _item; }
        }
        /// <summary>
        /// 外键
        /// </summary>
        /// <param name="parentTable">主表</param>
        /// <param name="childTable">子表</param>
        /// <param name="parentParam">主表属性</param>
        /// <param name="childParam">子表属性</param>
        /// <param name="previous">上一个</param>
        public KeyWordAddForeignkeyItem(string name, string parentTable, string childTable,
            string parentParam, string childParam,
            BQLQuery previous)
            : base(previous)
        {
            _item = new TableRelationAttribute("",name,childTable,parentTable,childParam,parentParam,"",false);
        }



        /// <summary>
        /// Insert的字段关键字项
        /// </summary>
        /// <param name="paramHandles">字段集合</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordAddForeignkeyItem(TableRelationAttribute info, BQLQuery previous)
            : base(previous)
        {
            _item = info;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {

        }

        internal override void Tran(KeyWordInfomation info)
        {
            AlterTableCondition con = info.Condition as AlterTableCondition;
            if (con == null) 
            {
                return;
            }

            IDBAdapter ida = info.DBInfo.CurrentDbAdapter;

            con.Operator.Append("add constraint ");
            con.SqlParams.Append(_item.Name);
            con.SqlParams.Append(" foreign key (" + ida.FormatParam(_item.SourceName) + ") ");
            con.SqlParams.Append("references ");
            con.SqlParams.Append(ida.FormatTableName(_item.TargetTable) + "(" + ida.FormatParam(_item.TargetName)+")");
        }
    }

    
}
