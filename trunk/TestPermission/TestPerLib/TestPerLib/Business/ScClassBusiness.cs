using System;
using TestPerLib;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.CommBase;
namespace TestPerLib.Business
{
    /// <summary>
    ///  ҵ���
    /// </summary>
    public class ScClassBusinessBase<T>: TestPerLib.Business.ScBaseBusinessBase<T> where T:ScClass,new()
    {
        //�����ʵ����Ҫ���̳����ڴ�д��ҵ�񷽷����������ҵ������ʹ��
    }
    
    /// <summary>
    ///  ҵ���
    /// </summary>
    public class ScClassBusiness: ScClassBusinessBase<ScClass>
    {
        public ScClassBusiness()
        {
            
        }
        
    }
}



