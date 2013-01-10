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
    /// �ı���ı༭��
    /// </summary>
    public partial class CheckBoxEditor : EditorBase
    {
        public CheckBoxEditor()
        {
            InitializeComponent();
        }

        private void CheckBoxEditor_Load(object sender, EventArgs e)
        {
            
        }


        /// <summary>
        /// �Ƿ�ѡ��
        /// </summary>
        public bool Checked 
        {
            get 
            {
                return chkValue.Checked;
            }
            set 
            {
                chkValue.Checked = value;
            }
        }
        /// <summary>
        /// ֵ
        /// </summary>
        public override object Value
        {
            get
            {
                return Checked;
            }
            set
            {

                Checked = Convert.ToBoolean(value);

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
        private void chkValue_CheckedChanged(object sender, EventArgs e)
        {
            DoValueChange(sender,chkValue.Checked);
        }


    }
}
