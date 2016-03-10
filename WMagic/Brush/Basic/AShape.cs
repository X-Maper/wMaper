using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WMagic.Brush.Basic
{
    /// <summary>
    /// 图元抽象基类
    /// </summary>
    public abstract class AShape
    {
        /// <summary>
        /// 语法解析器
        /// </summary>
        protected class GParse
        {
            private StringBuilder path;

            /// <summary>
            /// 构造函数
            /// </summary>
            public GParse()
            {
                this.path = new StringBuilder(128);
            }

            /// <summary>
            /// 绘制椭圆弧线
            /// </summary>
            /// <param name="size">弧的所在椭圆辐射半径</param>
            /// <param name="rtAngle">椭圆倾斜度</param>
            /// <param name="isLarge">弧度范围是否大于180</param>
            /// <param name="sweepDir">弧线是否正角方向绘制</param>
            /// <param name="endPoint">弧线将绘制到的点</param>
            /// <returns></returns>
            public GParse A(GExtra size, double rtAngle, int isLarge, int sweepDir, GPoint endPoint)
            {
                if (size != null && endPoint != null)
                {
                    this.path.AppendFormat(" {0} {1} {2} {3} {4} {5} {6} {7}", "A", size.X, size.Y, rtAngle, isLarge, sweepDir, endPoint.X, endPoint.Y);
                }
                return this;
            }

            /// <summary>
            /// 绘制三次方贝塞尔曲线
            /// </summary>
            /// <param name="ctrlPoint1">曲线的第一个控制点</param>
            /// <param name="ctrlPoint2">曲线的第二个控制点</param>
            /// <param name="endPoint">曲线将绘制到的点</param>
            /// <returns></returns>
            public GParse C(GPoint ctrlPoint1, GPoint ctrlPoint2, GPoint endPoint)
            {
                if (ctrlPoint1 != null && ctrlPoint2 != null && endPoint != null)
                {
                    this.path.AppendFormat(" {0} {1} {2} {3} {4} {5} {6}", "C", ctrlPoint1.X, ctrlPoint1.Y, ctrlPoint2.X, ctrlPoint2.Y, endPoint.X, endPoint.Y);
                }
                return this;
            }

            /// <summary>
            /// 绘制二次贝塞尔曲线
            /// </summary>
            /// <param name="ctrlPoint">曲线的控制点</param>
            /// <param name="endPoint">曲线将绘制到的点</param>
            /// <returns></returns>
            public GParse Q(GPoint ctrlPoint, GPoint endPoint)
            {
                if (ctrlPoint != null && endPoint != null)
                {
                    this.path.AppendFormat(" {0} {1} {2} {3} {4}", "Q", ctrlPoint.X, ctrlPoint.Y, endPoint.X, endPoint.Y);
                }
                return this;
            }

            /// <summary>
            /// 绘制直线
            /// </summary>
            /// <param name="endPoints">线条要绘制到的点集合</param>
            /// <returns></returns>
            public GParse L(GPoint[] endPoints)
            {
                if (endPoints != null && endPoints.Length > 0)
                {
                    this.path.AppendFormat(" {0}", "L");
                    for (int i = 0, l = endPoints.Length; i < l; i++)
                    {
                        this.path.AppendFormat(" {0} {1}", endPoints[i].X, endPoints[i].Y);
                    }
                }
                return this;
            }

            /// <summary>
            /// 制定新图形的起点
            /// </summary>
            /// <param name="startPoint">新图形的起点</param>
            /// <returns></returns>
            public GParse M(GPoint startPoint)
            {
                if (startPoint != null)
                {
                    this.path.AppendFormat(" {0} {1} {2}", "M", startPoint.X, startPoint.Y);
                }
                return this;
            }

            /// <summary>
            /// 闭合绘制图形
            /// </summary>
            /// <returns></returns>
            public GParse Z()
            {
                this.path.AppendFormat(" {0}", "Z");
                return this;
            }

            /// <summary>
            /// 生成图形对象
            /// </summary>
            /// <returns></returns>
            public Geometry P()
            {
                Geometry geometry = null;
                try
                {
                    geometry = Geometry.Parse(this.path.ToString().Trim());
                }
                catch
                {
                    geometry = null;
                }
                finally
                {
                    // 冻结对象
                    if (geometry != null && geometry.CanFreeze)
                    {
                        geometry.Freeze();
                    }
                }
                return geometry;
            }
        }

        #region 事件

        public event MouseButtonEventHandler MouseDown;
        public event MouseEventHandler MouseMove;
        public event MouseButtonEventHandler MouseUp;
        public event MouseEventHandler MouseLeave;
        public event MouseEventHandler MouseEnter;
        public event MouseWheelEventHandler MouseWheel;
        public event ContextMenuEventHandler ContextMenuOpening;

        internal void OnMouseDown(MouseButtonEventArgs e)
        {
            if (this.MouseDown != null)
            {
                this.MouseDown(this, e);
            }
        }

        internal void OnMouseMove(MouseEventArgs e)
        {
            if (this.MouseMove != null)
            {
                this.MouseMove(this, e);
            }
        }

        internal void OnMouseUp(MouseButtonEventArgs e)
        {
            if (this.MouseUp != null)
            {
                this.MouseUp(this, e);
            }
        }

        internal void OnMouseLeave(MouseEventArgs e)
        {
            if (this.MouseLeave != null)
            {
                this.MouseLeave(this, e);
            }
        }

        internal void OnMouseEnter(MouseEventArgs e)
        {
            if (this.MouseEnter != null)
            {
                this.MouseEnter(this, e);
            }
        }

        internal void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (this.MouseWheel != null)
            {
                this.MouseWheel(this, e);
            }
        }

        internal void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            if (this.ContextMenuOpening != null)
            {
                this.ContextMenuOpening(this, e);
            }
        }

        #endregion

        #region 变量

        // 图元画笔
        private DrawingVisual brush;
        // 图元画板
        private MagicGraphic graph;
        // 图元标识
        private string ident;
        // 图元标题
        private string title;
        // 图源光标
        private Cursor mouse;
        // 是否挂载
        private bool mount;
        // 是否可见
        private bool matte;
        // 透明度
        private int alpha;

        #endregion

        #region 属性方法

        protected DrawingVisual Brush
        {
            get { return this.brush; }
        }

        protected MagicGraphic Graph
        {
            get { return this.graph; }
        }

        protected string Ident
        {
            get { return this.ident; }
        }

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public Cursor Mouse
        {
            get { return this.mouse; }
            set { this.mouse = value; }
        }

        public bool Mount
        {
            get { return this.mount; }
            set { this.mount = value; }
        }

        public bool Matte
        {
            get { return this.matte; }
            set { this.matte = value; }
        }

        public int Alpha
        {
            get { return this.alpha; }
            set { this.alpha = value; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public AShape(MagicGraphic graph, DrawingVisual brush)
        {
            this.alpha = 100;
            this.title = null;
            this.matte = false;
            this.mount = false;
            this.brush = brush;
            this.graph = graph;
            this.ident = graph.Index;
            {
                this.brush.SetValue(MagicGraphic.Nexus, this);
            }
        }

        #endregion

        #region 抽象函数

        /// <summary>
        /// 元素渲染
        /// </summary>
        protected abstract void Render();

        #endregion

        #region 函数方法

        /// <summary>
        /// 元素绘制
        /// </summary>
        public void Paint()
        {
            if (!this.mount)
            {
                this.mount = true;
                {
                    this.graph.Panel.AppendVisual(this.brush);
                }
            }
            // 透明度
            {
                this.brush.Opacity = this.alpha / 100.0;
            }
            this.Render();
        }

        /// <summary>
        /// 图元擦除
        /// </summary>
        public void Earse()
        {
            if (this.mount)
            {
                this.graph.Panel.RemoveVisual(this.brush);
                {
                    this.mount = false;
                }
            }
        }

        /// <summary>
        /// 图元置前
        /// </summary>
        public void Front()
        {
            if (this.mount)
            {
                this.graph.Panel.RemoveVisual(this.brush);
                this.graph.Panel.AppendVisual(this.brush);
            }
        }

        /// <summary>
        /// 初始化画笔
        /// </summary>
        /// <returns></returns>
        protected Pen InitPen(GLinear style, Color color, int thick)
        {
            Pen pen = new Pen(new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B)), thick);
            switch (style)
            {
                case GLinear.DOT:
                    {
                        pen.DashStyle = DashStyles.Dot;
                        break;
                    }
                case GLinear.DASH:
                    {
                        pen.DashStyle = DashStyles.Dash;
                        break;
                    }
                case GLinear.SOLID:
                    {
                        pen.DashStyle = DashStyles.Solid;
                        break;
                    }
                case GLinear.DASHDOT:
                    {
                        pen.DashStyle = DashStyles.DashDot;
                        break;
                    }
                default:
                    {
                        pen.DashStyle = DashStyles.Solid;
                        break;
                    }
            }
            // 冻结画笔
            if (pen.CanFreeze)
            {
                pen.Freeze();
            }
            return pen;
        }

        /// <summary>
        /// 初始化填充
        /// </summary>
        /// <param name="color">填充颜色</param>
        /// <returns></returns>
        protected SolidColorBrush InitStuff(Color color)
        {
            SolidColorBrush stuff = new SolidColorBrush(color);
            // 冻结填充
            if (stuff.CanFreeze)
            {
                stuff.Freeze();
            }
            return stuff;
        }

        /// <summary>
        /// 绘制箭头
        /// </summary>
        /// <param name="dct"></param>
        /// <param name="bgn"></param>
        /// <param name="end"></param>
        /// <param name="pen"></param>
        protected void DrawArrow(DrawingContext dct, GPoint bgn, GPoint end, Pen pen)
        {
            double wide = pen.Thickness * 8;
            double high = pen.Thickness * 2;
            double theta = Math.Atan2(bgn.Y - end.Y, bgn.X - end.X);

            double sint = Math.Sin(theta);
            double cost = Math.Cos(theta);

            Point crd = new Point(end.X, end.Y);

            dct.DrawLine(pen, crd, new Point(end.X + (wide * cost - high * sint), end.Y + (wide * sint + high * cost)));
            dct.DrawLine(pen, crd, new Point(end.X + (wide * cost + high * sint), end.Y - (high * cost - wide * sint)));
        }

        #endregion
    }
}