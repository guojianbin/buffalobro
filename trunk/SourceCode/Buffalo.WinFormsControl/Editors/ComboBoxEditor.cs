using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Buffalo.Win32Kernel;

namespace Buffalo.WinFormsControl.Editors
{
    /// <summary>
    /// �ı���ı༭��
    /// </summary>
    [ToolboxItem(true)]
    public partial class ComboBoxEditor : EditorBase
    {
        public ComboBoxEditor()
        {
            InitializeComponent();
        }

        private void ComboBoxEditor_Load(object sender, EventArgs e)
        {
            
        }

        private void pnlValue_Resize(object sender, EventArgs e)
        {
            int width = pnlValue.Width - 4;
            if (width > 0) 
            {
                cmbValue.Width = width;
            }
        }
        public override string  Text
        {
	        get 
	        {
                return cmbValue.Text;
	        }
	        set 
	        {
                cmbValue.Text = value;
	        }
        }

        /// <summary>
        /// ֵ
        /// </summary>
        public override object Value
        {
            get
            {
                return cmbValue.SelectedValue;
            }
            set
            {
                cmbValue.SelectedValue=value;
            }
        }
        /// <summary>
        /// ��ǩ���
        /// </summary>
        public int LableWidth 
        {
            get 
            {
                return pnlLable.Width;
            }
            set 
            {
                pnlLable.Width = value;
            }
        }

        /// <summary>
        /// ��ǩ����
        /// </summary>
        public Font LableFont
        {
            get
            {
                return labSummary.Font;
            }
            set
            {
                labSummary.Font=value;
            }
        }

        /// <summary>
        /// ��ǩ����
        /// </summary>
        public string LableText
        {
            get 
            {
                return labSummary.Text;
            }
            set 
            {
                labSummary.Text = value;
            }
        }

        private void cmbValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoValueChange(sender, cmbValue.SelectedValue);
        }

        /// <summary>
        /// ��ֵ
        /// </summary>
        /// <param name="lstItem"></param>
        public void BindValue(List<ComboBoxItem> lstItem) 
        {
            cmbValue.DisplayMember = "Text";
            cmbValue.ValueMember = "Tag";
            cmbValue.DataSource = lstItem;
        }


    }
}
