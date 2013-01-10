using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel.Defaults;

namespace Buffalo.WinFormsControl.Editors
{
    public delegate void ValueChangeHandle(object sender,object oldValue,object newValue);
    public class EditorBase:UserControl
    {
        public ValueChangeHandle OnValueChange;

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
