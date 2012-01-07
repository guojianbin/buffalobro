using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;

namespace Buffalo.DBTools.HelperKernel
{
    public class EntityRelationCollection : List<EntityRelationItem>
    {
        public void SortItem()
        {
            this.Sort(new FieldComparer<EntityRelationItem>());
        }



    }

    
}
