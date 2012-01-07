using System;
using System.Collections.Generic;
using System.Text;
/** 
@author 289323612@qq.com
@version ����ʱ�䣺2011-12-1
��DataGridView���֧�ְٷֱȵ��� 
*/
namespace Buffalo.WinFormsControl.DataGridViewPercent
{
    public class DataGridViewPercentColumn:System.Windows.Forms.DataGridViewColumn
    {

        public DataGridViewPercentColumn()
        {
            base.CellTemplate = new DataGridPercentCell();
        }

        private bool _showTotle=false;

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
