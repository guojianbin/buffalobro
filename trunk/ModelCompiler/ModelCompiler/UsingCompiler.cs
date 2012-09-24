using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ModelCompiler
{
    public class UsingCompiler
    {
        private StringReader _content;
        public UsingCompiler(string content) 
        {
            _content = new StringReader(content);
        }
    }
}
