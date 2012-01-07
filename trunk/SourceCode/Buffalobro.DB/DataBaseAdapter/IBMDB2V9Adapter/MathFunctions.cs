using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalobro.DB.DataBaseAdapter.IBMDB2V9Adapter
{
    public class MathFunctions : SQLCommon.DataBaseAdapter.Oracle9Adapter.MathFunctions
    {
        public override string DoRandom(string[] values)
        {
            return " rand()";
        }
    }
}
