namespace WMaper.Meta
{
    public sealed class Dimen
    {
        #region 变量

        private double w;
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

        public Dimen()
            : this(0.0, 0.0)
        { }

        public Dimen(double w, double h)
        {
            this.w = w;
            this.h = h;
        }

        #endregion
    }
}