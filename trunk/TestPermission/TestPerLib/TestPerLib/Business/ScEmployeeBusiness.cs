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
    ///  ��½Ա��ҵ���
    /// </summary>
    public class ScEmployeeBusinessBase<T>: TestPerLib.Business.ScBaseBusinessBase<T> where T:ScEmployee,new()
    {
        //�����ʵ����Ҫ���̳����ڴ�д��ҵ�񷽷����������ҵ������ʹ��
    }
    
    /// <summary>
    ///  ��½Ա��ҵ���
    /// </summary>
    public class ScEmployeeBusiness: ScEmployeeBusinessBase<ScEmployee>
    {
        public ScEmployeeBusiness()
        {
            
        }
        
    }
}



