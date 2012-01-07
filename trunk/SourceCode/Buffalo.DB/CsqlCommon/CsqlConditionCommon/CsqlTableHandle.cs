using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CsqlCommon.CsqlKeyWordCommon;
using System.Data;
using Buffalo.Kernel;

namespace Buffalo.DB.CsqlCommon.CsqlConditionCommon
{
    public abstract class CsqlTableHandle : CsqlValueItem
    {
        
        /// <summary>
        /// 获取对应的实体属性
        /// </summary>
        /// <returns></returns>
        internal abstract List<ParamInfo> GetParamInfoHandle();
        /// <summary>
        /// 给这个表定义一个别名
        /// </summary>
        /// <param name="asName">别名</param>
        /// <returns></returns>
        public CsqlAliasHandle AS(string asName)
        {
            CsqlAliasHandle item = new CsqlAliasHandle(this, asName);
            return item;
        }

        /// <summary>
        /// 获取字段
        /// </summary>
        /// <param name="paramName">字段名</param>
        /// <returns></returns>
        public virtual CsqlParamHandle this[string paramName]
        {
            get
            {
                return new CsqlOtherParamHandle(this, paramName);
            }
        }

        /// <summary>
        /// 获取字段
        /// </summary>
        /// <param name="paramName">字段名</param>
        /// <returns></returns>
        public virtual CsqlParamHandle this[string paramName,DbType dbType]
        {
            get
            {
                CsqlOtherParamHandle prm = new CsqlOtherParamHandle(this, paramName);
                prm.ValueDbType = dbType;
                return prm;
            }
        }
        protected CsqlParamHandle __ = null;
        /// <summary>
        /// 获取所有字段
        /// </summary>
        /// <returns></returns>
        public virtual CsqlParamHandle _
        {
            get
            {
                if (CommonMethods.IsNull(__))
                {
                    __ = new CsqlOtherParamHandle(this, "*");
                }
                return __;
            }
        }
    }
}
