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
using TestPerLib;
using TestPerLib.Business;
using System.Collections.Generic;
using Buffalo.DB.QueryConditions;

public partial class ManagerPage_UserEdit : ScPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            FillBelongClass();
        }

    }

    /// <summary>
    /// 把界面内容填充到实体
    /// </summary>
    /// <param name="objClass"></param>
    private void FillInfo(ScStudent objStudent) 
    {
        if(ViewState["Id"]!=null)
        {
            objStudent.Id = Convert.ToInt32(ViewState["Id"]);
        }
        objStudent.Name = txtName.Text;
        objStudent.ClassId = Convert.ToInt32(ddlClass.SelectedValue);

    }

    /// <summary>
    /// 把实体内容加载到界面
    /// </summary>
    /// <param name="objStudent"></param>
    private void LoadInfo(ScStudent objStudent) 
    {

    }

    /// <summary>
    /// 填充所属班级的单选框
    /// </summary>
    private void FillBelongClass() 
    {
        ddlClass.Items.Clear();
        ScClassBusiness bo = new ScClassBusiness();
        ScopeList lstScope=new ScopeList();
        List<ScClass> lst = bo.SelectList(lstScope);
        foreach (ScClass obj in lst) 
        {
            ListItem item=new ListItem(obj.ClassName,obj.Id.ToString());
            ddlClass.Items.Add(item);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ScStudent obj = new ScStudent();
        FillInfo(obj);
        ScStudentBusiness bo = new ScStudentBusiness();
        bo.Insert(obj);
        Alert("保存完毕");
    }
}
