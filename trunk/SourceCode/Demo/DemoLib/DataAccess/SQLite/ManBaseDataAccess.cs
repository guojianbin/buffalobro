using System;
using System.Data;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using TestAddIn;
using System.Collections.Generic;
using TestAddIn.DataAccess.IDataAccess;

namespace TestAddIn.DataAccess.SQLite
{
    ///<summary>
    /// Êý¾Ý·ÃÎÊ²ã
    ///</summary>
    [IDalAttribute(typeof(IManBaseDataAccess))]
    public class ManBaseDataAccess : DataAccessModel<ManBase>,IManBaseDataAccess
    {
        public ManBaseDataAccess(DataBaseOperate oper): base(oper)
        {
            
        }
        public ManBaseDataAccess(): base()
        {
        }
    }
}



