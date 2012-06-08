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
        private static byte[] _msCache = null;
        private static Thread _updateThread;
        private static bool _isRunning=false;
        /// <summary>
        /// 当前桌面
        /// </summary>
        public static byte[] CurrentDesktop 
        {
            get 
            {
                return _msCache;
            }
        }

        /// <summary>
        /// 开始更新
        /// </summary>
        public static void StarUpdate() 
        {
            _isRunning = true;
            _updateThread = new Thread(new ThreadStart(UpdatePrintScreen));
            _updateThread.Start();
        }

        /// <summary>
        /// 刷新屏幕数据
        /// </summary>
        private static void UpdatePrintScreen()
        {
            while (_isRunning)
            {
                Image img = WindowsApplication.PrintScreen();

                _msCache = Picture.PictureToBytes(img, System.Drawing.Imaging.ImageFormat.Jpeg);

                Thread.Sleep(300);
            }
        }

        /// <summary>
        /// 开始更新
        /// </summary>
        public static void StopUpdate()
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
