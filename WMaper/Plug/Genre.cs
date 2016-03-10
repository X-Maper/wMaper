using System;
using WMagic;
using WMaper.Base;
using WMaper.Meta.Param;
using WMaper.Meta.Radio;

namespace WMaper.Plug
{
    public sealed class Genre : Decor
    {
        #region 变量

        // 控件停靠位置
        private string anchor;

        #endregion

        #region 构造函数

        public Genre()
            : base()
        {
            this.anchor = "right";
        }

        public Genre(Option option)
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

        #region 重载方法

        public sealed override void Render(Maper drv)
        {
            if (!MatchUtils.IsEmpty(this.Target = drv) && drv.Enable)
            {
                Genre genre = null;
                {
                    if (!MatchUtils.IsEmpty(genre = drv.Widget.Genre))
                    {
                        if (!this.Equals(genre))
                        {
                            {
                                genre.Remove();
                            }
                            this.Render(drv);
                        }
                    }
                    else
                    {
                        // 监听广播
                        this.Observe(drv.Listen.SwapEvent, this.Redraw, 0);
                        // 加载控件
                        try
                        {
                            (drv.Widget.Genre = this).Redraw();
                        }
                        catch
                        {
                            drv.Widget.Genre = null;
                        }
                        finally
                        {
                            if (MatchUtils.IsEmpty(drv.Widget.Genre))
                            {
                                // 移除监听
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
                        if (this.Target.Listen.SwapEvent.Equals(msg.Chan))
                        {
                            this.Redraw();
                        }
                    }
                }
            }
        }

        public sealed override void Redraw()
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (MatchUtils.IsEmpty(this.Handle))
                {
                    this.Target.Canvas.Children.Add(
                        this.Handle = new Misc.View.Plug.Genre(this, 80)
                    );
                }
                // 绘制图形
                WMaper.Misc.View.Plug.Genre v_genre = this.Handle as WMaper.Misc.View.Plug.Genre;
                {
                    v_genre.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (!MatchUtils.IsEmpty(this.Handle))
                        {
                            v_genre.Build();
                        }
                    }));
                }
            }
        }

        public sealed override void Remove()
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (this.Obscure(this.Target.Listen.SwapEvent, this.Redraw))
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
                        }
                        else
                        {
                            this.Target.Widget.Genre = null;
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

        #endregion
    }
}