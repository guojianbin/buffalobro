using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Buffalo.Permissions.PermissionsInfo
{
    /// <summary>
    /// 模块权限
    /// </summary>
    public enum BPModelRights
    {
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable=1,
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disable=2
    }
}
