using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Buffalo.WinFormsControl.PagingBar
{
    public partial class MediumPagingBar : UserControl
    {
        public MediumPagingBar()
        {
            InitializeComponent();
        }
        #region ��������
        public event PageIndexChange OnPageIndexChange;

        /// <summary>
        /// ��ҳ��
        /// </summary>
        public long TotlePage
        {
            get
            {
                if (_pageSize > 0)
                {
                    return (long)Math.Ceiling((double)_totleRecords / (double)_pageSize);
                }
                return 0;
            }

        }

        private long _currentPage = 0;
        /// <summary>
        ///��ǰҳ��
        /// </summary>
        public long CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
            }
        }


        private int _pageSize;
        /// <summary>
        /// ����Դ
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        private long _totleRecords;
        /// <summary>
        /// �ܼ�¼����
        /// </summary>
        public long TotleRecords
        {
            get
            {
                return _totleRecords;
            }
            set
            {
                _totleRecords = value;

                
            }
        }

        /// <summary>
        /// �Կؼ����з�ҳ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="currentIndex"></param>
        protected void DoPageIndexChange(object sender, long currentIndex)
        {

            this.CurrentPage = currentIndex;
            if (OnPageIndexChange != null)
            {
                OnPageIndexChange(sender, currentIndex);
            }
        }

        /// <summary>
        /// ��ȡ�ַ�����ߵ���������
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetAllNumber(string str)
        {
            if (str == null || str == "")
            {
                return "0";
            }
            string ret = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsDigit(str, i))
                {
                    ret += str[i];
                }
            }
            if (ret.Length <= 0)
            {
                ret = "0";
            }
            return ret;
        }
        #endregion
        /// <summary>
        /// �������
        /// </summary>
        public void BindData()
        {
            long curPage = CurrentPage + 1;
            long totlePage = TotlePage;
            this.Visible = true;
            if (TotleRecords == 0)
            {
                this.Visible = false;
                return;
            }
            CurrentPage = CurrentPage;
            lblPage.Text = "/" + totlePage.ToString() + "ҳ";
            labInfo.Text = "��[" + TotleRecords.ToString() + "]��";
            txtPage.Text = curPage.ToString();
            btnLast.Enabled = true;
            btnNext.Enabled = true;
            btnFirsh.Enabled = true;
            btnUp.Enabled = true;
            if (CurrentPage == 0)
            {
                //lbPri.Enabled = false;

                btnFirsh.Enabled = false;
                btnUp.Enabled = false;
            }
            if (CurrentPage >= TotlePage - 1)
            {
                //lbNext.Enabled = false;
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
        }

        private void btnFirsh_Click(object sender, EventArgs e)
        {
            DoPageIndexChange(this, 0);
        }

        private void txtPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                int page = Convert.ToInt32(GetAllNumber(txtPage.Text));

                long totle = TotlePage;
                if (page > totle)
                {
                    page = 0;
                }
                DoPageIndexChange(this, page - 1);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            long page = CurrentPage - 1;
            if (page < 0)
            {
                page = 0;
            }
            DoPageIndexChange(this, page);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            long page = CurrentPage + 1;
            if (page > TotlePage - 1)
            {
                page = TotlePage - 1;
            }

            DoPageIndexChange(this, page);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {

            DoPageIndexChange(this, TotlePage - 1);
        }
    }
}
