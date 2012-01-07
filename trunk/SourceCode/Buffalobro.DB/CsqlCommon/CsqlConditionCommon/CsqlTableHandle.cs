using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.EntityInfos;
using Buffalobro.DB.DataBaseAdapter.IDbAdapters;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon;
using System.Data;
using Buffalo.Kernel;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    public abstract class CsqlTableHandle : CsqlValueItem
    {
        
        /// <summary>
        /// ��ȡ��Ӧ��ʵ������
        /// </summary>
        /// <returns></returns>
        internal abstract List<ParamInfo> GetParamInfoHandle();
        /// <summary>
        /// ���������һ������
        /// </summary>
        /// <param name="asName">����</param>
        /// <returns></returns>
        public CsqlAliasHandle AS(string asName)
        {
            CsqlAliasHandle item = new CsqlAliasHandle(this, asName);
            return item;
        }

        /// <summary>
        /// ��ȡ�ֶ�
        /// </summary>
        /// <param name="paramName">�ֶ���</param>
        /// <returns></returns>
        public virtual CsqlParamHandle this[string paramName]
        {
            get
            {
                return new CsqlOtherParamHandle(this, paramName);
            }
        }

        /// <summary>
        /// ��ȡ�ֶ�
        /// </summary>
        /// <param name="paramName">�ֶ���</param>
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
        /// ��ȡ�����ֶ�
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
