using System;
using System.Collections.Generic;
using System.Text;

namespace ModelCompiler
{
    public interface ICodeOutputer
    {
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        public string GetCode(Queue<ExpressionItem> queitem);
    }
}
