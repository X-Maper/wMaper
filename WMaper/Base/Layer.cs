using System;
using System.Windows.Controls;
using WMagic;
using WMaper.Meta;
using WMaper.Meta.Radio;

namespace WMaper.Base
{
    /// <summary>
    /// 图层抽象类
    /// </summary>
    public abstract class Layer
    {
        #region 变量

        // 图层标识
        private string index;
        // 图层分组
        private string group;
        // 图层标题
        private string title;
        // 图层透明
        private int alpha;
        // 是否启用
        private bool enable;
        // 目标引擎
        private Maper target;
        // 图层面板
        private Canvas facade;

        #endregion

        #region 构造函数

        public Layer()
        {
            this.alpha = 100;
            this.index = null;
            this.group = null;
            this.title = null;
            this.target = null;
            this.facade = null;
            this.enable = true;
        }

        #endregion

        #region 属性方法

        public string Index
        {
            get { return this.index; }
            set { this.index = value; }
        }

        public string Group
        {
            get { return this.group; }
            set { this.group = value; }
        }

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public int Alpha
        {
            get { return this.alpha; }
            set { this.alpha = value; }
        }

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

        #endregion

        #region 抽象函数

        /// <summary>
        /// 图层渲染函数
        /// </summary>
        /// <param name="drv">地图对象</param>
        public abstract void Render(Maper drv);

        /// <summary>
        /// 图层重绘函数
        /// </summary>
        /// <param name="msg">重绘消息</param>
        public abstract void Redraw(Msger msg);

        /// <summary>
        /// 图层重绘函数
        /// </summary>
        public abstract void Redraw();

        /// <summary>
        /// 图层移除函数
        /// </summary>
        public abstract void Remove();

        #endregion

        #region 函数方法

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="evt">事件对象</param>
        /// <param name="msg">消息对象</param>
        public void Trigger(Event evt, object msg)
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

        /// <summary>
        /// 判断图层是否在显示范围内
        /// </summary>
        /// <param name="point">显示坐标</param>
        /// <returns></returns>
        protected bool Viewble(Coord point)
        {
            return this.Viewble(null, point);
        }

        /// <summary>
        /// 判断图层是否在显示范围内
        /// </summary>
        /// <param name="arise">显示范围</param>
        /// <returns></returns>
        protected bool Viewble(Arise arise)
        {
            return this.Viewble(arise, null);
        }

        /// <summary>
        /// 判断图层是否在显示范围内
        /// </summary>
        /// <param name="arise">显示范围</param>
        /// <param name="point">显示坐标</param>
        /// <returns></returns>
        protected bool Viewble(Arise arise, Coord point)
        {
            if (!MatchUtils.IsEmpty(this.target) && !MatchUtils.IsEmpty(this.target.Netmap))
            {
                bool permit = true;
                {
                    if (!MatchUtils.IsEmpty(point))
                    {
                        Bound bound = this.target.Viewbox(false);
                        {
                            permit = !MatchUtils.IsEmpty(bound) ? bound.Contain(this.target, point) : false;
                        }
                    }
                    if (!MatchUtils.IsEmpty(arise) && permit)
                    {
                        double scale = this.target.Netmap.Deg2sc();
                        {
                            permit = scale > 0 ? (arise.Min <= 0.0 || scale <= arise.Min) && (arise.Max <= 0.0 || scale >= arise.Max) : false;
                        }
                    }
                }
                return permit;
            }
            return false;
        }

        #endregion
    }
}