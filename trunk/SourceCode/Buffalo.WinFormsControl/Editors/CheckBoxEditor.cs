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
    [ToolboxItem(true)]
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
        /// ��ť��ʽ����
        /// </summary>
        public OnOffButtonType OnOffType
        {
            get { return chkValue.OnOffType; }
            set 
            { 
                chkValue.OnOffType = value;

            }
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


        public override Label Lable
        {
            get
            {
                return labSummary;
            }
        }

        
        private void chkValue_CheckedChanged(object sender, EventArgs e)
        {
            DoValueChange(sender,chkValue.Checked);
        }


    }
}
