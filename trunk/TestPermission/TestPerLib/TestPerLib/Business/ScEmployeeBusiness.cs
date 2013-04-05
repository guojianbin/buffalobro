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
    ///  登陆员工业务层
    /// </summary>
    public class ScEmployeeBusinessBase<T>: TestPerLib.Business.ScBaseBusinessBase<T> where T:ScEmployee,new()
    {
        //如果此实体需要被继承则在此写的业务方法能在子类的业务类中使用
    }
    
    /// <summary>
    ///  登陆员工业务层
    /// </summary>
    public class ScEmployeeBusiness: ScEmployeeBusinessBase<ScEmployee>
    {
        public ScEmployeeBusiness()
        {
            
        }
        
    }
}



