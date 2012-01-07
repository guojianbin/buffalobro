using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalobro.DB.MessageOutPuters
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
        public bool OutPut(string messName, string messInfo) 
        {
            if (_defaultOutputer != null)
            {
                _defaultOutputer.Output(messName, messInfo);
                return true;
            }
            return false;
        }
    }
}
