using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AddInSetup
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        List<AddInInfo> _lstAddIns = null;

        /// <summary>
        /// ˢ���б�
        /// </summary>
        private void RefreashDisplay() 
        {
            gvAddIns.DataSource = null;
            if (_lstAddIns != null && _lstAddIns.Count > 0) 
            {
                gvAddIns.DataSource = _lstAddIns;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            _lstAddIns = AddInInfo.GetAddInInfo();
            gvAddIns.AutoGenerateColumns = false;
            RefreashDisplay();
        }

        private void gvAddIns_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = gvAddIns.Columns[e.ColumnIndex].Name;
            if (colName == "ColState") 
            {
                AddInInfo info = gvAddIns.Rows[e.RowIndex].DataBoundItem as AddInInfo;
                if (info != null) 
                {
                    if (info.IsSetup)
                    {
                        string err=info.UnInstall();
                        if (err == null)
                        {
                            MessageBox.Show("ж�����", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else 
                        {
                            MessageBox.Show("ж�ش���:"+err, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else 
                    {
                        string err = info.Install();
                        if (err == null)
                        {
                            MessageBox.Show("��װ���", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("��װ����:" + err, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    RefreashDisplay();
                }
            }
        }
    }
}