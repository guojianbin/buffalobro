using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Buffalo.WinFormsControl.Editors
{
    /// <summary>
    /// 文本框的编辑器
    /// </summary>
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
                txtValue.Width = width;
            }
        }
        public override string  Text
        {
	        get 
	        { 
		         return txtValue.Text;
	        }
	          set 
	        { 
		        txtValue.Text = value;
	        }
        }
        /// <summary>
        /// 标签宽度
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
        /// 标签字体
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
        /// 标签内容
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

        
        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            DoValueChange(sender, txtValue.Text);
        }



    }
}
