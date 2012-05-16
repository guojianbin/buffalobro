using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Buffalo.Permissions.PermissionsInfo
{
    /// <summary>
    /// 子模块权限
    /// </summary>
    public enum BPModelItemRight
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 使用
        /// </summary>
        [Description("使用")]
        Use = 1,
        /// <summary>
        /// 被授权使用
        /// </summary>
        [Description("被授权使用")]
        Authorization = 2,
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disable = 4,
        /// <summary>
        /// 授权他人使用
        /// </summary>
        [Description("授权他人使用")]
        AuthorizationTo = 8,
        /// <summary>
        /// 所有权限
        /// </summary>
        [Description("所有权限")]
        All = Use | Authorization | AuthorizationTo,
    }
}
