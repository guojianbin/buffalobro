using System;
using System.Collections.Generic;
using System.Text;

namespace ModelCompiler
{
    public interface ICodeOutputer
    {
        /// <summary>
        /// ЛёШЁДњТы
        /// </summary>
        /// <returns></returns>
        string GetCode(Queue<ExpressionItem> queitem);
    }
}
