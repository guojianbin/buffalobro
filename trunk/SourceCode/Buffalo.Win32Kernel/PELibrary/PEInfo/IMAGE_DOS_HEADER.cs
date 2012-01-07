using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo
{
    /// <summary>
    /// DOS的.EXE头部(struct _IMAGE_DOS_HEADER)
    /// </summary>
    unsafe public struct IMAGE_DOS_HEADER
    {
        /// <summary>
        /// 魔术数字(MZ头值为 0x5A4D)
        /// </summary>
        public fixed byte e_magic[2];
        /// <summary>
        /// 文件最后页的字节数
        /// </summary>
        public ushort e_cblp;
        /// <summary>
        /// 文件页数
        /// </summary>
        public ushort e_cp;
        /// <summary>
        /// 重定义元素个数
        /// </summary>
        public ushort e_crlc;
        /// <summary>
        /// 头部尺寸，以段落为单位
        /// </summary>
        public ushort e_cparhdr;
        /// <summary>
        /// 所需的最小附加段
        /// </summary>
        public ushort e_minalloc;
        /// <summary>
        /// 所需的最大附加段
        /// </summary>
        public ushort e_maxalloc;
        /// <summary>
        /// 初始的SS值(相对偏移量)
        /// </summary>
        public ushort e_ss;
        /// <summary>
        /// 初始的SP值
        /// </summary>
        public ushort e_sp;
        /// <summary>
        /// 校验和
        /// </summary>
        public ushort e_csum;
        /// <summary>
        /// 初始的IP值
        /// </summary>
        public ushort e_ip;
        /// <summary>
        /// 初始的CS值(相对偏移量)
        /// </summary>
        public ushort e_cs;
        /// <summary>
        /// 重分配表文件地址
        /// </summary>
        public ushort e_lfarlc;
        /// <summary>
        /// 覆盖号
        /// </summary>
        public ushort e_ovno;
        /// <summary>
        /// 保留字
        /// </summary>
        public fixed ushort e_res[4];
        /// <summary>
        /// OEM标识符(相对e_oeminfo)
        /// </summary>
        public ushort e_oemid;
        /// <summary>
        /// OEM信息
        /// </summary>
        public ushort e_oeminfo;
        /// <summary>
        /// 保留字2
        /// </summary>
        public fixed ushort e_res2[10];
        /// <summary>
        /// 新exe头部的文件地址
        /// </summary>
        public uint e_lfanew;

    }
}
