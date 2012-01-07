using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace Buffalobro.DB.PropertyAttributes
{
    public class TableAttribute : System.Attribute
    {
        private string tableName;
        private bool isParamNameUpper;
        /// <summary>
        /// ���ʾ
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="connectionKey">�����ַ����ļ�</param>
        /// <param name="isParamNameUpper">�ֶ����Ƿ�ת�ɴ�д</param>
        public TableAttribute(string tableName, bool isParamNameUpper)
        {
            this.tableName = tableName;
            this.isParamNameUpper = isParamNameUpper;
        }
        /// <summary>
        /// ���ʾ
        /// </summary>
        /// <param name="tableName">����</param>
        public TableAttribute(string tableName):this(tableName,false)
        {

        }
        /// <summary>
        /// ����
        /// </summary>
        public string TableName 
        {
            get 
            {
                return tableName;
            }
        }
        /// <summary>
        /// �ֶ����Ƿ�ת�ɴ�д
        /// </summary>
        public bool IsParamNameUpper
        {
            get
            {
                return isParamNameUpper;
            }
        }

    }
}
