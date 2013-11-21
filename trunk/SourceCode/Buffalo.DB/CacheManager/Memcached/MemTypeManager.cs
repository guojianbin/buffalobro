using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Buffalo.DB.CacheManager.Memcached
{
    public delegate object ReadInfo(BinaryReader reader);
    public delegate void WriteInfo(BinaryWriter writer,object value);
    /// <summary>
    /// 读写器类型管理器
    /// </summary>
    public class MemTypeManager
    {
        private static Dictionary<int, MemTypeItem> _dicMemTypeItem = null;
        private static Dictionary<string, MemTypeItem> _dicMemTypeItemByFullName = null;

        private static bool _isInit=InitTypes();

        private static bool InitTypes()
        {
            _dicMemTypeItem = new Dictionary<int, MemTypeItem>();
            _dicMemTypeItemByFullName = new Dictionary<string, MemTypeItem>();
            MemTypeItem item = null;
            AddItem<bool>(1, ReadBoolean);

            AddItem<short>(2, ReadInt16);
            AddItem<int>(3, ReadInt);
            AddItem<long>(4, ReadInt64);

            AddItem<ushort>(5, ReadUInt16);
            AddItem<uint>(6, ReadUInt);
            AddItem<ulong>(7, ReadUInt64);

            AddItem<byte>(8, ReadByte);
            AddArrayItem<byte[]>(9, ReadBytes, WriteArray<byte[]>);
            AddItem<char>(10, ReadChar);
            AddArrayItem<char[]>(11, ReadChars, WriteArray<char[]>);
            AddItem<decimal>(12, ReadDecimal);
            AddItem<double>(13, ReadDouble);
            AddItem<sbyte>(14, ReadSByte);
            AddItem<float>(15, ReadSingle);
            //AddItem<string>(2, ReadString);
            AddArrayItem<string>(16, ReadString, WriteString);
            
            
            return true;
        }

        /// <summary>
        /// 获取类型信息
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static MemTypeItem GetTypeInfo(Type objType) 
        {
            string key = objType.FullName;
            MemTypeItem item = null;
            if (_dicMemTypeItemByFullName.TryGetValue(key, out item)) 
            {
                return item;
            }
            return null;
        }
        /// <summary>
        /// 根据类型ID获取类型信息
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <returns></returns>
        public static MemTypeItem GetTypeByID(int typeID)
        {
            
            MemTypeItem item = null;
            if (_dicMemTypeItem.TryGetValue(typeID, out item))
            {
                return item;
            }
            return null;
        }
        /// <summary>
        /// 添加项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeID">分配的ID</param>
        /// <param name="readInfo">读取器</param>
        private static void AddItem<T>(int typeID, ReadInfo readInfo) 
        {
            MemTypeItem item = new MemTypeItem(typeID, typeof(T), readInfo, Write<T>);
            _dicMemTypeItem[typeID] = item;
            _dicMemTypeItemByFullName[item.ItemType.FullName] = item;
        }

        /// <summary>
        /// 添加项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeID">分配的ID</param>
        /// <param name="readInfo">读取器</param>
        private static void AddArrayItem<T>(int typeID, ReadInfo readInfo,WriteInfo writeInfo)
        {
            MemTypeItem item = new MemTypeItem(typeID, typeof(T), readInfo, writeInfo);
            _dicMemTypeItem[typeID] = item;
            _dicMemTypeItemByFullName[item.ItemType.FullName] = item;
        }
        public static object ReadBoolean(BinaryReader reader){return reader.ReadBoolean();}

        public static object ReadInt16(BinaryReader reader){return reader.ReadInt16();}
        public static object ReadInt(BinaryReader reader){return reader.ReadInt32();}
        public static object ReadInt64(BinaryReader reader){return reader.ReadInt64();}

        public static object ReadUInt16(BinaryReader reader){return reader.ReadUInt16();}
        public static object ReadUInt(BinaryReader reader){return reader.ReadUInt32();}
        public static object ReadUInt64(BinaryReader reader) { return reader.ReadUInt64(); }

        public static object ReadByte(BinaryReader reader) { return reader.ReadByte(); }
        //public static object ReadBytes(BinaryReader reader) { return reader.ReadBytes(); }
        public static object ReadChar(BinaryReader reader) { return reader.ReadChar(); }
        //public static object ReadChars(BinaryReader reader) { return reader.ReadChars(); }
        public static object ReadDecimal(BinaryReader reader) { return reader.ReadDecimal(); }
        public static object ReadDouble(BinaryReader reader) { return reader.ReadDouble(); }
        public static object ReadSByte(BinaryReader reader) { return reader.ReadSByte(); }
        public static object ReadSingle(BinaryReader reader) { return reader.ReadSingle(); }

        public static object ReadString(BinaryReader reader) 
        {
            int len = reader.ReadInt32();
            if (len == -1) 
            {
                return null;
            }
            byte[] buffer = reader.ReadBytes(len);
            return MemDataSerialize.DefaultEncoding.GetString(buffer); 
        }
        /// <summary>
        /// 读取字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static object ReadChars(BinaryReader reader)
        {
            int len = reader.ReadInt32();
            if (len == -1)
            {
                return null;
            }
            char[] buffer = reader.ReadChars(len);

            return buffer;
        }
        /// <summary>
        /// 读取字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static object ReadBytes(BinaryReader reader)
        {
            int len = reader.ReadInt32();
            if (len == -1)
            {
                return null;
            }
            byte[] buffer = reader.ReadBytes(len);

            return buffer;
        }
        

        public static object WriteString(BinaryWriter writer,object value)
        {
            string str = value as string;
            int len = 0;
            if (str == null)
            {
                len = -1;//-1时候是null;
            }
            else 
            {
                len = str.Length;
            }
            writer.Write(len);
            byte[] buffer = reader.ReadBytes(len);
            return MemDataSerialize.DefaultEncoding.GetString(buffer);
        }

        /// <summary>
        /// 写入值
        /// </summary>
        /// <param name="writer">写入器</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void WriteArray<T>(BinaryWriter writer, object value)
            where T:System.Array
        {
            writer.Write(value == null);//写入是否为空
            T val = (T)value;
            writer.Write(val.Length);
            writer.Write(val);
        }
        /// <summary>
        /// 写入值
        /// </summary>
        /// <param name="writer">写入器</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void Write<T>(BinaryWriter writer, object value)
        {
            writer.Write(value==null);//写入是否为空
            writer.Write((T)value);
        }
    }
}
