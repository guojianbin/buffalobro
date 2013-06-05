using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.ClassProxyBuilder
{
    /// <summary>
    /// �������ӿ�
    /// </summary>
    public interface IInterceptor
    {
        /// <summary>
        /// ��������ǰ
        /// </summary>
        /// <param name="operationName">������</param>
        /// <param name="inputs">����</param>
        /// <returns>״̬�������ڵ��ú���</returns>
        object BeforeCall(string operationName, object[] inputs);

        /// <summary>
        /// �������ú�
        /// </summary>
        /// <param name="operationName">������</param>
        /// <param name="returnValue">���</param>
        /// <param name="correlationState">״̬����</param>
        void AfterCall(object obj, string operationName, object returnValue, object correlationState);
    }
}
