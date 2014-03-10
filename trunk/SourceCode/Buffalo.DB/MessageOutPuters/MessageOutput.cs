using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.MessageOutPuters
{
    /// <summary>
    /// 信息输出类
    /// </summary>
    public class MessageOutput
    {
        private static IMessageOutputer _debugOutputer = new DebugOutputer();


        private IMessageOutputer _defaultOutputer =null;

        /// <summary>
        /// 信息输出类
        /// </summary>
        public MessageOutput() 
        {
#if DEBUG
            _defaultOutputer = _debugOutputer;
#endif
        }

        /// <summary>
        /// 默认的信息输出类
        /// </summary>
        public IMessageOutputer DefaultOutputer
        {
            get { return _defaultOutputer; }
            set { _defaultOutputer = value; }
        }

        /// <summary>
        /// 判断是否有输出器
        /// </summary>
        public bool HasOutput 
        {
            get 
            {
                return _defaultOutputer != null;
            }
        }

        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="messName">信息发送者名称</param>
        /// <param name="messInfo">信息</param>
        /// <returns></returns>
        public bool OutPut(MessageType messType, MessageInfo mess) 
        {
            if (_defaultOutputer != null)
            {
                _defaultOutputer.Output( messType, mess);
                return true;
            }
            return false;
        }

       /// <summary>
        /// 输出信息
       /// </summary>
       /// <param name="messType">消息类型</param>
       /// <param name="type">类型</param>
       /// <param name="extendType">扩展类型</param>
       /// <param name="value">值</param>
       /// <returns></returns>
        public bool OutPut(MessageType messType, string type,string extendType,string value)
        {
            MessageInfo mess = new MessageInfo();
            mess[MessageInfo.Type] = type;
            if (extendType != null) 
            {
                mess[MessageInfo.ExtendType] = extendType;
            }
            mess[MessageInfo.Value] = value;
            return OutPut(messType,mess);
        }
    }
}
