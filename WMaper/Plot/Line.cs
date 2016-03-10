using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using WMagic;
using WMagic.Brush;
using WMagic.Brush.Shape;
using WMaper.Base;
using WMaper.Core;
using WMaper.Meta;
using WMaper.Meta.Param;

namespace WMaper.Plot
{
    /// <summary>
    /// 线条层
    /// </summary>
    public sealed class Line : Geom
    {
        #region 变量

        // 粗细
        private int thick;
        // 箭头
        private bool arrow;
        // 颜色
        private Color color;
        // 样式
        private GLinear style;
        // 路径
        private List<Coord> route;

        #endregion

        #region 属性方法

        public int Thick
        {
            get { return this.thick; }
            set { this.thick = value; }
        }

        public bool Arrow
        {
            get { return this.arrow; }
            set { this.arrow = value; }
        }

        public Color Color
        {
            get { return this.color; }
            set { this.color = value; }
        }

        public GLinear Style
        {
            get { return this.style; }
            set { this.style = value; }
        }

        public List<Coord> Route
        {
            get { return this.route; }
            set { this.route = value; }
        }

        #endregion

        #region 构造函数

        public Line()
            : base()
        {
            this.thick = 1;
            this.arrow = false;
            this.style = GLinear.SOLID;
            this.route = new List<Coord>();
            this.color = Color.FromRgb(255, 0, 0);
        }

        public Line(Option option)
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
                    this.Matte = option.Fetch<bool>("Matte");
                if (option.Exist("Amend"))
                    this.Amend = option.Fetch<bool>("Amend");
                if (option.Exist("Alpha"))
                    this.Alpha = option.Fetch<int>("Alpha");
                if (option.Exist("Thick"))
                    this.Thick = option.Fetch<int>("Thick");
                if (option.Exist("Arrow"))
                    this.Arrow = option.Fetch<bool>("Arrow");
                if (option.Exist("Arise"))
                    this.Arise = option.Fetch<Arise>("Arise");
                if (option.Exist("Color"))
                    this.Color = option.Fetch<Color>("Color");
                if (option.Exist("Style"))
                    this.Style = option.Fetch<GLinear>("Style");
                if (option.Exist("Route"))
                    this.Route = option.Fetch<List<Coord>>("Route");
            }
        }

        #endregion

        #region 函数方法

        public sealed override void Redraw()
        {
            if (!MatchUtils.IsEmpty(this.Target) && !MatchUtils.IsEmpty(this.Target.Netmap) && !MatchUtils.IsEmpty(this.Facade) && this.Target.Enable && this.Enable)
            {
                bool hide = this.Matte || !this.Viewble(this.Arise);
                {
                    if (!MatchUtils.IsEmpty(this.Handle))
                    {
                        if (!(this.Handle.Matte = hide))
                        {
                            GLine v_line = this.Handle as GLine;
                            {
                                v_line.Mouse = this.Mouse;
                                v_line.Title = this.Title;
                                v_line.Alpha = this.Alpha;
                                v_line.Color = this.color;
                                v_line.Thick = this.thick;
                                v_line.Style = this.style;
                                v_line.Arrow = this.arrow;
                                v_line.Route = this.Fit4r(this.route);
                            }
                        }
                        // 重绘图形
                        this.Facade.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (!MatchUtils.IsEmpty(this.Handle))
                            {
                                this.Handle.Paint();
                            }
                        }));
                    }
                    else
                    {
                        if (!((this.Handle = this.Target.Sketch.CreateLine()).Matte = hide))
                        {
                            GLine v_line = this.Handle as GLine;
                            {
                                v_line.Mouse = this.Mouse;
                                v_line.Title = this.Title;
                                v_line.Alpha = this.Alpha;
                                v_line.Color = this.color;
                                v_line.Thick = this.thick;
                                v_line.Style = this.style;
                                v_line.Arrow = this.arrow;
                                v_line.Route = this.Fit4r(this.route);
                            }
                            // 绘制图形
                            this.Facade.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (!MatchUtils.IsEmpty(this.Handle))
                                {
                                    v_line.Paint();
                                }
                            }));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取周长
        /// </summary>
        /// <param name="fun"></param>
        public void Length(Action<double> fun)
        {
            if (!MatchUtils.IsEmpty(this.Target) && !MatchUtils.IsEmpty(this.Handle) && !MatchUtils.IsEmpty(fun))
            {
                double sum = 0.0;
                {
                    for (int i = 0, l = this.route.Count - 1; i < l; i++)
                    {
                        sum += this.route[i].Distance(this.Target, this.route[i + 1]);
                    }
                }
                // 回调长度
                try
                {
                    fun.Invoke(Math.Round(sum * 100) / 100.0);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    fun = null;
                }
            }
        }

        /// <summary>
        /// 是否相交
        /// </summary>
        /// <param name="crd"></param>
        /// <param name="fun"></param>
        public void Within(Coord crd, Action<bool> fun)
        {
            if (!MatchUtils.IsEmpty(this.Target) && !MatchUtils.IsEmpty(this.Handle) && !MatchUtils.IsEmpty(fun) && !MatchUtils.IsEmpty(crd))
            {
                // 回调相交
                try
                {
                    fun.Invoke(false);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    fun = null;
                }
            }
        }

        #endregion
    }
}