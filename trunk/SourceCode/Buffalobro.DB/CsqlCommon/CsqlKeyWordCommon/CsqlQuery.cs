using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    /// <summary>
    /// CSQL��ѯ
    /// </summary>
    public abstract class CsqlQuery
    {
        private CsqlQuery previous;
        /// <summary>
        /// �ؼ�����
        /// </summary>
        /// <param name="keyWordName">�ؼ�����</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal CsqlQuery( CsqlQuery previous) 
        {
            this.previous = previous;
        }
       
        /// <summary>
        /// ��һ���ؼ���
        /// </summary>
        internal CsqlQuery Previous
        {
            get
            {
                return previous;
            }
        }
        /// <summary>
        /// �ؼ��ֽ���
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal abstract void Tran(KeyWordInfomation info);
        internal abstract void LoadInfo(KeyWordInfomation info);
        /// <summary>
        /// �������ѯ����һ������
        /// </summary>
        /// <param name="asName">����(�������Ҫ��������Ϊnull��"")</param>
        /// <returns></returns>
        public CsqlAliasHandle AS(string asName) 
        {
            CsqlAliasHandle item = new CsqlAliasHandle(this, asName);
            return item;
        }
    }
}
