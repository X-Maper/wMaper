namespace WMagic.Brush.Basic
{
    /// <summary>
    /// 角度范围类
    /// </summary>
    public class GAngle
    {

        #region 变量

        // 起始角
        private double f;
        // 结束角
        private double t;

        #endregion

        #region 属性方法

        public double F
        {
            get { return this.f; }
            set { this.f = value; }
        }

        public double T
        {
            get { return this.t; }
            set { this.t = value; }
        }

        #endregion

        #region 构造函数

        public GAngle()
            : this(0.0, 0.0)
        { }

        public GAngle(double f, double t)
        {
            this.f = f;
            this.t = t;
        }

        #endregion

    }
}