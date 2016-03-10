using System.Collections;
using WMagic;

namespace WMaper.Meta.Param
{
    /// <summary>
    /// 配置类
    /// </summary>
    public sealed class Option
    {
        #region 变量

        // 配置集合
        private Hashtable config;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Option()
        {
            this.config = new Hashtable();
        }

        public Option(string key, object obj)
            : this()
        {
            this.Append(key, obj);
        }

        #endregion

        #region 静态方法

        public static Option Create()
        {
            return new Option();
        }

        public static Option Create(string key, object obj)
        {
            return new Option(key, obj);
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 添加配置对象
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="obj">对象</param>
        /// <returns>添加的配置对象</returns>
        public Option Append(string key, object obj)
        {
            if (!this.Exist(key))
            {
                this.config.Add(key, obj);
            }
            return this;
        }

        /// <summary>
        /// 移除配置对象
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>移除的配置对象</returns>
        public Option Remove(string key)
        {
            if (this.Exist(key))
            {
                this.config.Remove(key);
            }
            return this;
        }

        /// <summary>
        /// 获取配置对象
        /// </summary>
        /// <typeparam name="FUTURE">配置的类型</typeparam>
        /// <param name="key">主键</param>
        /// <returns>获取的配置对象</returns>
        public FUTURE Fetch<FUTURE>(string key)
        {
            if (this.Exist(key))
            {
                return (FUTURE)this.config[key];
            }
            return default(FUTURE);
        }

        /// <summary>
        /// 是否存在主键
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>是否存在主键</returns>
        public bool Exist(string key)
        {
            return this.config.ContainsKey(key);
        }

        /// <summary>
        /// 清空配置对象
        /// </summary>
        public void Clear()
        {
            if (!MatchUtils.IsEmpty(this.config))
            {
                this.config.Clear();
            }
        }

        #endregion
    }
}