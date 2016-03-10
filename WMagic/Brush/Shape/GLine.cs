using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using WMagic.Brush.Basic;

namespace WMagic.Brush.Shape
{
    /// <summary>
    /// 线条类
    /// </summary>
    public class GLine : AShape
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
        private List<GPoint> route;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="graph">图元画板</param>
        public GLine(MagicGraphic graph)
            : base(graph, new DrawingVisual())
        {
            this.thick = 1;
            this.arrow = false;
            this.color = Colors.Red;
            this.style = GLinear.SOLID;
            this.route = new List<GPoint>();
        }

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

        public List<GPoint> Route
        {
            get { return this.route; }
            set { this.route = value; }
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 线条渲染
        /// </summary>
        protected sealed override void Render()
        {
            int count = !MatchUtils.IsEmpty(this.route) ? this.route.Distinct(new GPoint.Comparer()).Count() : 0;
            if (count < 2)
            {
                this.Earse();
            }
            else
            {
                // 配置画笔
                Pen pen = this.InitPen(this.style, this.color, this.thick);
                if (pen != null)
                {
                    // 配置线段
                    Geometry geom = (new GParse()).M(this.route[0]).L(this.route.Skip(1).ToArray()).P();
                    if (geom != null)
                    {
                        Pen apen = this.arrow ? this.InitPen(GLinear.SOLID, this.color, this.thick) : null;
                        {
                            using (DrawingContext dc = this.Brush.RenderOpen())
                            {
                                if (!this.Matte)
                                {
                                    dc.DrawGeometry(null, pen, geom);
                                    // 绘制箭头
                                    if (!MatchUtils.IsEmpty(apen))
                                    {
                                        count = this.route.Count;
                                        {
                                            this.DrawArrow(dc, this.route[count - 2], this.route[count - 1], apen);
                                        }
                                    }
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