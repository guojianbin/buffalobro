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
    [IDalAttribute(typeof(IManMessageDataAccess))]
    public class ManMessageDataAccess : DataAccessModel<ManMessage>,IManMessageDataAccess
    {
        public ManMessageDataAccess(DataBaseOperate oper): base(oper)
        {
            
        }
        public ManMessageDataAccess(): base()
        {
        }
    }
}



