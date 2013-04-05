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
    /// ��½Ա�����ݷ��ʲ�
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



