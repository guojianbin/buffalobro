using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Buffalo.DB.DBCheckers;
using Buffalo.DBTools.HelperKernel;
using Buffalo.DB.DataBaseAdapter;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Buffalo.DBTools.ROMHelper;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using Buffalo.DB.CommBase.BusinessBases;

namespace Buffalo.DBTools
{
    public partial class FrmAllTables : Form
    {
        public FrmAllTables()
        {
            InitializeComponent();
        }
        ClassDesignerDocView _selectDocView;
        /// <summary>
        /// 选择的文档
        /// </summary>
        public ClassDesignerDocView SelectDocView
        {
            get { return _selectDocView; }
            set { _selectDocView = value; }
        }
        private Diagram _selectedDiagram = null;

        /// <summary>
        /// 选中的关系图
        /// </summary>
        public Diagram SelectedDiagram
        {
            get { return _selectedDiagram; }
            set { _selectedDiagram = value; }
        }
        private Project _currentProject;
        /// <summary>
        /// 当前项目
        /// </summary>
        public Project CurrentProject
        {
            get { return _currentProject; }
            set { _currentProject = value; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        DBConfigInfo _dbInfo;
        /// <summary>
        /// 数据库信息
        /// </summary>
        public DBConfigInfo DbInfo 
        {
            get 
            {
                if (_dbInfo == null) 
                {
                    _dbInfo = FrmDBSetting.GetDBConfigInfo(CurrentProject, SelectDocView, DBEntityInfo.GetNameSpace(SelectDocView, CurrentProject) + ".DataAccess");
                    

                }
                return _dbInfo;
            }
        }

        private void FrmAllTables_Load(object sender, EventArgs e)
        {
            gvTables.AutoGenerateColumns = false;
            DBInfo info = DbInfo.CreateDBInfo();
            List<DBTableInfo> lst = TableChecker.GetAllTables(info);
            gvTables.DataSource = lst;
        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            
            List<DBTableInfo> lst = gvTables.DataSource as List<DBTableInfo>;
            if (lst == null) 
            {
                return;
            }
            List<string> selection = new List<string>();
            foreach (DBTableInfo info in lst) 
            {
                if (info.IsGenerate) 
                {
                    selection.Add(info.Name);
                }
            }
            DBInfo db=DbInfo.CreateDBInfo();


            using (BatchAction ba = db.DefaultOperate.StarBatchAction())
            {
                using (FrmProcess frmPro = FrmProcess.ShowProcess())
                {
                    frmPro.UpdateProgress(0, 10, "正在读取类信息");
                    List<DBTableInfo> lstGen = TableChecker.GetTableInfo(db, selection);

                    string entityNamespace = DBEntityInfo.GetNameSpace(SelectDocView, CurrentProject);
                    for (int i = 0; i < lstGen.Count; i++)
                    {
                        frmPro.UpdateProgress(i, lstGen.Count, "正在生成");

                        DBEntityInfo info = new DBEntityInfo(entityNamespace, lstGen[i], SelectDocView, CurrentProject, DbInfo);
                        info.GreanCode();
                    }
                }
            }
            this.Close();
        }


    }
}