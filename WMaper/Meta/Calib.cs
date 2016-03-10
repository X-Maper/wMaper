namespace WMaper.Meta
{
    /// <summary>
    /// 锚点类
    /// </summary>
    public sealed class Calib
    {
        #region 变量

        // X轴辐射范围
        private double x;
        // Y轴辐射范围
        private double y;

        #endregion

        #region 属性方法

        public double X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        #endregion

        #region 构造函数

        public Calib()
            : this(0.0, 0.0)
        { }

        public Calib(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion
    }
}