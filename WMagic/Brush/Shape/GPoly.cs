using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using WMagic.Brush.Basic;

namespace WMagic.Brush.Shape
{
    public class GPoly : AShape
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
        private List<GPoint> route;

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

        public List<GPoint> Route
        {
            get { return this.route; }
            set { this.route = value; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="graph">图元画板</param>
        public GPoly(MagicGraphic graph)
            : base(graph, new DrawingVisual())
        {
            this.thick = 1;
            this.color = Colors.Red;
            this.style = GLinear.SOLID;
            this.stuff = Colors.Transparent;
            this.route = new List<GPoint>();
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 多边形渲染
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
                    // 配置填充
                    SolidColorBrush fill = this.InitStuff(this.stuff);
                    if (fill != null)
                    {
                        // 配置多边形
                        Geometry geom = count.Equals(2) ? (
                            (new GParse()).M(this.route[0]).L(this.route.Skip(1).ToArray()).P()
                        ) : (
                            (new GParse()).M(this.route[0]).L(this.route.Skip(1).ToArray()).Z().P()
                        );
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
                        }
                    }
                }
            }
        }

        #endregion
    }
}