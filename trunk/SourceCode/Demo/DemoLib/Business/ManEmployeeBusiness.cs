using System;
using TestAddIn;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
namespace TestAddIn.Business
{
    /// <summary>
    ///  员工业务层
    /// </summary>
    public class ManEmployeeBusiness: TestAddIn.Business.ManBaseBusiness
    {
		public BusinessModelBase<ManEmployee> Default
		{
			get
			{
				return new BusinessModelBase<ManEmployee>();
			}
		}
        public ManEmployeeBusiness()
        {
            
        }
    }
}



