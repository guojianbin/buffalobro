using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using System.Collections.Generic;
using Buffalo.DB.QueryConditions;
using TestAddIn.BQLEntity;
using TestAddIn;
using Buffalo.DB.DbCommon;
namespace TestAddIn.DataAccess.Bql
{
    ///<summary>
    /// ���ݷ��ʲ�
    ///</summary>
    public class ManUsersDataAccess :BQLDataAccessBase<ManUsers>
    {
        public ManUsersDataAccess(DataBaseOperate oper)
            : base(oper)
        {
            
        }
        public ManUsersDataAccess()
        {
            
        }
    }
}



