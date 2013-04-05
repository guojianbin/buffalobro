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
    /// 登陆员工数据访问层
    ///</summary>
    public class ScEmployeeDataAccess : DataAccessModel<ScEmployee>,IScEmployeeDataAccess
    {
        public ScEmployeeDataAccess(DataBaseOperate oper): base(oper)
        {
            
        }
        public ScEmployeeDataAccess(): base()
        {
        }
    }
}



