using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.EntityInfos;

namespace Buffalobro.DB.CommBase.DataAccessBases.AliasTableMappingManagers
{
    /// <summary>
    /// ����Readerӳ��
    /// </summary>
    public class AliasReaderMapping
    {
        private int _readerIndex;
        /// <summary>
        /// ������Reader��������
        /// </summary>
        public int ReaderIndex 
        {
            get 
            {
                return _readerIndex;
            }
        }

        private EntityPropertyInfo _propertyInfo;

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public EntityPropertyInfo PropertyInfo 
        {
            get 
            {
                return _propertyInfo;
            }
        }

        /// <summary>
        /// ����Readerӳ��
        /// </summary>
        /// <param name="readerIndex">Reader��������</param>
        /// <param name="propertyInfo">������Ϣ</param>
        public AliasReaderMapping(int readerIndex, EntityPropertyInfo propertyInfo) 
        {
            _readerIndex = readerIndex;
            _propertyInfo = propertyInfo;
        }

    }
}
