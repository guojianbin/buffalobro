using System;
using System.Data;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using TestAddIn;
using System.Collections.Generic;
using TestAddIn.DataAccess.IDataAccess;

namespace TestAddIn.DataAccess.DB2v9
{
    ///<summary>
    /// Êý¾Ý·ÃÎÊ²ã
    ///</summary>
    [IDalAttribute(typeof(IManClassDataAccess))]
    public class ManClassDataAccess : DataAccessModel<ManClass>,IManClassDataAccess
    {
        public ManClassDataAccess(DataBaseOperate oper): base(oper)
        {
            
        }
        public ManClassDataAccess(): base()
        {
        }
    }
}



