using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Buffalo.Permissions.PermissionsInfo;
using System.Collections.Generic;
using Buffalo.DB.QueryConditions;
using Buffalo.Permissions.PermissionsInfo.BQLEntity;
using System.Reflection;

public partial class PermissionManager : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 设置或获取分组的组ID
    /// </summary>
    public string GroupID
    {
        get 
        {
            return ViewState["PermissionManager$$GroupID"] as string;
        }
        set 
        {
            ViewState["PermissionManager$$GroupID"] = value;
        }
    }

    /// <summary>
    /// 绑定权限
    /// </summary>
    private void BindPermission() 
    {
        string gid = GroupID;



    }



}
