using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Buffalo.DBTools.UIHelper
{
    public partial class FrmCompileResault : Form
    {
        public FrmCompileResault()
        {
            InitializeComponent();
        }

        private void FrmCompileError_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 显示编译结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="error"></param>
        public static void ShowCompileResault(string code, string error) 
        {
            FrmCompileResault frm = new FrmCompileResault();
            if (!string.IsNullOrEmpty(code))
            {
                frm.txtCode.Text = code;
            }
            else 
            {
                frm.spInfo.Panel1Collapsed = true;
            }
            if (!string.IsNullOrEmpty(error))
            {
                frm.txtError.Text = error;
                frm.Text = "模版编译错误";
            }
            else 
            {
                frm.Text = "模版编译结果";
                frm.spInfo.Panel2Collapsed = true;
            }
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}