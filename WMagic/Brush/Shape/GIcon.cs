using System.Windows;
using System.Windows.Media;
using WMagic.Brush.Basic;

namespace WMagic.Brush.Shape
{
    /// <summary>
    /// 图标类
    /// </summary>
    public class GIcon : AShape
    {
        #region 变量

        // 斜率
        private double slope;
        // 边框
        private GFrame frame;
        // 尺寸
        private GDimen dimen;
        // 锚点
        private GCalib calib;
        // 位置
        private GPoint point;
        // 图源
        private ImageSource image;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="graph">图元画板</param>
        public GIcon(MagicGraphic graph)
            : base(graph, new DrawingVisual())
        {
            this.slope = 0;
        }

        #endregion

        #region 属性方法

        public double Slope
        {
            get { return this.slope; }
            set { this.slope = value; }
        }

        public GFrame Frame
        {
            get { return this.frame; }
            set { this.frame = value; }
        }

        public GDimen Dimen
        {
            get { return this.dimen; }
            set { this.dimen = value; }
        }

        public GCalib Calib
        {
            get { return this.calib; }
            set { this.calib = value; }
        }

        public GPoint Point
        {
            get { return this.point; }
            set { this.point = value; }
        }

        public ImageSource Image
        {
            get { return this.image; }
            set
            {
                if (!MatchUtils.IsEmpty(this.image = value))
                {
                    // 冻结图像
                    if (this.image.CanFreeze)
                    {
                        this.image.Freeze();
                    }
                }
            }
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 线条渲染
        /// </summary>
        protected sealed override void Render()
        {
            if (!MatchUtils.IsEmpty(this.image) && !MatchUtils.IsEmpty(this.point))
            {
                // 配置尺寸
                double wide = 0, high = 0;
                {
                    if (!MatchUtils.IsEmpty(this.dimen))
                    {
                        if (this.dimen.W > 0)
                        {
                            wide = this.dimen.W;
                        }
                        if (this.dimen.H > 0)
                        {
                            high = this.dimen.H;
                        }
                    }
                }
                // 配置校准
                double movx = 0, movy = 0;
                {
                    if (!MatchUtils.IsEmpty(this.calib))
                    {
                        if (this.calib.X != 0)
                        {
                            movx = this.calib.X;
                        }
                        if (this.calib.Y != 0)
                        {
                            movy = this.calib.Y;
                        }
                    }
                }
                // 配置画笔
                Pen pen = null;
                {
                    if (!MatchUtils.IsEmpty(this.frame))
                    {
                        pen = this.InitPen(this.frame.Style, this.frame.Color, this.frame.Thick);
                    }
                }
                // 绘制图形
                using (DrawingContext dc = this.Brush.RenderOpen())
                {
                    if (!this.Matte)
                    {
                        dc.DrawImage(this.image, new Rect(this.point.X - movx, this.point.Y - movy, wide, high));
                        // 绘制边框
                        if (pen != null)
                        {
                            dc.DrawRectangle(null, pen, new Rect(this.point.X - movx - this.frame.Thick / 2.0, this.point.Y - movy - this.frame.Thick / 2.0, wide + this.frame.Thick, high + this.frame.Thick));
                        }
                    }
                }
                // 旋转对象
                if (this.Matte || this.slope == 0)
                {
                    this.Brush.Transform = null;
                }
                else
                {
                    this.Brush.Transform = new RotateTransform(-this.slope, this.point.X - movx + wide / 2, this.point.Y - movy + high / 2);
                }
            }
        }

        #endregion
    }
}