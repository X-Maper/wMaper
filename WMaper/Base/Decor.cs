using System;
using System.Windows.Controls;
using WMaper.Meta.Radio;

namespace WMaper.Base
{
    /// <summary>
    /// 控件抽象类
    /// </summary>
    public abstract class Decor
    {
        #region 变量

        // 是否启用
        private bool enable;
        // 目标引擎
        private Maper target;
        // 控件面板
        private Canvas facade;
        // 控件句柄
        private UserControl handle;

        #endregion

        #region 构造函数

        public Decor()
        {
            this.target = null;
            this.facade = null;
            this.handle = null;
            this.enable = true;
        }

        #endregion

        #region 属性方法

        public bool Enable
        {
            get { return this.enable; }
            set { this.enable = value; }
        }

        public Maper Target
        {
            get { return this.target; }
            set { this.target = value; }
        }

        public Canvas Facade
        {
            get { return this.facade; }
            set { this.facade = value; }
        }

        public UserControl Handle
        {
            get { return this.handle; }
            protected set { this.handle = value; }
        }

        #endregion

        #region 抽象函数

        /// <summary>
        /// 控件渲染函数
        /// </summary>
        /// <param name="drv">地图对象</param>
        public abstract void Render(Maper drv);

        /// <summary>
        /// 控件重绘函数
        /// </summary>
        /// <param name="msg">重绘消息</param>
        public abstract void Redraw(Msger msg);

        /// <summary>
        /// 控件重绘函数
        /// </summary>
        public abstract void Redraw();

        /// <summary>
        /// 控件移除函数
        /// </summary>
        public abstract void Remove();

        #endregion

        #region 函数方法

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="evt">事件对象</param>
        /// <param name="msg">消息对象</param>
        public void Trigger(Event evt, Object msg)
        {
            evt.Broadcast(msg);
        }

        /// <summary>
        /// 事件屏蔽
        /// </summary>
        /// <param name="evt">事件对象</param>
        /// <param name="fun">回调函数</param>
        /// <returns></returns>
        public bool Obscure(Event evt, Action<Msger> fun)
        {
            return evt.Detach(fun);
        }

        /// <summary>
        /// 事件观察
        /// </summary>
        /// <param name="evt">事件对象</param>
        /// <param name="fun">回调函数</param>
        /// <returns></returns>
        public bool Observe(Event evt, Action<Msger> fun)
        {
            return evt.Attach(fun);
        }

        /// <summary>
        /// 事件观察
        /// </summary>
        /// <param name="evt">事件对象</param>
        /// <param name="fun">回调函数</param>
        /// <param name="low">优先级</param>
        /// <returns></returns>
        public bool Observe(Event evt, Action<Msger> fun, int low)
        {
            return evt.Attach(fun, low);
        }

        #endregion
    }
}