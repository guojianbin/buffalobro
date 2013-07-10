using System;
using TestPerLib;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.CommBase;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
namespace TestPerLib.Business
{
    /// <summary>
    ///  ҵ���
    /// </summary>
    public class ScBaseBusinessBase<T>: BusinessModelBase<T> where T:ScBase,new()
    {
        //�����ʵ����Ҫ���̳����ڴ�д��ҵ�񷽷����������ҵ������ʹ��

        public override object Insert(T entity, ValueSetList setList, bool fillIdentity)
        {
            if (setList == null) 
            {
                setList = new ValueSetList();
            }
            setList.Add("LastDate", BQL.NowDate());
            setList.Add("CreateDate", BQL.NowDate());
            entity.State = 1;
            return base.Insert(entity, setList, fillIdentity);
        }

        public override object Update(T entity, ScopeList scorpList, ValueSetList lstValue, bool optimisticConcurrency)
        {
            if (lstValue == null) 
            {
                lstValue = new ValueSetList();
            }
            lstValue.Add("LastDate", BQL.NowDate());
            return base.Update(entity, scorpList, lstValue, optimisticConcurrency);
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public object Save(T entity) 
        {
            if(entity.Id>0)
            {
                return Update(entity);
            }
            return Insert(entity, true);
        }
    }
}



