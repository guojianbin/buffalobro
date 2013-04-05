using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using System.Collections.Generic;
using Buffalo.DB.QueryConditions;
using TestPerLib.BQLEntity;
using TestPerLib;
using Buffalo.DB.DbCommon;
namespace TestPerLib.DataAccess.Bql
{
    ///<summary>
    /// Êý¾Ý·ÃÎÊ²ã
    ///</summary>
    public class ScStudentDataAccess :BQLDataAccessBase<ScStudent>
    {
        public ScStudentDataAccess(DataBaseOperate oper)
            : base(oper)
        {
            
        }
        public ScStudentDataAccess()
        {
            
        }
    }
}



