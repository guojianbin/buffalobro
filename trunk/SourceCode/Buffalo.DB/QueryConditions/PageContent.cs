using System;
using System.Data;
using Buffalo.DB;
using System.Data.SqlClient;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.Defaults;

namespace Buffalo.DB.QueryConditions
{
	/// <summary>
	/// ��ѯ����
	/// </summary>
	public enum SearchType
	{
		/// <summary>
		/// ��ȷ
		/// </summary>
		Precision,
		/// <summary>
		/// ģ��
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
        /// ��ȡ��ʼ��ѯ������(��0��ʼ)
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
		/// ��ʼ������
		/// </summary>
		/// <param name="ds">���ݼ�</param>
		private void InitData()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ds">ָ��һ��DataSet���������</param>
		public PageContent()
		{
			InitData();
		}

        /// <summary>
        /// ��ʼ��ѯ����(��0��ʼ)
        /// </summary>
        internal long StarIndex
        {
            get { return starIndex; }
            set { starIndex = value; }
        }
		
		/// <summary>
		/// ҳ��С
		/// </summary>
        public long PageSize
		{
			get { return pageSize; }
			set { pageSize=value; }
		}
		/// <summary>
		/// ҳ������0��ʼ��
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
		/// �ò�ѯ���ܼ�¼��
		/// </summary>
        public long TotleRecords
		{
            set { totleRecords = value; }
			get { return totleRecords; }
		}
        /// <summary>
        /// ��ҳ��
        /// </summary>
        public long TotlePage
        {
            set { totlePage = value; }
            get { return totlePage; }
        }
		/// <summary>
		/// ����ѯ����
		/// </summary>
        public long MaxSelectRecords
		{
            set { maxSelectRecords = value; }
            get { return maxSelectRecords; }
		}

        /// <summary>
        /// �Ƿ���������
        /// </summary>
        public bool IsFillTotleRecords
        {
            get { return isFillTotleRecords; }
            set { isFillTotleRecords = value; }
        }

	}
}
