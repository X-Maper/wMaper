using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using WMagic;
using WMagic.Brush;
using WMagic.Brush.Basic;
using WMagic.Brush.Shape;
using WMaper.Base;
using WMaper.Core;
using WMaper.Meta;
using WMaper.Meta.Param;

namespace WMaper.Plot
{
    /// <summary>
    /// 多边形层
    /// </summary>
    public sealed class Poly : Geom
    {
        #region 变量

        // 粗细
        private int thick;
        // 颜色
        private Color color;
        // 填充
        private Color stuff;
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

        public Color Color
        {
            get { return this.color; }
            set { this.color = value; }
        }

        public Color Stuff
        {
            get { return this.stuff; }
            set { this.stuff = value; }
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

        public Poly()
            : base()
        {
            this.thick = 1;
            this.style = GLinear.SOLID;
            this.route = new List<Coord>();
            this.stuff = Colors.Transparent;
            this.color = Color.FromRgb(255, 0, 0);
        }

        public Poly(Option option)
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
                if (option.Exist("Arise"))
                    this.Arise = option.Fetch<Arise>("Arise");
                if (option.Exist("Stuff"))
                    this.Stuff = option.Fetch<Color>("Stuff");
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
                            GPoly v_poly = this.Handle as GPoly;
                            {
                                v_poly.Mouse = this.Mouse;
                                v_poly.Title = this.Title;
                                v_poly.Alpha = this.Alpha;
                                v_poly.Color = this.color;
                                v_poly.Thick = this.thick;
                                v_poly.Style = this.style;
                                v_poly.Stuff = this.stuff;
                                v_poly.Route = this.Fit4r(this.route);
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
                        if (!((this.Handle = this.Target.Sketch.CreatePoly()).Matte = hide))
                        {
                            GPoly v_poly = this.Handle as GPoly;
                            {
                                v_poly.Mouse = this.Mouse;
                                v_poly.Title = this.Title;
                                v_poly.Alpha = this.Alpha;
                                v_poly.Color = this.color;
                                v_poly.Thick = this.thick;
                                v_poly.Style = this.style;
                                v_poly.Stuff = this.stuff;
                                v_poly.Route = this.Fit4r(this.route);
                            }
                            // 绘制图形
                            this.Facade.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (!MatchUtils.IsEmpty(this.Handle))
                                {
                                    v_poly.Paint();
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
                    for (int i = 0, l = this.route.Count; i < l; i++)
                    {
                        if (l == i + 1)
                        {
                            sum += this.route[i].Distance(this.Target, this.route[0]);
                        }
                        else
                        {
                            sum += this.route[i].Distance(this.Target, this.route[i + 1]);
                        }
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
        /// 获取面积
        /// </summary>
        /// <param name="fun"></param>
        public void Square(Action<double> fun)
        {
            if (!MatchUtils.IsEmpty(this.Target) && !MatchUtils.IsEmpty(this.Handle) && !MatchUtils.IsEmpty(fun))
            {
                double sqr = 0.0;
                {
                    List<GPoint> fit4r = this.Fit4r(this.route);
                    for (int i = 0, l = fit4r.Count - 1; i < l; i++)
                    {
                        if (l == i + 1)
                        {
                            sqr += fit4r[i].X * fit4r[0].Y - fit4r[i].Y * fit4r[0].X;
                        }
                        else
                        {
                            sqr += fit4r[i].X * fit4r[i + 1].Y - fit4r[i].Y * fit4r[i + 1].X;
                        }
                    }
                }
                // 回调面积
                try
                {
                    fun.Invoke(Math.Round(Math.Abs(0.5 * sqr * Math.Pow(this.Target.Netmap.Deg2sc() / (this.Target.Netmap.Craft * WMaper.Units.M), 2)) * 100) / 100.0);
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