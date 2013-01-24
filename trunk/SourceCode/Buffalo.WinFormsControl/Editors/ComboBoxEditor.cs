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
    /// 文本框的编辑器
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
        /// 值
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
        /// 标签字体
        /// </summary>
        public Font LableFont
        {
            get
            {
                return cmbValue.Font;
            }
            set
            {
                cmbValue.Font = value;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return cmbValue.ForeColor;
            }
            set
            {
                cmbValue.ForeColor = value;
            }
        }


        public override Label Lable
        {
            get
            {
                return labSummary;
            }
        }


        

        private void cmbValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoValueChange(sender, cmbValue.SelectedValue);
        }

        /// <summary>
        /// 绑定值
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
