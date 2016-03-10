namespace WMagic.Brush.Basic
{
    /// <summary>
    /// 尺寸大小类
    /// </summary>
    public class GDimen
    {

        #region 变量

        // 宽度
        private double w;
        // 高度
        private double h;

        #endregion

        #region 属性方法

        public double W
        {
            get { return this.w; }
            set { this.w = value; }
        }

        public double H
        {
            get { return this.h; }
            set { this.h = value; }
        }

        #endregion

        #region 构造函数

        public GDimen()
            : this(0.0, 0.0)
        { }

        public GDimen(double w, double h)
        {
            this.w = w;
            this.h = h;
        }

        #endregion

    }
}