using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel.Defaults;
using System.ComponentModel;
using System.Drawing;


namespace Buffalo.WinFormsControl.Editors
{
    public delegate void ValueChangeHandle(object sender,object oldValue,object newValue);
    [ToolboxItem(false)]
    public class EditorBase:UserControl
    {
        public event ValueChangeHandle OnValueChange;

        private string _bindPropertyName;
        /// <summary>
        /// �󶨵�������
        /// </summary>
        public string BindPropertyName
        {
            get { return _bindPropertyName; }
            set { _bindPropertyName = value; }
        }
        /// <summary>
        /// ��ǩ����
        /// </summary>
        public virtual string LableText
        {
            get
            {
                return Lable.Text;
            }
            set
            {
                Lable.Text = value;
            }
        }
        public virtual Font LableFont
        {
            get
            {
                return Lable.Font;
            }
            set
            {
                Lable.Font = value;
            }
        }

        public virtual Color LableForeColor
        {
            get
            {
                return Lable.ForeColor;
            }
            set
            {
                Lable.ForeColor = value;
            }
        }
        
        

        public virtual Label Lable
        {
            get 
            { 
                return null; 
            }
            
        }

        /// <summary>
        /// ֵ
        /// </summary>
        public virtual object Value 
        {
            get { return null; }
            set { }
        }
        private object _oldvalue;
        /// <summary>
        /// ����ֵ�ı��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected void DoValueChange(object sender, object newValue)
        {
            if (OnValueChange != null) 
            {
                object ovalue = _oldvalue;
                if (ovalue==null && newValue != null) 
                {
                    ovalue = DefaultValue.DefaultForType(newValue.GetType());
                }
                OnValueChange(sender, ovalue, newValue);
                _oldvalue = newValue;
            }
        }
        /// <summary>
        /// ����ֵ�ı��¼�
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected void DoValueChange(object newValue)
        {
            DoValueChange(this, newValue);
        }
    }
}
