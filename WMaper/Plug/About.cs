using System;
using WMagic;
using WMaper.Base;
using WMaper.Meta.Param;
using WMaper.Meta.Radio;

namespace WMaper.Plug
{
    /// <summary>
    /// 版权控件
    /// </summary>
    public sealed class About : Decor
    {
        #region 变量

        // 版权文本
        private string author;
        // 控件停靠
        private string anchor;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public About()
            : base()
        {
            this.anchor = "left";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option">控件配置</param>
        public About(Option option)
            : this()
        {
            if (!MatchUtils.IsEmpty(option))
            {
                if (option.Exist("Author"))
                    this.Author = option.Fetch<string>("Author");
                if (option.Exist("Anchor"))
                    this.anchor = option.Fetch<string>("Anchor").ToLower();
            }
        }

        #endregion

        #region 属性方法

        public string Author
        {
            get { return this.author; }
            set { this.author = value; }
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
                About about = null;
                {
                    if (!MatchUtils.IsEmpty(about = drv.Widget.About))
                    {
                        if (!this.Equals(about))
                        {
                            {
                                about.Remove();
                            }
                            this.Render(drv);
                        }
                    }
                    else
                    {
                        // 加载控件
                        try
                        {
                            (drv.Widget.About = this).Redraw();
                        }
                        catch
                        {
                            drv.Widget.About = null;
                        }
                        finally
                        {
                            if (MatchUtils.IsEmpty(drv.Widget.About))
                            {
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
                        this.Handle = new Misc.View.Plug.About(this, 30)
                    );
                }
                // 绘制图形
                WMaper.Misc.View.Plug.About v_about = this.Handle as WMaper.Misc.View.Plug.About;
                {
                    v_about.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (!MatchUtils.IsEmpty(this.Handle))
                        {
                            v_about.Build();
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
                    if (wipe)
                    {
                        this.Target.Widget.About = null;
                        {
                            this.Facade = null;
                            this.Handle = null;
                            this.Target = null;
                        }
                    }
                }
            }
        }

        #endregion
    }
}