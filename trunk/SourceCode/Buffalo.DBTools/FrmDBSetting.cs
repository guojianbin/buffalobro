using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Buffalo.DBTools.HelperKernel;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DbCommon;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;

namespace Buffalo.DBTools
{
    public partial class FrmDBSetting : Form
    {
        public FrmDBSetting()
        {
            InitializeComponent();
        }

        private DBConfigInfo _info=new DBConfigInfo();

        public static DBConfigInfo GetDBConfigInfo(Project curProject, ClassDesignerDocView docView,string dalNamespace) 
        {
            DBConfigInfo dbinfo = DBConfigInfo.LoadInfo(curProject, docView);
            if (dbinfo == null)
            {
                using (FrmDBSetting frmSetting = new FrmDBSetting())
                {
                    frmSetting.Info.DbName = DBConfigInfo.GetDbName(docView);
                    if (frmSetting.ShowDialog() != DialogResult.OK)
                    {
                        return null;
                    }
                    dbinfo = frmSetting.Info;

                    dbinfo.AppNamespace = dalNamespace+"."+dbinfo.DbType;
                    dbinfo.FileName = DBConfigInfo.GetFileName(curProject, docView);
                    dbinfo.SaveConfig(dbinfo.FileName);
                }
            }
            return dbinfo;
        }

        /// <summary>
        /// 数据库信息
        /// </summary>
        public DBConfigInfo Info
        {
            get { return _info; }
        }

        /// <summary>
        /// 初始化数据库类型
        /// </summary>
        private void InitDBType() 
        {
            cmbType.DisplayMember = "Text";
            cmbType.ValueMember = "Value";
            cmbType.DataSource = Generate3Tier.DataAccessTypes;
        }

        private void InitTiers() 
        {

            cmbTier.DisplayMember = "Text";
            cmbTier.ValueMember = "Value";
            cmbTier.DataSource = Generate3Tier.Tiers;
        }

        private void FrmDBSetting_Load(object sender, EventArgs e)
        {
            InitTiers();
            InitDBType();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (FillInfo())
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 填充信息
        /// </summary>
        /// <returns></returns>
        private bool FillInfo() 
        {
            if (string.IsNullOrEmpty(rtbConnstr.Text))
            {
                MessageBox.Show("连接字符串不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            string dbType = cmbType.SelectedValue as string;
            if (string.IsNullOrEmpty(dbType))
            {
                MessageBox.Show("请选择数据库类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            object tier = cmbTier.SelectedValue;
            if (tier==null)
            {
                MessageBox.Show("请选择架构", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            _info.ConnectionString = rtbConnstr.Text;
            _info.DbType = dbType;
            _info.Tier = Convert.ToInt32(tier);
            return true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (!FillInfo())
                {
                    return;
                }
                DBInfo db = _info.CreateDBInfo();
                DataBaseOperate oper = db.CreateOperate();

                oper.ConnectDataBase();
                MessageBox.Show("测试成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch (Exception ex)
            {
                MessageBox.Show("连接错误:" + ex.Message, "连接数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}