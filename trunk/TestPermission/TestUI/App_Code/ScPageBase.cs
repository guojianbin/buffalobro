using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TestPerLib;
using Buffalo.WebKernel.WebCommons.PageBases;

/// <summary>
/// ScPageBase 的摘要说明
/// </summary>
public class ScPageBase : PageBase<ScEmployee>
{
    public ScPageBase()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    protected override void OnLoad(EventArgs e)
    {
        
        base.OnLoad(e);
    }

    /// <summary>
    /// 获取数据的行数
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <param name="currentPage"></param>
    /// <returns></returns>
    public long GetDataIndex(int rowIndex, long currentPage)
    {
        return rowIndex + (PageSize * currentPage) + 1;
    }
    /// <summary>
    /// 分页大小
    /// </summary>
    public virtual int PageSize 
    {
        get 
        {
            return 10;
        }
    }

}
