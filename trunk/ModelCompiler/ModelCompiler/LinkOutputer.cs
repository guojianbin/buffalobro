using System;
using System.Collections.Generic;
using System.Text;

namespace ModelCompiler
{
    public class LinkOutputer
    {

        #region ICodeOutputer ≥…‘±
        public string[] GetCode(Queue<ExpressionItem> queitem)
        {
            List<string> lst=new List<string>();
            while (queitem.Count > 0) 
            {
                ExpressionItem item = queitem.Dequeue();
                switch (item.Type) 
                {
                    case ExpressionType.String:
                        lst.AddRange(item.Content.ToString().Split('\n'));
                        break;
                    default:
                        break;
                }
            }
            return lst.ToArray();
        }

        #endregion
    }
}
