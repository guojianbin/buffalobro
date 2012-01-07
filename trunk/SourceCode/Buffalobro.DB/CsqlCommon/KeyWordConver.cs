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
    /// 关键字转换器
    /// </summary>
    internal class KeyWordConver
    {
        private Stack<CsqlQuery> stkKeyWord = new Stack<CsqlQuery>();
        
        /// <summary>
        /// 开始转换
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal AbsCondition ToConver(CsqlQuery item, KeyWordInfomation info) 
        {
            
            CollectItem(item, info);
            return DoConver(info);
        }

        /// <summary>
        /// 遍历关键字链，把它放进关键字栈(把倒置的关键字项转回来)
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
        ///// 加载From的别名表的信息
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
        /// 开始分析
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
