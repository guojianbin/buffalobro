using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using System.Collections.Generic;
using Buffalo.DB.QueryConditions;
using TestPerLib.BQLEntity;
using TestPerLib;
using Buffalo.DB.DbCommon;
namespace TestPerLib.DataAccess.Bql
{
    ///<summary>
    /// ��½Ա�����ݷ��ʲ�
    ///</summary>
    public class ScEmployeeDataAccess :BQLDataAccessBase<ScEmployee>
    {
        public ScEmployeeDataAccess(DataBaseOperate oper)
            : base(oper)
        {
            
        }
        public ScEmployeeDataAccess()
        {
            
        }
    }
}



