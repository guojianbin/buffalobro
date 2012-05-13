using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

/// <summary>
/// UserErrorType 的摘要说明
/// </summary>
public enum UserErrorType
{
    /// <summary>
    /// 无错误
    /// </summary>
    [Description("无错误")]
    None,
    /// <summary>
    /// 用户被冻结
    /// </summary>
    [Description("用户被冻结")]
    UserForzon,
    /// <summary>
    /// 用户被锁定
    /// </summary>
    [Description("用户被锁定")]
    UserLock
}
