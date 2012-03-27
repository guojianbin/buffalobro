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
using System.IO;
using System.Xml;

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

        /// <summary>
        /// 当前表集合
        /// </summary>
        List<DBTableInfo> _curLst;

        private void FrmAllTables_Load(object sender, EventArgs e)
        {
            gvTables.AutoGenerateColumns = false;
            DBInfo info = DbInfo.CreateDBInfo();
            _curLst = TableChecker.GetAllTables(info);
            RefreashTablesInfo();
        }

        /// <summary>
        /// 刷新表状态
        /// </summary>
        private void RefreashTablesInfo()
        {
            gvTables.DataSource = null;
            if (_curLst != null && _curLst.Count > 0)
            {
                gvTables.DataSource = _curLst;
            }
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
                    string file = SelectDocView.DocData.FileName;
                    XmlDocument doc = DBEntityInfo.GetClassDiagram(file);

                    frmPro.UpdateProgress(0, 10, "正在读取类信息");
                    List<DBTableInfo> lstGen = TableChecker.GetTableInfo(db, selection);
                    string entityNamespace = DBEntityInfo.GetNameSpace(SelectDocView, CurrentProject);
                    for (int i = 0; i < lstGen.Count; i++)
                    {
                        frmPro.UpdateProgress(i, lstGen.Count, "正在生成");

                        DBEntityInfo info = new DBEntityInfo(entityNamespace, lstGen[i], SelectDocView, CurrentProject, DbInfo);
                        info.GreanCode(doc);
                    }
                    //拷贝备份
                    File.Copy(file, file + ".bak", true);
                    EntityMappingConfig.SaveXML(file, doc);
                }
            }
            this.Close();
        }



        /// <summary>
        /// 刷新选中状态
        /// </summary>
        private void RefreashCheckType() 
        {
           
            if (gvTables.Rows.Count>0)
            {
                bool isCheckAll = true;

                foreach (DataGridViewRow row in gvTables.Rows) 
                {
                    DataGridViewCheckBoxCell cell = row.Cells["ColChecked"] as DataGridViewCheckBoxCell;
                    if (cell != null && !(bool)cell.FormattedValue) 
                    {
                        isCheckAll = false;
                        break;
                    }
                }
                chkAll.Checked = isCheckAll;
            }
            
        }


        private void gvTables_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            string colName = gvTables.Columns[e.ColumnIndex].Name;
            if (colName == "ColChecked")
            {
                gvTables.EndEdit();
                RefreashCheckType();
                //if ((bool)gvTables.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue)
                //{
                //    dgTest[colNumber, e.RowIndex].Value = e.RowIndex.ToString() + 9999;
                //}
                
            }
        }


        private void chkAll_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (DataGridViewRow row in gvTables.Rows)
            {
                DataGridViewCheckBoxCell cell = row.Cells["ColChecked"] as DataGridViewCheckBoxCell;
                cell.Value = chkAll.Checked;
            }
        }
    }
}