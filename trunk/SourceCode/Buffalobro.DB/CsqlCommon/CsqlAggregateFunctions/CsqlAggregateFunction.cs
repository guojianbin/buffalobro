using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.DataBaseAdapter.IDbAdapters;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.Kernel.Defaults;
using System.Data;
using Buffalo.Kernel;

namespace Buffalobro.DB.CsqlCommon.CsqlAggregateFunctions
{
    public delegate string DelAggregateFunctionHandle(string paramName,DBInfo info);
    public class CsqlAggregateFunction : CsqlParamHandle
    {
        private DelAggregateFunctionHandle functionHandle;
        private CsqlParamHandle param;
        /// <summary>
        /// �ۺϺ���
        /// </summary>
        /// <param name="functionName">������</param>
        /// <param name="param"></param>
        public CsqlAggregateFunction(DelAggregateFunctionHandle functionHandle, CsqlParamHandle param)

        {
            this.functionHandle = functionHandle;
            this.param = param;
        }
        internal override void FillInfo(KeyWordInfomation info)
        {
            
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            DelAggregateFunctionHandle handle = functionHandle;
            if (handle != null) 
            {
                string strParam = null;
                if (!CommonMethods.IsNull(param))
                {
                    strParam = param.DisplayValue(info);
                }
                else 
                {
                    strParam = "*";
                }
                if (this.ValueDbType == DbType.Object) //����˺���Ϊδ֪��������ʱ����Զ�תΪ�ֶ�����
                {
                    _valueDbType = param.ValueDbType;
                }
                return handle(strParam,info.DBInfo);
            }
            return null;
        }

        
    }
}
