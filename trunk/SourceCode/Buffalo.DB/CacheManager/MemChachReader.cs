using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// �ڴ������ж�ȡ��
    /// </summary>
    public class MemChachReader : IDataReader
    {
        /// <summary>
        /// ����
        /// </summary>
        private DataSet _data;
        /// <summary>
        /// ��ǰ��������
        /// </summary>
        private int _currentIndex = -1;
        /// <summary>
        /// ��ǰ���ݱ�
        /// </summary>
        private DataTable _currentData;
        /// <summary>
        /// ��ǰ���ݱ������
        /// </summary>
        private int _currentRowIndex=0;
        /// <summary>
        /// ��ǰ��
        /// </summary>
        private DataRow _currentRow;

        public MemChachReader(DataSet ds) 
        {
            _data = ds;
        }

        #region IDataReader ��Ա

        public void Close()
        {
            
        }

        public int Depth
        {
            get { return 1; }
        }

        public DataTable GetSchemaTable()
        {
            return null;
        }

        public bool IsClosed
        {
            get { return false; }
        }

        public bool NextResult()
        {
            _currentIndex++;
            _currentData = _data.Tables[_currentIndex];
            _currentRowIndex = -1;
            _currentRow = null;
        }

        public bool Read()
        {
            _currentRowIndex++;
            _currentRow = _currentData.Rows[_currentRowIndex];
        }

        public int RecordsAffected
        {
            get
            {
                return 0;
            }
        }

        #endregion

        #region IDisposable ��Ա

        public void Dispose()
        {
            
        }

        #endregion

        #region IDataRecord ��Ա

        public int FieldCount
        {
            get 
            {
                _currentData.Columns.Count;
            }
        }

        public bool GetBoolean(int i)
        {
            return (bool)_currentRow[i];
        }

        public byte GetByte(int i)
        {
            return (byte)_currentRow[i];
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            byte[] arr = (byte[])_currentRow[i];
            Array.Copy(arr, (int)fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i)
        {
            return (char)_currentRow[i];
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            char[] arr = (char[])_currentRow[i];
            Array.Copy(arr, (int)fieldOffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetDataTypeName(int i)
        {
            return _currentData.Columns[i].DataType.FullName;
        }

        public DateTime GetDateTime(int i)
        {
            return (DateTime)_currentRow[i];
        }

        public decimal GetDecimal(int i)
        {
            return (decimal)_currentRow[i];
        }

        public double GetDouble(int i)
        {
            return (double)_currentRow[i];
        }

        public Type GetFieldType(int i)
        {
            return _currentData.Columns[i].DataType;
        }

        public float GetFloat(int i)
        {
            return (float)_currentRow[i];
        }

        public Guid GetGuid(int i)
        {
            return (Guid)_currentRow[i];
        }

        public short GetInt16(int i)
        {
            return (short)_currentRow[i];
        }

        public int GetInt32(int i)
        {
            return (int)_currentRow[i];
        }

        public long GetInt64(int i)
        {
            return (long)_currentRow[i];
        }

        public string GetName(int i)
        {
            return _currentData.Columns[i].ColumnName;
        }

        public int GetOrdinal(string name)
        {
            for (int i = 0; i < _currentData.Columns.Count; i++)
            {
                DataColumn col = _currentData.Columns[i];
                if (col.ColumnName == name) 
                {
                    return i;
                }
            }
            return -1;
        }

        public string GetString(int i)
        {
            return (string)_currentRow[i];
        }

        public object GetValue(int i)
        {
            return _currentRow[i];
        }

        public int GetValues(object[] values)
        {
            for (int i = 0; i < _currentData.Columns.Count; i++) 
            {
                if (i >= values.Length) 
                {
                    return values.Length;
                }
                values[i] = _currentRow[i];
            }
            return _currentData.Columns.Count;
        }

        public bool IsDBNull(int i)
        {
            return _currentRow[i] is DBNull;
        }

        public object this[string name]
        {
            get
            {
                return _currentRow[name];
            }
        }

        public object this[int i]
        {
            get 
            {
                return _currentRow[i];
            }
        }

        #endregion
    }
}
