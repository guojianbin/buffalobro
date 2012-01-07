using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Buffalo.WinFormsControl.PagingBar
{
    public partial class PagingBar :UserControl
    {
        private static readonly Size BarSize = new Size(335, 26);
        public PagingBar()
        {
            InitializeComponent();
            this.Size = BarSize;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Size = BarSize;
        }

        #region 基本数据
        public event PageIndexChange OnPageIndexChange;

        private long _totlePage;
        /// <summary>
        /// 总页数
        /// </summary>
        public long TotlePage
        {
            get
            {
                return _totlePage;
            }
            
        }

        private long _currentPage = 0;
        /// <summary>
        ///当前页数
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
        /// 数据源
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
        /// 总记录条数
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
                
                if (_totleRecords >=0)
                {
                    if (_pageSize > 0)
                    {
                        _totlePage = _totleRecords / _pageSize;
                    }

                    
                }
            }
        }

        /// <summary>
        /// 对控件进行翻页处理
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
        /// 获取字符串里边的所有数字
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
        /// 填充数据
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
            lblPage.Text = "/"+TotlePage.ToString()+"页";
            labInfo.Text = "共["+TotleRecords.ToString()+"]条";
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
    public delegate void PageIndexChange(object sender, long currentIndex);
}

