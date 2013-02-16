using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// 文件生成类型
    /// </summary>
    public enum BuildAction
    {
        /// <summary>
        /// 代码
        /// </summary>
        Code=1,
        /// <summary>
        /// 嵌入资源
        /// </summary>
        Resource=3,
        /// <summary>
        /// 普通文件
        /// </summary>
        File=2
    }
}
