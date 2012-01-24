using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;

namespace Buffalo.DBTools.HelperKernel
{
    public class EntityParamFieldCollection : List<EntityParamField>
    {
        public void SortItem() 
        {
            this.Sort(new FieldComparer<EntityParamField>());
        }
        
    }
    
}
