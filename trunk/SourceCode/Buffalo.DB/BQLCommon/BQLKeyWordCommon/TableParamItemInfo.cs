using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// 表字段信息
    /// </summary>
    public class TableParamItemInfo
    {
        /// <summary>
        /// 表字段信息
        /// </summary>
        /// <param name="paramName">字段名</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="allowNull">允许空</param>
        /// <param name="type">字段类型</param>
        public TableParamItemInfo(string paramName, DbType dbType,
            bool allowNull, EntityPropertyType type, int length) 
        {
            _paramName = paramName;
            _dbtype = dbType;
            _allowNull = allowNull;
            _type = type;
            _length = length;

        }

        
        private int _length;

        /// <summary>
        /// 长度
        /// </summary>
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        private string _paramName;
        /// <summary>
        /// 字段名
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
            set { _paramName = value; }
        }

        private DbType _dbtype;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbType DBType
        {
            get { return _dbtype; }
            set { _dbtype = value; }
        }

        private bool _allowNull;

        /// <summary>
        /// 允许空
        /// </summary>
        public bool AllowNull
        {
            get { return _allowNull; }
            set { _allowNull = value; }
        }

        private EntityPropertyType _type;

        /// <summary>
        /// 字段类型
        /// </summary>
        public EntityPropertyType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string DisplayInfo(KeyWordInfomation info, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            sb.Append(idba.FormatTableName(ParamName) + " ");
            sb.Append(idba.DBTypeToSQL(DBType, Length) + " ");

            bool isPrimary = EnumUnit.ContainerValue((int)_type,(int)EntityPropertyType.PrimaryKey);
            bool isAutoIdentity = EnumUnit.ContainerValue((int)_type, (int)EntityPropertyType.Identity);

            if (isAutoIdentity) 
            {
                sb.Append(idba.DBIdentity(tableName, _paramName));
            }

            if (isPrimary)
            {
                sb.Append(" primary key ");
            }
            else 
            {
                if (AllowNull)
                {
                    sb.Append("NULL");
                }
                else
                {
                    sb.Append("NOT NULL");
                }
            }
            return sb.ToString();
        }

    }
}
