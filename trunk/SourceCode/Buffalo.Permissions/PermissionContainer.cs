using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;
using System.Web;

namespace Buffalo.Permissions
{
    /// <summary>
    /// È¨ÏÞÈÝÆ÷
    /// </summary>
    public class PermissionContainer
    {
        private const string PermissionSessionID = "Buffalo.Permissions";




        public static object PerData 
        {
            get 
            {
                if (CommonMethods.IsWebContext) 
                {
                    return HttpContext.Current.Session[PermissionSessionID];
                }
                return null;
            }
        }
    }
}
