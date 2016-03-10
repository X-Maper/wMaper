using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Media.Imaging;
using WMagic.Cache;
using WMagic.Thread;

namespace WMagic.Image.Policy
{
    /// <remarks>
    /// -----------------------------------------------------------------------
    /// 部件名：AsyncLoadImage
    /// 工程名：WMagic
    /// 版权：CopyRight (c) 2013
    /// 创建人：ZFL
    /// 描述：异步载图类
    /// 创建日期：2013.05.16
    /// 修改人：ZFL
    /// 修改日期：2013.05.27
    /// -----------------------------------------------------------------------
    /// </remarks>

    /// <summary>
    /// 异步载图类
    /// </summary>
    public class AsyncLoadImage
    {

        #region 变量

        // 数据源
        private String path;
        // 期望参数
        private Object[] expect;
        // 异步回调
        private Delegate action;
        // 数据缓存
        private MagicCache cache;
        // 线程令牌
        private CancellationToken token;
        // 线程句柄
        private MagicThreadFactory thread;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path">数据源</param>
        /// <param name="cache">数据缓存</param>
        /// <param name="action">回调函数</param>
        public AsyncLoadImage(String path, MagicCache cache, CancellationToken token, MagicThreadFactory thread, Delegate action)
            : this(path, cache, token, thread, action, null)
        { }

        public AsyncLoadImage(String path, MagicCache cache, CancellationToken token, MagicThreadFactory thread, Delegate action, Object[] expect)
        {
            this.path = path;
            this.cache = cache;
            this.token = token;
            this.thread = thread;
            this.action = action;
            this.expect = expect;
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 获取图像数据
        /// </summary>
        public void DownLoad()
        {
            if (!this.token.IsCancellationRequested)
            {
                if (this.cache.IsExist(this.path))
                {
                    Object[] result = { ImageUtils.Format(this.cache.Fetch(this.path) as byte[]) };
                    if (MatchUtils.IsEmpty(this.expect))
                    {
                        this.action.DynamicInvoke(result);
                    }
                    else
                    {
                        this.action.DynamicInvoke(result.Concat(this.expect).ToArray());
                    }
                }
                else
                {
                    using (WebClient http = new WebClient())
                    {
                        http.Headers.Set("User-Agent", "Windows NT");
                        {
                            this.token.Register(() =>
                            {
                                this.thread.Attach(new MagicThread(StampUtils.GetSequence(), http.CancelAsync));
                            });
                            // 注册回调
                            {
                                http.DownloadDataCompleted += new DownloadDataCompletedEventHandler(this.WebClient_DownLoadDataCompleted);
                            }
                            http.DownloadDataAsync(new Uri(this.path));
                        }
                    }
                }
            }
        }

        private void WebClient_DownLoadDataCompleted(Object sender, DownloadDataCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                BitmapImage result = null;
                {
                    if (e.Error == null)
                    {
                        byte[] icon = e.Result;
                        if (!MatchUtils.IsEmpty(icon) && this.cache.Store(this.path, icon))
                        {
                            result = ImageUtils.Format(icon);
                        }
                    }
                }
                // 回调结果
                if (MatchUtils.IsEmpty(this.expect))
                {
                    this.action.DynamicInvoke(new Object[] { result });
                }
                else
                {
                    this.action.DynamicInvoke(new Object[] { result }.Concat(this.expect).ToArray());
                }
            }
        }

        #endregion
    }
}