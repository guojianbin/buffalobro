using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.EntityInfos;

namespace Buffalobro.DB.CommBase.DataAccessBases.AliasTableMappingManagers
{
    /// <summary>
    /// 别名Reader映射
    /// </summary>
    public class AliasReaderMapping
    {
        private int _readerIndex;
        /// <summary>
        /// 所属的Reader的列索引
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
        /// 属性信息
        /// </summary>
        public EntityPropertyInfo PropertyInfo 
        {
            get 
            {
                return _propertyInfo;
            }
        }

        /// <summary>
        /// 别名Reader映射
        /// </summary>
        /// <param name="readerIndex">Reader的列索引</param>
        /// <param name="propertyInfo">属性信息</param>
        public AliasReaderMapping(int readerIndex, EntityPropertyInfo propertyInfo) 
        {
            _readerIndex = readerIndex;
            _propertyInfo = propertyInfo;
        }

    }
}
