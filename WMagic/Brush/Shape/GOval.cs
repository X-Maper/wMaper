using System.Windows.Media;
using WMagic.Brush.Basic;

namespace WMagic.Brush.Shape
{
    /// <summary>
    /// 椭圆类
    /// </summary>
    public class GOval : AShape
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
        // 辐射
        private GExtra extra;
        // 中点
        private GPoint point;
        // 样式
        private GLinear style;

        #endregion

        #region 属性

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
        public GOval(MagicGraphic graph)
            : base(graph, new DrawingVisual())
        {
            this.thick = 1;
            this.slope = 0;
            this.color = Colors.Red;
            this.style = GLinear.SOLID;
            this.stuff = Colors.Transparent;
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 线条渲染
        /// </summary>
        protected sealed override void Render()
        {
            if (!MatchUtils.IsEmpty(this.point) && !MatchUtils.IsEmpty(this.extra))
            {
                // 配置画笔
                Pen pen = this.InitPen(this.style, this.color, this.thick);
                if (pen != null)
                {
                    // 配置内部填充
                    SolidColorBrush fill = this.InitStuff(this.stuff);
                    if (fill != null)
                    {
                        // 配置椭圆
                        Geometry geom = (new GParse()).M(new GPoint(point.X, this.point.Y - this.extra.Y))
                                                      .A(new GExtra(extra.X, this.extra.Y), 0, 1, 1, new GPoint(this.point.X, this.point.Y + this.extra.Y))
                                                      .A(new GExtra(extra.X, this.extra.Y), 0, 1, 1, new GPoint(this.point.X, this.point.Y - this.extra.Y))
                                                      .P();
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