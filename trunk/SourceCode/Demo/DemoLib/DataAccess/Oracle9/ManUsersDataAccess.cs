using System;
using System.Data;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using TestAddIn;
using System.Collections.Generic;
using TestAddIn.DataAccess.IDataAccess;

namespace TestAddIn.DataAccess.Oracle9
{
    ///<summary>
    /// Êý¾Ý·ÃÎÊ²ã
    ///</summary>
    [IDalAttribute(typeof(IManUsersDataAccess))]
    public class ManUsersDataAccess : DataAccessModel<ManUsers>,IManUsersDataAccess
    {
        public ManUsersDataAccess(DataBaseOperate oper): base(oper)
        {
            
        }
        public ManUsersDataAccess(): base()
        {
        }
    }
}



