using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo
{
    /// <summary>
    /// DOS��.EXEͷ��(struct _IMAGE_DOS_HEADER)
    /// </summary>
    unsafe public struct IMAGE_DOS_HEADER
    {
        /// <summary>
        /// ħ������(MZͷֵΪ 0x5A4D)
        /// </summary>
        public fixed byte e_magic[2];
        /// <summary>
        /// �ļ����ҳ���ֽ���
        /// </summary>
        public ushort e_cblp;
        /// <summary>
        /// �ļ�ҳ��
        /// </summary>
        public ushort e_cp;
        /// <summary>
        /// �ض���Ԫ�ظ���
        /// </summary>
        public ushort e_crlc;
        /// <summary>
        /// ͷ���ߴ磬�Զ���Ϊ��λ
        /// </summary>
        public ushort e_cparhdr;
        /// <summary>
        /// �������С���Ӷ�
        /// </summary>
        public ushort e_minalloc;
        /// <summary>
        /// �������󸽼Ӷ�
        /// </summary>
        public ushort e_maxalloc;
        /// <summary>
        /// ��ʼ��SSֵ(���ƫ����)
        /// </summary>
        public ushort e_ss;
        /// <summary>
        /// ��ʼ��SPֵ
        /// </summary>
        public ushort e_sp;
        /// <summary>
        /// У���
        /// </summary>
        public ushort e_csum;
        /// <summary>
        /// ��ʼ��IPֵ
        /// </summary>
        public ushort e_ip;
        /// <summary>
        /// ��ʼ��CSֵ(���ƫ����)
        /// </summary>
        public ushort e_cs;
        /// <summary>
        /// �ط�����ļ���ַ
        /// </summary>
        public ushort e_lfarlc;
        /// <summary>
        /// ���Ǻ�
        /// </summary>
        public ushort e_ovno;
        /// <summary>
        /// ������
        /// </summary>
        public fixed ushort e_res[4];
        /// <summary>
        /// OEM��ʶ��(���e_oeminfo)
        /// </summary>
        public ushort e_oemid;
        /// <summary>
        /// OEM��Ϣ
        /// </summary>
        public ushort e_oeminfo;
        /// <summary>
        /// ������2
        /// </summary>
        public fixed ushort e_res2[10];
        /// <summary>
        /// ��exeͷ�����ļ���ַ
        /// </summary>
        public uint e_lfanew;

    }
}
