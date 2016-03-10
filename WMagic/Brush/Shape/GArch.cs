using System;
using System.Windows.Media;
using WMagic.Brush.Basic;

namespace WMagic.Brush.Shape
{
    public class GArch : AShape
    {
        #region 变量

        // 粗细
        private int thick;
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
        // 中点
        private GPoint point;
        // 样式
        private GLinear style;

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

        public GPoint Point
        {
            get { return this.point; }
            set { this.point = value; }
        }

        public GLinear Style
        {
            get { return this.style; }
            set { this.style = value; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="graph">图元画板</param>
        public GArch(MagicGraphic graph)
            : base(graph, new DrawingVisual())
        {
            this.thick = 1;
            this.slope = 0;
            this.color = Colors.Red;
            this.style = GLinear.SOLID;
            this.stuff = Colors.Transparent;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 中心角转离心角
        /// </summary>
        /// <param name="av">中心角</param>
        /// <param name="ar">扇形所在椭圆辐射半径</param>
        /// <returns>离心角</returns>
        private double Ang2ecc(double av, GExtra ar)
        {
            double x = ar.Y * Math.Cos(av * Math.PI / 180), y = ar.X * Math.Sin(av * Math.PI / 180);
            {
                return Math.Atan2(Math.Abs(y), x) * (
                    y < 0 ? -1 : 1
                );
            }
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 线条渲染
        /// </summary>
        protected sealed override void Render()
        {
            if (!MatchUtils.IsEmpty(this.point) && !MatchUtils.IsEmpty(this.extra) && !MatchUtils.IsEmpty(this.angle))
            {
                // 配置画笔
                Pen pen = this.InitPen(this.style, this.color, this.thick);
                if (pen != null)
                {
                    // 配置填充
                    SolidColorBrush fill = this.InitStuff(this.stuff);
                    if (fill != null)
                    {
                        // 配置扇形
                        Geometry geom = (new GParse()).M(new GPoint(this.point.X + this.extra.X * Math.Cos(this.Ang2ecc(this.Angle.F, this.extra)), this.point.Y - this.extra.Y * Math.Sin(this.Ang2ecc(this.Angle.F, this.extra))))
                                                      .A(new GExtra(this.extra.X, this.extra.Y), 0, ((this.angle.T - this.angle.F) >= 180 ? 1 : 0), 0, new GPoint(this.point.X + this.extra.X * Math.Cos(this.Ang2ecc(this.Angle.T, this.extra)), this.point.Y - this.extra.Y * Math.Sin(this.Ang2ecc(this.Angle.T, this.extra))))
                                                      .L(new GPoint[] { new GPoint(this.point.X, this.point.Y) })
                                                      .Z().P();
                        if (geom != null)
                        {
                            // 绘制图形
                            using (DrawingContext dc = this.Brush.RenderOpen())
                            {
                                if (!this.Matte)
                                {
                                    dc.DrawGeometry(fill, pen, geom);
                                }
                            }
                            // 旋转对象
                            if (this.Matte || this.slope == 0)
                            {
                                this.Brush.Transform = null;
                            }
                            else
                            {
                                this.Brush.Transform = new RotateTransform(-this.slope, this.point.X, this.point.Y);
                            }
                        }
                    }
                }
            }

        }

        #endregion
    }
}