using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.HelperKernel
{

    public class ComboBoxItemCollection : List<ComboBoxItem>
    {
        /// <summary>
        /// ���ƻ�ȡ��
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ComboBoxItem FindByText(string text) 
        {
            foreach (ComboBoxItem item in this) 
            {
                if (item.Text == text) 
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// ���ƻ�ȡ��
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ComboBoxItem FindByValue(object value)
        {
            foreach (ComboBoxItem item in this)
            {
                if (item.Value == null && value==null) 
                {
                    return item;
                }
                if (item.Value.Equals(value))
                {
                    return item;
                }
            }
            return null;
        }
    }



    public class ComboBoxItem
    {
        public ComboBoxItem(string text, object value)
        {
            _value = value;
            _text = text;
        }

        public ComboBoxItem(string text)
        {
            _value = text;
            _text = text;
        }

        private object _tag;

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        private object _value;

        /// <summary>
        /// ֵ
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string _text;
        /// <summary>
        /// ��ʾ�ı�
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
