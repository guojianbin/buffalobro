using System;
using System.Collections.Generic;
using System.Text;

namespace ModelCompiler
{
    public class LinkOutputer
    {

        #region ICodeOutputer 成员
        ///// <summary>
        ///// 过滤连接dll信息
        ///// </summary>
        ///// <param name="items"></param>
        ///// <returns></returns>
        //private string[] LinkFilter(string items) 
        //{

        //}

        /// <summary>
        /// 获取代码
        /// </summary>
        /// <param name="queitem"></param>
        /// <param name="modelPath">模块路径</param>
        /// <returns></returns>
        public List<string> GetCode(Queue<ExpressionItem> queitem,string modelPath)
        {
            List<string> lst=new List<string>();
            while (queitem.Count > 0) 
            {
                ExpressionItem item = queitem.Dequeue();
                switch (item.Type) 
                {
                    case ExpressionType.String:

                        lst.AddRange(item.Content.ToString().Split('\n'));
                        for (int i = 0; i < lst.Count; i++) 
                        {
                            lst[i] = lst[i].Replace("{BasePath}", modelPath);
                        }
                        break;
                    default:
                        break;
                }
            }
            return lst;
        }

        #endregion
    }
}
