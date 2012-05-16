using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Buffalo.Permissions.PermissionsInfo
{
    /// <summary>
    /// ��ģ��Ȩ��
    /// </summary>
    public enum BPModelItemRight
    {
        /// <summary>
        /// ��
        /// </summary>
        [Description("��")]
        None = 0,
        /// <summary>
        /// ʹ��
        /// </summary>
        [Description("ʹ��")]
        Use = 1,
        /// <summary>
        /// ����Ȩʹ��
        /// </summary>
        [Description("����Ȩʹ��")]
        Authorization = 2,
        /// <summary>
        /// ����
        /// </summary>
        [Description("����")]
        Disable = 4,
        /// <summary>
        /// ��Ȩ����ʹ��
        /// </summary>
        [Description("��Ȩ����ʹ��")]
        AuthorizationTo = 8,
        /// <summary>
        /// ����Ȩ��
        /// </summary>
        [Description("����Ȩ��")]
        All = Use | Authorization | AuthorizationTo,
    }
}
