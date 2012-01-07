using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.CsqlCommon.CsqlConditions;
using Buffalo.DB.CsqlCommon.CsqlKeyWordCommon;
using Buffalo.DB.CsqlCommon.CsqlExtendFunction;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel.Defaults;
using System.Data;
using System.Collections;
using Buffalo.DB.DBFunction;

namespace Buffalo.DB.CsqlCommon.CsqlConditionCommon
{
    public abstract class CsqlParamHandle:CsqlValueItem
    {

        //private IMathFunctions maths = null;
        //private IConvertFunction convert=null;
        public CsqlParamHandle() 
        {
            //IMathFunctions maths = db.Math;
            //IConvertFunction convert = db.Convert;
        }
        

        

        /// <summary>
        /// ��������
        /// </summary>
        public CsqlOrderByHandle DESC 
        {
            get 
            {
                return new CsqlOrderByHandle(this, SortType.DESC);
            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public CsqlOrderByHandle ASC
        {
            get
            {
                return new CsqlOrderByHandle(this, SortType.ASC);
            }
        }

        //public CsqlConditionItem 
        /// <summary>
        /// In����
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public CsqlConditionItem In(params ValueType[] values) 
        {
            return new CsqlConditionItem(this, values, CsqlConditionManager.DoIn);
        }
        /// <summary>
        /// In����
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public CsqlConditionItem In(params string[] values)
        {
            return new CsqlConditionItem(this, values, CsqlConditionManager.DoIn);
        }
        /// <summary>
        /// In����
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public CsqlConditionItem In(IEnumerable collection)
        {
            return new CsqlConditionItem(this, collection, CsqlConditionManager.DoIn);
        }
        /// <summary>
        /// In����
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public CsqlConditionItem In(CsqlQuery item)
        {
            return new CsqlConditionItem(this, item, CsqlConditionManager.DoIn);
        }
        /// <summary>
        /// NotIn����
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public CsqlConditionItem NotIn(params ValueType[] values)
        {
            return new CsqlConditionItem(this, values, CsqlConditionManager.DoNotIn);
        }
        /// <summary>
        /// NotIn����
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public CsqlConditionItem NotIn(params string[] values)
        {
            return new CsqlConditionItem(this, values, CsqlConditionManager.DoNotIn);
        }
        /// <summary>
        /// NotIn����
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public CsqlConditionItem NotIn(IEnumerable collection)
        {
            return new CsqlConditionItem(this, collection, CsqlConditionManager.DoNotIn);
        }
        /// <summary>
        /// NotIn����
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public CsqlConditionItem NotIn(CsqlQuery item)
        {
            return new CsqlConditionItem(this, item, CsqlConditionManager.DoNotIn);
        }

        
        
    }
}
