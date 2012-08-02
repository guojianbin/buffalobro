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
using Buffalo.Kernel;
using Buffalo.DBTools.DocSummary;
using Buffalo.DB.CommBase;

namespace Buffalo.DBTools
{
    public partial class FrmDBSetting : Form
    {
        public FrmDBSetting()
        {
            InitializeComponent();
            FillSummary();
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
                    StaticConnection.ClearCacheOperate(dbinfo.DbName);
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
            set { _info = value; }
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
            FillEdit();
            
        }

        private void FillEdit() 
        {
            if (_info != null) 
            {
                if (!string.IsNullOrEmpty(_info.DbType))
                {
                    cmbType.SelectedValue = _info.DbType;
                }
                if (_info.Tier == 1 || _info.Tier == 3)
                {
                    cmbTier.SelectedValue = _info.Tier;
                }
                rtbConnstr.Text = _info.ConnectionString;
                chkAllDal.Checked = _info.IsAllDal;
            }
        }
        /// <summary>
        /// 填充注释设置
        /// </summary>
        private void FillSummary() 
        {
            List<EnumInfo> infos = EnumUnit.GetEnumInfos(typeof(SummaryShowItem));
            
            foreach (EnumInfo info in infos) 
            {
                if (info.FieldName != "All") 
                {
                    ComboBoxItem item = new ComboBoxItem(info.Description, (int)info.Value);
                    clbSummary.Items.Add(item);
                }
                
            }

        }

        /// <summary>
        /// 显示配置框
        /// </summary>
        /// <param name="curProject"></param>
        /// <param name="docView"></param>
        /// <param name="dalNamespace"></param>
        public static void ShowConfig(Project curProject, ClassDesignerDocView docView, string dalNamespace) 
        {
            DBConfigInfo dbinfo = DBConfigInfo.LoadInfo(curProject, docView);

            using (FrmDBSetting frmSetting = new FrmDBSetting())
            {

                if (dbinfo == null)
                {
                    dbinfo = new DBConfigInfo();
                    dbinfo.DbName = DBConfigInfo.GetDbName(docView);
                    dbinfo.SummaryShow = SummaryShowItem.All;
                    dbinfo.FileName = DBConfigInfo.GetFileName(curProject, docView);

                }
                frmSetting.Info= dbinfo;
                frmSetting.SummaryItem = dbinfo.SummaryShow;
                if (frmSetting.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                if (string.IsNullOrEmpty(dbinfo.AppNamespace)) 
                {
                    dbinfo.AppNamespace = dalNamespace + "." + dbinfo.DbType;
                }

                dbinfo.SaveConfig(dbinfo.FileName);
                StaticConnection.ClearCacheOperate(dbinfo.DbName);
            }
            
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

        private SummaryShowItem _summaryItem;

        /// <summary>
        /// 设置注释选中状态
        /// </summary>
        private SummaryShowItem SummaryItem 
        {
            get 
            {
                int val = 0;
                for (int i = 0; i < clbSummary.Items.Count; i++) 
                {
                    ComboBoxItem item = clbSummary.Items[i] as ComboBoxItem;
                    
                    if (clbSummary.GetItemChecked(i) && item != null)
                    {
                        int curVal = Convert.ToInt32(item.Value);
                        val = val | curVal;
                    }
                }
                return (SummaryShowItem)val;
            }
            set 
            {
                _summaryItem = value;
                if (clbSummary != null)
                {
                    int val = (int)_summaryItem;
                    for (int i = 0; i < clbSummary.Items.Count; i++)
                    {
                        ComboBoxItem item = clbSummary.Items[i] as ComboBoxItem;
                        if (EnumUnit.ContainerValue(val, Convert.ToInt32(item.Value)))
                        {
                            clbSummary.SetItemChecked(i, true);
                        }

                    }
                }
                
            }
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
            _info.SummaryShow = SummaryItem;
            _info.IsAllDal = chkAllDal.Checked;
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
                using (DataBaseOperate oper = db.CreateOperate())
                {

                    oper.ConnectDataBase();
                    MessageBox.Show("测试成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("连接错误:" + ex.Message, "连接数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnModel_Click(object sender, EventArgs e)
        {
            string dbType = cmbType.SelectedValue as string;
            if (string.IsNullOrEmpty(dbType))
            {
                MessageBox.Show("请选择数据库类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string conn = Generate3Tier.GetConnString(dbType);
            if (!string.IsNullOrEmpty(conn)) 
            {
                rtbConnstr.Text = conn;
            }
        }
    }
}