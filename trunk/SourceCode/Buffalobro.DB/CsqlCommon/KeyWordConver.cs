using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon;
using Buffalobro.DB.DbCommon;
using Buffalobro.DB.QueryConditions;

namespace Buffalobro.DB.CsqlCommon
{
    /// <summary>
    /// �ؼ���ת����
    /// </summary>
    internal class KeyWordConver
    {
        private Stack<CsqlQuery> stkKeyWord = new Stack<CsqlQuery>();
        
        /// <summary>
        /// ��ʼת��
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal AbsCondition ToConver(CsqlQuery item, KeyWordInfomation info) 
        {
            
            CollectItem(item, info);
            return DoConver(info);
        }

        /// <summary>
        /// �����ؼ������������Ž��ؼ���ջ(�ѵ��õĹؼ�����ת����)
        /// </summary>
        /// <param name="item"></param>
        internal void CollectItem(CsqlQuery item, KeyWordInfomation info) 
        {
            CsqlQuery curItem = item;

            while (curItem != null) 
            {
                curItem.LoadInfo(info);
                stkKeyWord.Push(curItem);
                curItem = curItem.Previous;
            }
        }

        
        ///// <summary>
        ///// ����From�ı��������Ϣ
        ///// </summary>
        ///// <param name="item"></param>
        ///// <param name="info"></param>
        //private void LoadParamInfo(CsqlKeyWordItem item, KeyWordInfomation info)
        //{
        //    KeyWordSelectItem sitem = item as KeyWordSelectItem;
        //    if (sitem!=null)
        //    {
        //        sitem.LoadParamInfo(info);
        //    }
        //}
        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns></returns>
        private AbsCondition DoConver(KeyWordInfomation info) 
        {

            
            StringBuilder ret = new StringBuilder(2000);
            while (stkKeyWord.Count > 0) 
            {
                CsqlQuery item = stkKeyWord.Pop();
                //LoadParamInfo(item, info);
                //ret.Append(item.Tran(info)) ;
                item.Tran(info);
            }
            return info.Condition;
        }

    }
}
