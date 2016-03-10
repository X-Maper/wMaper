using System;
using WMagic;
using WMaper.Base;
using WMaper.Meta.Param;
using WMaper.Meta.Radio;

namespace WMaper.Plug
{
    /// <summary>
    /// 比例控件
    /// </summary>
    public sealed class Scale : Decor
    {
        #region 变量

        // 控件停靠位置
        private string anchor;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Scale()
            : base()
        {
            this.anchor = "left";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option">控件配置</param>
        public Scale(Option option)
            : this()
        {
            if (!MatchUtils.IsEmpty(option))
            {
                if (option.Exist("Anchor"))
                    this.anchor = option.Fetch<string>("Anchor").ToLower();
            }
        }

        #endregion

        #region 属性方法

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
                Scale scale = null;
                {
                    if (!MatchUtils.IsEmpty(scale = drv.Widget.Scale))
                    {
                        if (!this.Equals(scale))
                        {
                            {
                                scale.Remove();
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
                            (drv.Widget.Scale = this).Redraw();
                        }
                        catch
                        {
                            drv.Widget.Scale = null;
                        }
                        finally
                        {
                            if (MatchUtils.IsEmpty(drv.Widget.Scale))
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
                            this.Redraw();
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
                        this.Handle = new Misc.View.Plug.Scale(this, 50)
                    );
                }
                // 绘制图形
                WMaper.Misc.View.Plug.Scale v_scale = this.Handle as WMaper.Misc.View.Plug.Scale;
                {
                    v_scale.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (!MatchUtils.IsEmpty(this.Handle))
                        {
                            v_scale.Build();
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
                                this.Target.Widget.Scale = null;
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