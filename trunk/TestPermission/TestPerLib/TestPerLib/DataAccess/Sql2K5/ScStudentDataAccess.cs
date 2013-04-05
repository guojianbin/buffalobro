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
    /// ���ݷ��ʲ�
    ///</summary>
    public class ScStudentDataAccess : DataAccessModel<ScStudent>,IScStudentDataAccess
    {
        public ScStudentDataAccess(DataBaseOperate oper): base(oper)
        {
            
        }
        public ScStudentDataAccess(): base()
        {
        }
    }
}



