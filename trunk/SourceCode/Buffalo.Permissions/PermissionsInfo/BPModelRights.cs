using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Buffalo.Permissions.PermissionsInfo
{
    /// <summary>
    /// ģ��Ȩ��
    /// </summary>
    public enum BPModelRights
    {
        /// <summary>
        /// ����
        /// </summary>
        [Description("����")]
        Enable=1,
        /// <summary>
        /// ����
        /// </summary>
        [Description("����")]
        Disable=2
    }
}
