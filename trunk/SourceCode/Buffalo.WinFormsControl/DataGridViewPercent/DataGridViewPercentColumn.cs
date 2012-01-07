using System;
using System.Collections.Generic;
using System.Text;
/** 
@author 289323612@qq.com
@version 创建时间：2011-12-1
给DataGridView添加支持百分比的列 
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
        /// 是否在值后边显示总数
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
        /// 列总数
        /// </summary>
        public decimal TotleCount
        {
            get { return _TotleCount; }
            set { _TotleCount = value; }
        }

        private string _valueForamt;

        /// <summary>
        /// 值的格式化
        /// </summary>
        public string ValueForamt
        {
            get { return _valueForamt; }
            set { _valueForamt = value; }
        }
    }
}
