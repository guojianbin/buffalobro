using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.WinFormsControl.DataGridViewEx
{
    public class DataGridViewPercentColumn:System.Windows.Forms.DataGridViewColumn
    {

        public DataGridViewPercentColumn()
        {
            base.CellTemplate = new DataGridViewPercentCell();
        }

        private bool _showTotle = false;

        /// <summary>
        /// �Ƿ���ֵ�����ʾ����
        /// </summary>
        public bool ShowTotle
        {
            get
            {
                return _showTotle;
            }
            set
            {
                _showTotle = value;
            }
        }

        private decimal _TotleCount;
        /// <summary>
        /// ������
        /// </summary>
        public decimal TotleCount
        {
            get { return _TotleCount; }
            set { _TotleCount = value; }
        }

        private string _valueForamt;

        /// <summary>
        /// ֵ�ĸ�ʽ��
        /// </summary>
        public string ValueForamt
        {
            get { return _valueForamt; }
            set { _valueForamt = value; }
        }
    }
}
