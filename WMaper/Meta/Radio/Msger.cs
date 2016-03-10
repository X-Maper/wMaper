using System;
using WMaper.Base;

namespace WMaper.Meta.Radio
{
    /// <summary>
    /// 事件消息类
    /// </summary>
    public sealed class Msger
    {
        #region 变量

        // 事件对象
        private Event chan;
        // 消息内容
        private Object info;

        #endregion

        #region 属性方法

        public Event Chan
        {
            get { return this.chan; }
        }

        public Object Info
        {
            get { return this.info; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="chan">事件对象</param>
        /// <param name="info">消息内容</param>
        public Msger(Event chan, Object info)
        {
            this.chan = chan;
            this.info = info;
        }

        #endregion
    }
}