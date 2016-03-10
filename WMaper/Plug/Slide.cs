using System;
using WMagic;
using WMaper.Base;
using WMaper.Meta.Param;
using WMaper.Meta.Radio;

namespace WMaper.Plug
{
    /// <summary>
    /// 缩放控件
    /// </summary>
    public sealed class Slide : Decor
    {
        #region 变量

        // 是否简单形式
        private bool simple;
        // 控件停靠位置
        private string anchor;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Slide()
            : base()
        {
            this.simple = false;
            this.anchor = "left";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option">控件配置</param>
        public Slide(Option option)
            : this()
        {
            if (!MatchUtils.IsEmpty(option))
            {
                if (option.Exist("Simple"))
                    this.simple = option.Fetch<bool>("Simple");
                if (option.Exist("Anchor"))
                    this.anchor = option.Fetch<string>("Anchor").ToLower();
            }
        }

        #endregion

        #region 属性方法

        public bool Simple
        {
            get { return this.simple; }
            set { this.simple = value; }
        }

        public string Anchor
        {
            get { return this.anchor; }
            set
            {
                if (!MatchUtils.IsEmpty(value))
                {
                    this.anchor = value.ToLower();
                }
            }
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 控件渲染函数
        /// </summary>
        /// <param name="drv">地图对象</param>
        public sealed override void Render(Maper drv)
        {
            if (!MatchUtils.IsEmpty(this.Target = drv) && drv.Enable)
            {
                Slide slide = null;
                {
                    if (!MatchUtils.IsEmpty(slide = drv.Widget.Slide))
                    {
                        if (!this.Equals(slide))
                        {
                            {
                                slide.Remove();
                            }
                            this.Render(drv);
                        }
                    }
                    else
                    {
                        // 监听广播
                        this.Observe(drv.Listen.ZoomEvent, this.Redraw, 0);
                        this.Observe(drv.Listen.SwapEvent, this.Redraw, 0);
                        // 加载控件
                        try
                        {
                            (drv.Widget.Slide = this).Redraw();
                        }
                        catch
                        {
                            drv.Widget.Slide = null;
                        }
                        finally
                        {
                            if (MatchUtils.IsEmpty(drv.Widget.Slide))
                            {
                                // 移除监听
                                this.Obscure(drv.Listen.ZoomEvent, this.Redraw);
                                this.Obscure(drv.Listen.SwapEvent, this.Redraw);
                                {
                                    this.Facade = null;
                                    this.Handle = null;
                                    this.Target = null;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 控件重绘函数
        /// </summary>
        /// <param name="msg"></param>
        public sealed override void Redraw(Msger msg)
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (MatchUtils.IsEmpty(msg))
                {
                    this.Redraw();
                }
                else
                {
                    if (!MatchUtils.IsEmpty(this.Facade) && !MatchUtils.IsEmpty(this.Handle))
                    {
                        if (this.Target.Listen.ZoomEvent.Equals(msg.Chan) || this.Target.Listen.SwapEvent.Equals(msg.Chan))
                        {
                            if (!this.simple)
                            {
                                this.Redraw();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 控件重绘函数
        /// </summary>
        public sealed override void Redraw()
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (MatchUtils.IsEmpty(this.Handle))
                {
                    this.Target.Canvas.Children.Add(
                        this.Handle = new Misc.View.Plug.Slide(this, 60)
                    );
                }
                // 绘制图形
                WMaper.Misc.View.Plug.Slide v_slide = this.Handle as WMaper.Misc.View.Plug.Slide;
                {
                    v_slide.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (!MatchUtils.IsEmpty(this.Handle))
                        {
                            v_slide.Build();
                        }
                    }));
                }
            }
        }

        /// <summary>
        /// 控件移除函数
        /// </summary>
        public sealed override void Remove()
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (this.Obscure(this.Target.Listen.ZoomEvent, this.Redraw))
                {
                    if (!this.Obscure(this.Target.Listen.SwapEvent, this.Redraw))
                    {
                        this.Observe(this.Target.Listen.ZoomEvent, this.Redraw, 0);
                    }
                    else
                    {
                        bool wipe = true;
                        try
                        {
                            this.Target.Canvas.Children.Remove(this.Handle);
                        }
                        catch
                        {
                            wipe = false;
                        }
                        finally
                        {
                            if (!wipe)
                            {
                                this.Observe(this.Target.Listen.SwapEvent, this.Redraw, 0);
                                this.Observe(this.Target.Listen.ZoomEvent, this.Redraw, 0);
                            }
                            else
                            {
                                this.Target.Widget.Slide = null;
                                {
                                    this.Facade = null;
                                    this.Handle = null;
                                    this.Target = null;
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