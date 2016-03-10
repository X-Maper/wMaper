using System;
using System.Net;
using System.Windows.Media;
using WMagic.Cache;

namespace WMagic.Image.Policy
{
    /// <remarks>
    /// -----------------------------------------------------------------------
    /// 部件名：SyncLoadImage
    /// 工程名：WMagic
    /// 版权：CopyRight (c) 2013
    /// 创建人：ZFL
    /// 描述：同步载图类
    /// 创建日期：2013.05.27
    /// 修改人：
    /// 修改日期：
    /// -----------------------------------------------------------------------
    /// </remarks>

    /// <summary>
    /// 同步载图类
    /// </summary>
    public class SyncLoadImage
    {
        #region 变量

        // 数据源
        private String path;
        // 数据缓存
        private MagicCache cache;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path">数据源</param>
        /// <param name="cache">数据缓存</param>
        public SyncLoadImage(String path, MagicCache cache)
        {
            this.path = path;
            this.cache = cache;
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 获取图像数据
        /// </summary>
        public ImageSource DownLoad()
        {
            if (this.cache.IsExist(this.path))
            {
                return ImageUtils.Format(this.cache.Fetch(this.path) as byte[]);
            }
            else
            {
                using (WebClient http = new WebClient())
                {
                    byte[] data = http.DownloadData(new Uri(this.path));
                    if (!MatchUtils.IsEmpty(data) && this.cache.Store(this.path, data))
                    {
                        return ImageUtils.Format(data);
                    }
                }
            }
            return null;
        }

        #endregion
    }
}