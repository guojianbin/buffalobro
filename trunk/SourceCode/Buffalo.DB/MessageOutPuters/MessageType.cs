using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Buffalo.DB.MessageOutPuters
{
    /// <summary>
    /// ��Ϣ����
    /// </summary>
    public enum MessageType
    {
        [Description("��ѯ���ݿ�")]
        Query,
        [Description("ִ�����")]
        Execute,
        [Description("��������")]
        OtherOper,
        [Description("��ѯ����")]
        QueryCache,
        [Description("����������쳣")]
        CacheException,
    }
}
