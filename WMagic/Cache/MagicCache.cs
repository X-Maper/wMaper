using System;
using System.Collections.Specialized;
using System.Runtime.Caching;

namespace WMagic.Cache
{
    /// <remarks>
    /// -----------------------------------------------------------------------
    /// 部件名：WMagicCache
    /// 工程名：WMagic
    /// 版权：CopyRight (c) 2013
    /// 创建人：ZFL
    /// 描述：缓存类
    /// 创建日期：2013.05.10
    /// 修改人：ZFL
    /// 修改日期：2013.05.13
    /// -----------------------------------------------------------------------
    /// </remarks>

    /// <summary>
    /// 缓存类
    /// </summary>
    public class MagicCache
    {
        #region 常量

        // 缓存超时
        private const string DEFAULT_POLLINGINTERVAL = "00:00:00";
        // 缓存容量
        private const int DEFAULT_CACHEMEMORYLIMITMEGABYTES = 0;
        private const int DEFAULT_PHYSICALMEMORYLIMITPERCENTAGE = 0;

        #endregion

        #region 变量

        // 缓存名称
        private string memoryCacheName;
        // 缓存实例
        private MemoryCache memoryCache;
        // 缓存策略
        private int cacheMemoryLimit;
        private int physicalMemoryLimit;
        private TimeSpan pollingInterval;
        private TimeSpan cacheItemExperied;
        private CacheItemPolicy cacheItemPolicy;

        #endregion

        #region 属性

        public int CacheMemoryLimit
        {
            get { return this.cacheMemoryLimit; }
        }

        public int PhysicalMemoryLimit
        {
            get { return this.physicalMemoryLimit; }
        }

        public TimeSpan PollingInterval
        {
            get { return this.pollingInterval; }
        }

        public TimeSpan CacheItemExperied
        {
            get { return this.cacheItemExperied; }
            set
            {
                this.cacheItemExperied = value;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">缓存名称</param>
        public MagicCache(string name)
            : this(name, DEFAULT_CACHEMEMORYLIMITMEGABYTES, DEFAULT_PHYSICALMEMORYLIMITPERCENTAGE, TimeSpan.Parse(DEFAULT_POLLINGINTERVAL))
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">缓存名称</param>
        /// <param name="cacheMemoryLimit">缓存最大内存大小</param>
        /// <param name="physicalMemoryLimit">缓存最大物理内存百分比</param>
        /// <param name="pollingInterval">缓存内存检测时间间隔</param>
        public MagicCache(string name, int cacheMemoryLimit, int physicalMemoryLimit, TimeSpan pollingInterval)
            : this(name, cacheMemoryLimit, physicalMemoryLimit, pollingInterval, ObjectCache.NoSlidingExpiration)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">缓存名称</param>
        /// <param name="cacheMemoryLimit">缓存最大内存大小</param>
        /// <param name="physicalMemoryLimit">缓存最大物理内存百分比</param>
        /// <param name="pollingInterval">缓存内存检测时间间隔</param>
        /// <param name="cacheItemExperied">缓存逐出最大时间间隔</param>
        public MagicCache(string name, int cacheMemoryLimit, int physicalMemoryLimit, TimeSpan pollingInterval, TimeSpan cacheItemExperied)
        {
            // 校验缓存名称
            if (!MatchUtils.IsEmpty(this.memoryCacheName = name))
            {
                // 缓存策略配置
                NameValueCollection memoryCacheConfig = new NameValueCollection();
                {
                    memoryCacheConfig.Add("CacheMemoryLimitMegabytes", (this.cacheMemoryLimit = cacheMemoryLimit) + "");
                    memoryCacheConfig.Add("PhysicalMemoryLimitPercentage", (this.physicalMemoryLimit = physicalMemoryLimit) + "");
                    memoryCacheConfig.Add("PollingInterval", (this.pollingInterval = pollingInterval) + "");
                }
                // 初始化缓存
                this.memoryCache = new MemoryCache(this.memoryCacheName, memoryCacheConfig);
                {
                    (this.cacheItemPolicy = new CacheItemPolicy()).SlidingExpiration = (
                        this.cacheItemExperied = cacheItemExperied
                    );
                }
            }
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 是否存在缓存
        /// </summary>
        /// <param name="key">缓存关键字</param>
        /// <returns>是否存在</returns>
        public bool IsExist(string key)
        {
            return this.memoryCache.Contains(key);
        }

        /// <summary>
        /// 获取缓存数量
        /// </summary>
        /// <returns>缓存数量</returns>
        public long Count()
        {
            return this.memoryCache.GetCount();
        }

        /// <summary>
        /// 添加指定缓存
        /// </summary>
        /// <param name="key">缓存关键字</param>
        /// <param name="value">缓存数据</param>
        /// <returns>是否成功</returns>
        public bool Store(string key, Object value)
        {
            return !MatchUtils.IsEmpty(key) ? this.memoryCache.Add(key, value, this.cacheItemPolicy, null) : false;
        }

        /// <summary>
        /// 擦除指定缓存
        /// </summary>
        /// <param name="key">缓存关键字</param>
        /// <returns>指定缓存</returns>
        public Object Erase(string key)
        {
            return this.IsExist(key) ? this.memoryCache.Remove(key, null) : null;
        }

        /// <summary>
        /// 获取指定缓存
        /// </summary>
        /// <param name="key">缓存关键字</param>
        /// <returns>指定缓存</returns>
        public Object Fetch(string key)
        {
            return this.IsExist(key) ? this.memoryCache.GetCacheItem(key).Value : null;
        }

        /// <summary>
        /// 销毁所有缓存
        /// </summary>
        public void Destroy()
        {
            if (this.memoryCache != null)
            {
                try
                {
                    this.memoryCache.Dispose();
                }
                catch
                {
                    this.memoryCache = null;
                }
                finally
                {
                    this.memoryCache = null;
                }
            }
        }

        #endregion
    }
}