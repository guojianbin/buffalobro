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
    /// 桌面缓存
    /// </summary>
    public class DesktopCache
    {
        private byte[] _msCache = null;
        private Thread _updateThread;
        private bool _isRunning=false;
        private int _sleeptime;

        /// <summary>
        /// 屏幕桌面缓存
        /// </summary>
        /// <param name="sleeptime">截取时间间隔(毫秒)</param>
        public DesktopCache(int sleeptime) 
        {
            _sleeptime = sleeptime;
        }

        /// <summary>
        /// 当前桌面
        /// </summary>
        public byte[] CurrentDesktop 
        {
            get 
            {
                return _msCache;
            }
        }

        /// <summary>
        /// 开始更新
        /// </summary>
        public void StarUpdate() 
        {
            _isRunning = true;
            _updateThread = new Thread(new ThreadStart(UpdatePrintScreen));
            _updateThread.Start();
        }

        /// <summary>
        /// 刷新屏幕数据
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
        /// 开始更新
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
