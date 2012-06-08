using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using Buffalo.Win32Kernel.Win32;
using Buffalo.Kernel;
using System.Threading;

namespace BroadcastDesktop
{
    /// <summary>
    /// ���滺��
    /// </summary>
    public class DesktopCache
    {
        private byte[] _msCache = null;
        private Thread _updateThread;
        private bool _isRunning=false;
        private int _sleeptime;

        /// <summary>
        /// ��Ļ���滺��
        /// </summary>
        /// <param name="sleeptime">��ȡʱ����(����)</param>
        public DesktopCache(int sleeptime) 
        {
            _sleeptime = sleeptime;
        }

        /// <summary>
        /// ��ǰ����
        /// </summary>
        public byte[] CurrentDesktop 
        {
            get 
            {
                return _msCache;
            }
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        public void StarUpdate() 
        {
            _isRunning = true;
            _updateThread = new Thread(new ThreadStart(UpdatePrintScreen));
            _updateThread.Start();
        }

        /// <summary>
        /// ˢ����Ļ����
        /// </summary>
        private void UpdatePrintScreen()
        {
            while (_isRunning)
            {
                Image img = WindowsApplication.PrintScreen();

                _msCache = Picture.PictureToBytes(img, System.Drawing.Imaging.ImageFormat.Jpeg);

                Thread.Sleep(_sleeptime);
            }
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        public void StopUpdate()
        {
            _isRunning = false;
            if (_updateThread != null)
            {
                _updateThread.Abort();
                _updateThread = null;
                Thread.Sleep(100);
            }
        }
    }
}
