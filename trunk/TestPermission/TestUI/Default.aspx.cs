using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Buffalo.Kernel;
using Buffalo.Permissions.PermissionsInfo;
using Buffalo.Permissions;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
using Buffalo.DB.QueryConditions;

public partial class _Default : System.Web.UI.Page 
{
    /// <summary>
    /// 获取权限信息
    /// </summary>
    /// <returns></returns>
    public static BPSysModel GetPermissionsInfo() 
    {
        BPSysModel modelInfo =BPSysModel.CreateSysModel("TestUI.EditUser","编辑用户",
            "系统功能.用户管理", "编辑用户信息");
        modelInfo.AddModelItem("EditUser","编辑用户", "编辑用户信息");
        return modelInfo;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        PermissionManager.CheckPermission(this.GetType().Assembly);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        PermissionManager.AddUserGroup("1", "管理员组");
        
    }
}
