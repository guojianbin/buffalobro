using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using System.Collections.Generic;
using Buffalo.DB.QueryConditions;
using TestAddIn.BQLEntity;
using TestAddIn;
using Buffalo.DB.DbCommon;
namespace TestAddIn.DataAccess.Bql
{
    ///<summary>
    /// Êý¾Ý·ÃÎÊ²ã
    ///</summary>
    public class ManMessageDataAccess :BQLDataAccessBase<ManMessage>
    {
        public ManMessageDataAccess(DataBaseOperate oper)
            : base(oper)
        {
            
        }
        public ManMessageDataAccess()
        {
            
        }
    }
}



