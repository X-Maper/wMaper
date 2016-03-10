using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WMagic;
using WMaper.Base;
using WMaper.Meta;
using WMaper.Meta.Param;

namespace WMaper.Core
{
    public sealed class Tips : Layer
    {
        #region 变量

        // 元素句柄
        private UserControl handle;
        // 是否可见
        private bool matte;
        // 显示范围
        private Arise arise;
        // 标题颜色
        private Color color;
        // 提示尺寸
        private Gauge gauge;
        // 提示漂移
        private Drift drift;
        // 提示位置
        private Coord point;
        // 提示内容 
        private Object quote;
        // 显示光标
        private Cursor mouse;
        // 提示销毁函数
        private Action scrap;
        // 标题字体
        private WMaper.Meta.Fonts fonts;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Tips()
            : base()
        {
            this.handle = null;
            this.matte = false;
            this.Alpha = 85;
            this.arise = null;
            this.quote = null;
            this.fonts = null;
            this.gauge = null;
            this.drift = null;
            this.point = null;
            this.scrap = null;
            this.color = Color.FromRgb(204, 85, 34);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public Tips(Option option)
            : this()
        {
            if (!MatchUtils.IsEmpty(option))
            {
                // 用户配置
                if (option.Exist("Index"))
                    this.Index = option.Fetch<string>("Index");
                if (option.Exist("Group"))
                    this.Group = option.Fetch<string>("Group");
                if (option.Exist("Title"))
                    this.Title = option.Fetch<string>("Title");
                if (option.Exist("Mouse"))
                    this.Mouse = option.Fetch<Cursor>("Mouse");
                if (option.Exist("Matte"))
                    this.matte = option.Fetch<bool>("Matte");
                if (option.Exist("Alpha"))
                    this.Alpha = option.Fetch<int>("Alpha");
                if (option.Exist("Arise"))
                    this.Arise = option.Fetch<Arise>("Arise");
                if (option.Exist("Color"))
                    this.Color = option.Fetch<Color>("Color");
                if (option.Exist("Gauge"))
                    this.Gauge = option.Fetch<Gauge>("Gauge");
                if (option.Exist("Drift"))
                    this.Drift = option.Fetch<Drift>("Drift");
                if (option.Exist("Point"))
                    this.Point = option.Fetch<Coord>("Point");
                if (option.Exist("Quote"))
                    this.Quote = option.Fetch<Object>("Quote");
                if (option.Exist("Scrap"))
                    this.Scrap = option.Fetch<Action>("Scrap");
                if (option.Exist("Fonts"))
                    this.Fonts = option.Fetch<WMaper.Meta.Fonts>("Fonts");
            }
        }

        #endregion

        #region 属性方法

        public UserControl Handle
        {
            get { return this.handle; }
        }

        public bool Matte
        {
            get { return this.matte; }
            set { this.matte = value; }
        }

        public Arise Arise
        {
            get { return this.arise; }
            set { this.arise = value; }
        }

        public Color Color
        {
            get { return this.color; }
            set { this.color = value; }
        }

        public Gauge Gauge
        {
            get { return this.gauge; }
            set { this.gauge = value; }
        }

        public Drift Drift
        {
            get { return this.drift; }
            set { this.drift = value; }
        }

        public Coord Point
        {
            get { return this.point; }
            set { this.point = value; }
        }

        public Object Quote
        {
            get { return this.quote; }
            set { this.quote = value; }
        }

        public Cursor Mouse
        {
            get { return this.mouse; }
            set { this.mouse = value; }
        }

        public Action Scrap
        {
            get { return this.scrap; }
            set { this.scrap = value; }
        }

        public WMaper.Meta.Fonts Fonts
        {
            get { return this.fonts; }
            set { this.fonts = value; }
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 提示置前
        /// </summary>
        public void Summit()
        {
            if (!MatchUtils.IsEmpty(this.Target) && !MatchUtils.IsEmpty(this.Facade) && this.Target.Enable && this.Enable)
            {
                this.Target.Vessel.Tips.Children.Add(this.handle);
            }
        }

        /// <summary>
        /// 提示渲染函数
        /// </summary>
        /// <param name="drv"></param>
        public sealed override void Render(Maper drv)
        {
            if (!MatchUtils.IsEmpty(this.Target = drv) && this.Target.Enable)
            {
                this.Index = MatchUtils.IsEmpty(this.Index) ? (this.Index = StampUtils.GetTimeStamp()) : this.Index;
                if (drv.Symbol.Tips.ContainsKey(this.Index))
                {
                    Tips tips = drv.Symbol.Tips[this.Index];
                    if (this.Equals(tips))
                    {
                        this.Redraw();
                    }
                    else
                    {
                        {
                            tips.Remove();
                        }
                        this.Render(drv);
                    }
                }
                else
                {
                    // 监听广播
                    this.Observe(drv.Listen.DragEvent, this.Redraw);
                    this.Observe(drv.Listen.ZoomEvent, this.Redraw);
                    this.Observe(drv.Listen.SwapEvent, this.Redraw);
                    // 绘制图形
                    drv.Symbol.Tips.Add(this.Index, this);
                    try
                    {
                        this.Redraw();
                    }
                    catch
                    {
                        drv.Symbol.Tips.Remove(this.Index);
                    }
                    finally
                    {
                        if (!drv.Symbol.Tips.ContainsKey(this.Index))
                        {
                            // 移除监听
                            this.Obscure(drv.Listen.DragEvent, this.Redraw);
                            this.Obscure(drv.Listen.ZoomEvent, this.Redraw);
                            this.Obscure(drv.Listen.SwapEvent, this.Redraw);
                            {
                                this.Facade = null;
                                this.handle = null;
                                this.Target = null;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 重绘提示
        /// </summary>
        /// <param name="msg"></param>
        public sealed override void Redraw(WMaper.Meta.Radio.Msger msg)
        {
            if (!MatchUtils.IsEmpty(this.Target) && !MatchUtils.IsEmpty(this.Target.Netmap) && this.Target.Enable && this.Enable)
            {
                if (MatchUtils.IsEmpty(msg))
                {
                    this.Redraw();
                }
                else
                {
                    if (!MatchUtils.IsEmpty(this.Facade) && !MatchUtils.IsEmpty(this.handle))
                    {
                        if (this.Target.Listen.DragEvent.Equals(msg.Chan) || this.Target.Listen.ZoomEvent.Equals(msg.Chan) || (
                            this.Target.Listen.SwapEvent.Equals(msg.Chan) && !(msg.Info as Geog).Cover
                        ))
                        {
                            this.handle.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (!MatchUtils.IsEmpty(this.handle))
                                {
                                    this.handle.Visibility = (
                                        this.matte || !this.Viewble(this.arise, this.point) ? Visibility.Collapsed : Visibility.Visible
                                    );
                                    {
                                        (this.handle as WMaper.Misc.View.Core.Tips).Adjust();
                                    }
                                }
                            }));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 重绘提示
        /// </summary>
        public sealed override void Redraw()
        {
            if (!MatchUtils.IsEmpty(this.Target) && !MatchUtils.IsEmpty(this.Target.Netmap) && this.Target.Enable && this.Enable)
            {
                if (MatchUtils.IsEmpty(this.handle))
                {
                    this.Target.Vessel.Tips.Children.Add(
                        this.handle = new Misc.View.Core.Tips(this)
                    );
                }
                // 绘制图形
                this.handle.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (!MatchUtils.IsEmpty(this.handle))
                    {
                        this.handle.Visibility = (
                            this.matte || !this.Viewble(this.arise, this.point) ? Visibility.Collapsed : Visibility.Visible
                        );
                        {
                            (this.handle as WMaper.Misc.View.Core.Tips).Build();
                        }
                    }
                }));
            }
        }

        /// <summary>
        /// 移除提示框
        /// </summary>
        public sealed override void Remove()
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (this.Obscure(this.Target.Listen.DragEvent, this.Redraw))
                {
                    if (!this.Obscure(this.Target.Listen.ZoomEvent, this.Redraw))
                    {
                        this.Observe(this.Target.Listen.DragEvent, this.Redraw);
                    }
                    else
                    {
                        if (!this.Obscure(this.Target.Listen.SwapEvent, this.Redraw))
                        {
                            this.Observe(this.Target.Listen.DragEvent, this.Redraw);
                            this.Observe(this.Target.Listen.ZoomEvent, this.Redraw);
                        }
                        else
                        {
                            if (!this.Target.Symbol.Tips.Remove(this.Index))
                            {
                                this.Observe(this.Target.Listen.DragEvent, this.Redraw);
                                this.Observe(this.Target.Listen.ZoomEvent, this.Redraw);
                                this.Observe(this.Target.Listen.SwapEvent, this.Redraw);
                            }
                            else
                            {
                                this.Target.Vessel.Tips.Children.Remove(this.handle);
                                {
                                    this.Facade = null;
                                    this.handle = null;
                                    this.Target = null;
                                }
                                // 回调事件
                                try
                                {
                                    if (!MatchUtils.IsEmpty(this.scrap))
                                    {
                                        this.scrap.Invoke();
                                    }
                                }
                                catch
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}