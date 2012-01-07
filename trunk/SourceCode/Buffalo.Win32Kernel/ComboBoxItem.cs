using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel
{
    /// <summary>
    /// ComboBox的项
    /// </summary>
    public class ComboBoxItem
    {
        private string _text;
        /// <summary>
        /// 要显示的文本
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }


        private object _tag;
        /// <summary>
        /// 值
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
