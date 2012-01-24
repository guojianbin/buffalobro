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
    /// 员工数据访问层
    ///</summary>
    [IDalAttribute(typeof(IManEmployeeDataAccess))]
    public class ManEmployeeDataAccess : DataAccessModel<ManEmployee>,IManEmployeeDataAccess
    {
        public ManEmployeeDataAccess(DataBaseOperate oper): base(oper)
        {
            
        }
        public ManEmployeeDataAccess(): base()
        {
        }
    }
}



