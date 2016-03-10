using System;
using System.Collections.Generic;
using System.Linq;
using WMagic;
using WMaper.Base;
using WMaper.Meta.Embed;
using WMaper.Meta.Param;
using WMaper.Meta.Radio;

namespace WMaper.Plug
{
    public sealed class Tools : Decor
    {
        #region 变量

        // 控件激活
        private int active;
        // 控件位置
        private string anchor;
        // 工具列表
        private List<Tool> action;

        #endregion

        #region 属性方法

        public int Active
        {
            get { return this.active; }
            set { this.active = value; }
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

        public List<Tool> Action
        {
            get
            {
                return this.action;
            }
            set
            {
                if (value != null)
                {
                    this.action = value;
                }
            }
        }

        #endregion

        #region 构造函数

        public Tools()
            : base()
        {
            this.active = 0;
            this.anchor = "right";
            this.action = new List<Tool>();
        }

        public Tools(Option option)
            : this()
        {
            if (!MatchUtils.IsEmpty(option))
            {
                if (option.Exist("Active"))
                    this.active = option.Fetch<int>("Active");
                if (option.Exist("Action"))
                    this.action = option.Fetch<List<Tool>>("Action");
                if (option.Exist("Anchor"))
                    this.anchor = option.Fetch<string>("Anchor").ToLower();
            }
        }

        #endregion

        #region 函数方法

        public void Select(int tool)
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (this.active != tool && tool > -1)
                {
                    Tool item = null;
                    try
                    {
                        item = this.action.Where(x => x.Allow).ElementAt(tool);
                    }
                    catch
                    {
                        item = null;
                    }
                    finally
                    {
                        if (item != null)
                        {
                            this.Select(item);
                        }
                    }
                }
            }
        }

        public void Select(Tool tool)
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (!MatchUtils.IsEmpty(this.Handle) && !MatchUtils.IsEmpty(this.action) && !MatchUtils.IsEmpty(tool) && this.action.Contains(tool) && tool.Allow)
                {
                    Misc.View.Plug.Tools v_tools = this.Handle as Misc.View.Plug.Tools;
                    {
                        v_tools.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (!MatchUtils.IsEmpty(this.Handle))
                            {
                                v_tools.Apply(Array.IndexOf(this.action.Where(x => x.Allow).ToArray(), tool));
                            }
                        }));
                    }
                }
            }
        }

        public void Append(Tool tool)
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (!MatchUtils.IsEmpty(this.action) && !MatchUtils.IsEmpty(tool) && !this.action.Contains(tool))
                {
                    lock (this.action)
                    {
                        this.action.Add(tool);
                    }
                    // 重绘控件
                    this.Redraw();
                }
            }
        }

        public void Repeal(int tool)
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (tool > -1)
                {
                    Tool item = null;
                    try
                    {
                        item = this.action.Where(x => x.Allow).ElementAt(tool);
                    }
                    catch
                    {
                        item = null;
                    }
                    finally
                    {
                        if (item != null)
                        {
                            this.Repeal(item);
                        }
                    }
                }
            }
        }

        public void Repeal(Tool tool)
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (!MatchUtils.IsEmpty(this.action) && !MatchUtils.IsEmpty(tool) && this.action.Contains(tool))
                {
                    if (!tool.Allow)
                    {
                        lock (this.action)
                        {
                            this.action.Remove(tool);
                        }
                    }
                    else
                    {
                        lock (this.action)
                        {
                            this.active -= (
                                Array.IndexOf(this.action.Where(x => x.Allow).ToArray(), tool) > this.active ? (
                                    this.action.Remove(tool) ? 0 : 0
                                ) : (
                                    this.action.Remove(tool) ? 1 : 0
                                )
                           );
                        }
                        // 重绘控件
                        this.Redraw();
                    }
                }
            }
        }

        #endregion

        #region 重载方法

        public sealed override void Render(Maper drv)
        {
            if (!MatchUtils.IsEmpty(this.Target = drv) && drv.Enable)
            {
                Tools tools = null;
                {
                    if (!MatchUtils.IsEmpty(tools = drv.Widget.Tools))
                    {
                        if (!this.Equals(tools))
                        {
                            {
                                tools.Remove();
                            }
                            this.Render(drv);
                        }
                    }
                    else
                    {
                        // 加载控件
                        try
                        {
                            (drv.Widget.Tools = this).Redraw();
                        }
                        catch
                        {
                            drv.Widget.Tools = null;
                        }
                        finally
                        {
                            if (MatchUtils.IsEmpty(drv.Widget.Tools))
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

        public sealed override void Redraw()
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (MatchUtils.IsEmpty(this.Handle))
                {
                    this.Target.Canvas.Children.Add(
                        this.Handle = new Misc.View.Plug.Tools(this, 70)
                    );
                }
                // 绘制图形
                WMaper.Misc.View.Plug.Tools v_tools = this.Handle as WMaper.Misc.View.Plug.Tools;
                {
                    v_tools.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (!MatchUtils.IsEmpty(this.Handle))
                        {
                            v_tools.Build();
                        }
                    }));
                }
            }

        }

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
                        this.Target.Widget.Tools = null;
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