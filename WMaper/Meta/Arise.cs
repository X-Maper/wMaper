namespace WMaper.Meta
{
    public sealed class Arise
    {
        #region 变量

        private double min;
        private double max;

        #endregion

        #region 属性方法

        public double Min
        {
            get { return this.min; }
            set { this.min = value; }
        }

        public double Max
        {
            get { return this.max; }
            set { this.max = value; }
        }

        #endregion

        #region 构造函数

        public Arise()
            : this(0.0, 0.0)
        { }

        public Arise(double min, double max)
        {
            this.min = min;
            this.max = max;
        }

        #endregion
    }
}