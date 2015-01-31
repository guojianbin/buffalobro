using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Buffalo.Storage.LocalFileManager
{
    /// <summary>
    /// �����ļ�������
    /// </summary>
    public class LocalFileAdapter
    {
        /// <summary>
        /// �洢��Ŀ¼
        /// </summary>
        private string _fileRoot;
        /// <summary>
        /// �û���
        /// </summary>
        private string _userName;

        /// <summary>
        /// ����
        /// </summary>
        private string _password;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public LocalFileAdapter(string fileName, string userName, string password) 
        {
            _fileRoot = fileName;
            _userName = userName;
            _password = password;
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public void Open() 
        {
            if (string.IsNullOrEmpty(_userName)) 
            {
                return;
            }
            uint resault=FileAPI.WNetAddConnection(_userName, _password, _fileRoot, null);

        }

        /// <summary>
        /// �ر�����
        /// </summary>
        public void Close() 
        {
            if (string.IsNullOrEmpty(_userName))
            {
                return;
            }
            uint resault = FileAPI.WNetCancelConnection(_fileRoot, 1, true); //ȡ��ӳ��
            
        }
        /// <summary>
        /// ��ȡ�����ļ�
        /// </summary>
        /// <returns></returns>
        public List<string> ListFiles() 
        {
            string[] files = Directory.GetFiles(_fileRoot);
            return new List<string>(files);
        }
    }
}
