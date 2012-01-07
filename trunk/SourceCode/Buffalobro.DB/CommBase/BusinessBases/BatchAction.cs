using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.DbCommon;
using Buffalobro.DB.MessageOutPuters;

namespace Buffalobro.DB.CommBase.BusinessBases
{
    /// <summary>
    /// ���ݿ����������
    /// </summary>
    public class BatchAction : IDisposable
    {

        /// <summary>
        /// ���ͷ�������
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="runnow"></param>
        internal BatchAction(DataBaseOperate oper) 
        {
            if (oper.CommitState == CommitState.AutoCommit)
            {
#if DEBUG
                IMessageOutputer outputer=oper.DBInfo.SqlOutputer.DefaultOutputer;
                if ( outputer!= null) 
                {
                    outputer.Output("SQLCommon", oper.DBInfo.Name + "   StarBatchAction");
                }
#endif
                _state = oper.CommitState;
                oper.CommitState = CommitState.UserCommit;
                _oper = oper;
            }
        }


        private DataBaseOperate _oper;
        CommitState _state;

        /// <summary>
        /// �Ƿ�ǰ����
        /// </summary>
        public bool Runnow
        {
            get { return _oper!=null; }
        }

       

        #region IDisposable ��Ա

        /// <summary>
        /// �ͷ�����
        /// </summary>
        public void Dispose()
        {
            EndBatch();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// ������������
        /// </summary>
        private void EndBatch() 
        {
            if (_oper!=null)
            {
#if DEBUG
                IMessageOutputer outputer=_oper.DBInfo.SqlOutputer.DefaultOutputer;
                if (outputer != null)
                {
                    outputer.Output("SQLCommon", _oper.DBInfo.Name + "   EndBatchAction");
                }
#endif
                _oper.CommitState = _state;
                _oper.AutoClose();
            }
        }
        #endregion
    }
}
