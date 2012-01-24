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
    ///  ÒµÎñ²ã
    /// </summary>
    public class ManUsersBusiness: TestAddIn.Business.ManBaseBusiness
    {
		public BusinessModelBase<ManUsers> Default
		{
			get
			{
				return new BusinessModelBase<ManUsers>();
			}
		}
        public ManUsersBusiness()
        {
            
        }
    }
}



