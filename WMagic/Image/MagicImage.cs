using System;
using System.Windows.Media;
using WMagic.Cache;
using WMagic.Image.Policy;
using WMagic.Thread;

namespace WMagic.Image
{
    public class MagicImage
    {
        #region 函数方法

        /// <summary>
        /// 同步载图
        /// </summary>
        /// <param name="path">数据源</param>
        /// <param name="cache">数据缓存</param>
        /// <returns>图源数据</returns>
        public static ImageSource SyncLoad(String path, MagicCache cache)
        {
            return (new SyncLoadImage(path, cache)).DownLoad();
        }

        /// <summary>
        /// 异步载图
        /// </summary>
        /// <param name="path">数据源</param>
        /// <param name="cache">数据缓存</param>
        /// <param name="token">线程令牌</param>
        /// <param name="thread">线程工厂</param>
        /// <param name="action">回调函数</param>
        public static MagicThread AsyncLoad(String path, MagicCache cache, MagicThreadFactory thread, Delegate action)
        {
            System.Threading.CancellationTokenSource drive = new System.Threading.CancellationTokenSource();
            {
                MagicThread task = new MagicThread(path, (new AsyncLoadImage(path, cache, drive.Token, thread, action)).DownLoad);
                {
                    thread.Attach(task, drive);
                }
                return task;
            }
        }

        /// <summary>
        /// 异步载图
        /// </summary>
        /// <param name="path">数据源</param>
        /// <param name="cache">数据缓存</param>
        /// <param name="token">线程令牌</param>
        /// <param name="thread">线程工厂</param>
        /// <param name="action">回调函数</param>
        /// <param name="expect">期望参数</param>
        public static MagicThread AsyncLoad(String path, MagicCache cache, MagicThreadFactory thread, Delegate action, Object[] expect)
        {
            System.Threading.CancellationTokenSource drive = new System.Threading.CancellationTokenSource();
            {
                MagicThread task = new MagicThread(path, (new AsyncLoadImage(path, cache, drive.Token, thread, action, expect)).DownLoad);
                {
                    thread.Attach(task, drive);
                }
                return task;
            }
        }

        #endregion
    }
}