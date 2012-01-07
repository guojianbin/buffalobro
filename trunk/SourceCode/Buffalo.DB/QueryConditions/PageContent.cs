using System;
using System.Data;
using Buffalo.DB;
using System.Data.SqlClient;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.Defaults;

namespace Buffalo.DB.QueryConditions
{
	/// <summary>
	/// 查询类型
	/// </summary>
	public enum SearchType
	{
		/// <summary>
		/// 精确
		/// </summary>
		Precision,
		/// <summary>
		/// 模糊
		/// </summary>
		Faintness
	}
	
	public class PageContent
	{
        private long pageSize = 0;
        private long currentPage = 0;
        private long totlePage = 0;
        private long totleRecords = 0;
        private long maxSelectRecords = 0;
        
        private bool isFillTotleRecords=true;

        private long starIndex = DefaultValue.DefaultLong;

        private int pagerIndex=0;

        internal int PagerIndex
        {
            get { return pagerIndex; }
            set { pagerIndex = value; }
        }

        /// <summary>
        /// 获取起始查询的索引(从0开始)
        /// </summary>
        /// <returns></returns>
        public long GetStarIndex() 
        {
            if (starIndex != DefaultValue.DefaultLong)
            {
                return starIndex;
            }
            else 
            {
                return pageSize * currentPage;
            }
        }

		/// <summary>
		/// 初始化数据
		/// </summary>
		/// <param name="ds">数据集</param>
		private void InitData()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ds">指定一个DataSet来填充数据</param>
		public PageContent()
		{
			InitData();
		}

        /// <summary>
        /// 起始查询条数(从0开始)
        /// </summary>
        internal long StarIndex
        {
            get { return starIndex; }
            set { starIndex = value; }
        }
		
		/// <summary>
		/// 页大小
		/// </summary>
        public long PageSize
		{
			get { return pageSize; }
			set { pageSize=value; }
		}
		/// <summary>
		/// 页数，从0开始算
		/// </summary>
        public long CurrentPage
		{
			get { return currentPage; }
			set {
                if (value >= 0)
                {
                    currentPage = value;
                }
            }
		}

		

		/// <summary>
		/// 该查询的总记录数
		/// </summary>
        public long TotleRecords
		{
            set { totleRecords = value; }
			get { return totleRecords; }
		}
        /// <summary>
        /// 总页数
        /// </summary>
        public long TotlePage
        {
            set { totlePage = value; }
            get { return totlePage; }
        }
		/// <summary>
		/// 最大查询条数
		/// </summary>
        public long MaxSelectRecords
		{
            set { maxSelectRecords = value; }
            get { return maxSelectRecords; }
		}

        /// <summary>
        /// 是否查出总条数
        /// </summary>
        public bool IsFillTotleRecords
        {
            get { return isFillTotleRecords; }
            set { isFillTotleRecords = value; }
        }

	}
}
