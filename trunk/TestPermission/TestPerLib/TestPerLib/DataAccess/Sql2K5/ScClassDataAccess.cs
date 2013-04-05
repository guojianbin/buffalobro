using System;
using System.Data;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using TestPerLib;
using System.Collections.Generic;
using TestPerLib.DataAccess.IDataAccess;

namespace TestPerLib.DataAccess.Sql2K5
{
    ///<summary>
    /// Êý¾Ý·ÃÎÊ²ã
    ///</summary>
    public class ScClassDataAccess : DataAccessModel<ScClass>,IScClassDataAccess
    {
        public ScClassDataAccess(DataBaseOperate oper): base(oper)
        {
            
        }
        public ScClassDataAccess(): base()
        {
        }
    }
}



