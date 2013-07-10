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
using System.Collections.Generic;
using TestPerLib.Business;
using Buffalo.DB.QueryConditions;
using Buffalo.Permissions.DataViewInfo;
using TestPerLib.DataView;
using TestPerLib.BQLEntity;

public partial class ManagerPage_UserManager : ScPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) 
        {
            Bind();
            BindBelongClass();
        }
        pb.OnPageIndexChange += new Buffalo.WebKernel.WebCommons.PagerCommon.PageIndexChange(pb_OnPageIndexChange);
    }

    void pb_OnPageIndexChange(object sender, long currentIndex)
    {
        Bind();
        
    }

    /// <summary>
    /// 获取统计值
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string GetSum(string name) 
    {
        if (dtSum == null) 
        {
            return "0";
        }
        if (dtSum.Rows.Count == 0) 
        {
            return "0";
        }
        DataRow dr = dtSum.Rows[0];
        object value = dr[name];
        if (value == null || value is DBNull) 
        {
            return "0";
        }
        return value.ToString();
    }
    DataTable dtSum = null;
    /// <summary>
    /// 绑定数据
    /// </summary>
    private void Bind() 
    {
        //ScStudentBusiness bo=new ScStudentBusiness();
        //ScopeList lstScope=new ScopeList();
        //FillCondition(lstScope);
        //lstScope.PageContent.PageSize = 10;
        //lstScope.PageContent.CurrentPage =pb.CurrentPage;
        //List<ScStudent> lst=bo.SelectList(lstScope);
        //dtSum = DataViewerSum.GetSumQuery(new ScStudentDataView(), lstScope, School.GetDBinfo());
        //gvDisplay.DataSource = lst;
        //gvDisplay.DataBind();
        //pb.DataSource = lstScope.PageContent;
    }

    /// <summary>
    /// 绑定所属套餐
    /// </summary>
    private void BindBelongClass() 
    {
        ddlBelongClass.Items.Clear();
        ScClassBusiness bo=new ScClassBusiness();
        ScopeList lstScope=new ScopeList();
        List<ScClass> lstClass = bo.SelectList(lstScope);
        ListItem item = new ListItem("全部", "");
        ddlBelongClass.Items.Add(item);
        foreach (ScClass objClass in lstClass) 
        {
            item=new ListItem(objClass.ClassName,objClass.Id.ToString());
            ddlBelongClass.Items.Add(item);
        }
    }

    /// <summary>
    /// 保存条件
    /// </summary>
    private void SaveCondition() 
    {
        if (!string.IsNullOrEmpty(txtName.Text))
        {
            ViewState["vstate_Name"] = txtName.Text;
        }
        else
        {
            ViewState.Remove("vstate_Name");
        }

        if (!string.IsNullOrEmpty(ddlBelongClass.SelectedValue))
        {
            ViewState["vstate_BelongClass"] = ddlBelongClass.SelectedValue;
        }
        else 
        {
            ViewState.Remove("vstate_BelongClass");
        }
    }

    /// <summary>
    /// 填充条件
    /// </summary>
    /// <param name="lstScope"></param>
    private void FillCondition(ScopeList lstScope) 
    {
        string vsName=ViewState["vstate_Name"] as string;
        if (!string.IsNullOrEmpty(vsName)) 
        {
            lstScope.Add(School.ScStudent.Name.Like(vsName));
        }
        string vsBelongClass = ViewState["vstate_BelongClass"] as string;
        if (!string.IsNullOrEmpty(vsBelongClass))
        {
            lstScope.Add(School.ScStudent.BelongClass.Id == Convert.ToInt32(vsBelongClass));
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SaveCondition();
        Bind();
    }
    protected void gvDisplay_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DoDelete") 
        {
            int id = Convert.ToInt32(e.CommandArgument);
            ScStudentBusiness bo = new ScStudentBusiness();
            bo.DeleteById(id);
            Alert("删除完毕");
        }
    }
}
