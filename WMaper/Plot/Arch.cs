using System;
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
    /// 扇形层
    /// </summary>
    public sealed class Arch : Geom
    {
        #region 变量

        // 粗细
        private int thick;
        // 位置
        private Coord point;
        // 颜色
        private Color color;
        // 填充
        private Color stuff;
        // 斜率
        private double slope;
        // 角度
        private GAngle angle;
        // 辐射
        private GExtra extra;
        // 样式
        private GLinear style;

        #endregion

        #region 属性方法

        public int Thick
        {
            get { return this.thick; }
            set { this.thick = value; }
        }

        public Coord Point
        {
            get { return this.point; }
            set { this.point = value; }
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

        public double Slope
        {
            get { return this.slope; }
            set { this.slope = value; }
        }

        public GAngle Angle
        {
            get { return this.angle; }
            set { this.angle = value; }
        }

        public GExtra Extra
        {
            get { return this.extra; }
            set { this.extra = value; }
        }

        public GLinear Style
        {
            get { return this.style; }
            set { this.style = value; }
        }

        #endregion

        #region 构造函数

        public Arch()
            : base()
        {
            this.thick = 1;
            this.slope = 0;
            this.angle = null;
            this.extra = null;
            this.point = null;
            this.style = GLinear.SOLID;
            this.stuff = Colors.Transparent;
            this.color = Color.FromRgb(255, 0, 0);
        }

        public Arch(Option option)
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
                if (option.Exist("Slope"))
                    this.Slope = option.Fetch<double>("Slope");
                if (option.Exist("Arise"))
                    this.Arise = option.Fetch<Arise>("Arise");
                if (option.Exist("Stuff"))
                    this.Stuff = option.Fetch<Color>("Stuff");
                if (option.Exist("Color"))
                    this.Color = option.Fetch<Color>("Color");
                if (option.Exist("Point"))
                    this.Point = option.Fetch<Coord>("Point");
                if (option.Exist("Angle"))
                    this.Angle = option.Fetch<GAngle>("Angle");
                if (option.Exist("Extra"))
                    this.Extra = option.Fetch<GExtra>("Extra");
                if (option.Exist("Style"))
                    this.Style = option.Fetch<GLinear>("Style");
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
                            GArch v_arch = this.Handle as GArch;
                            {
                                v_arch.Mouse = this.Mouse;
                                v_arch.Title = this.Title;
                                v_arch.Alpha = this.Alpha;
                                v_arch.Color = this.color;
                                v_arch.Thick = this.thick;
                                v_arch.Style = this.style;
                                v_arch.Stuff = this.stuff;
                                v_arch.Slope = this.slope;
                                v_arch.Angle = this.angle;
                                v_arch.Extra = this.Fit4e(this.extra);
                                v_arch.Point = this.Fit4p(this.point);
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
                        if (!((this.Handle = this.Target.Sketch.CreateArch()).Matte = hide))
                        {
                            GArch v_arch = this.Handle as GArch;
                            {
                                v_arch.Mouse = this.Mouse;
                                v_arch.Title = this.Title;
                                v_arch.Alpha = this.Alpha;
                                v_arch.Color = this.color;
                                v_arch.Thick = this.thick;
                                v_arch.Style = this.style;
                                v_arch.Stuff = this.stuff;
                                v_arch.Slope = this.slope;
                                v_arch.Angle = this.angle;
                                v_arch.Extra = this.Fit4e(this.extra);
                                v_arch.Point = this.Fit4p(this.point);
                            }
                            // 绘制图形
                            this.Facade.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (!MatchUtils.IsEmpty(this.Handle))
                                {
                                    v_arch.Paint();
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
                // 回调长度
                try
                {
                    fun.Invoke(0.0);
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
                // 回调面积
                try
                {
                    fun.Invoke(0.0);
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