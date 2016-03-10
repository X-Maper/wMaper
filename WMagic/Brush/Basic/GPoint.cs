namespace WMagic.Brush.Basic
{
    /// <summary>
    /// 几何点类
    /// </summary>
    public class GPoint
    {

        #region 变量

        // X轴坐标
        private double x;
        // Y轴坐标
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

        public GPoint()
            : this(0.0, 0.0)
        { }

        public GPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

        #region 比较工具

        internal sealed class Comparer : System.Collections.Generic.IEqualityComparer<GPoint>
        {
            public bool Equals(GPoint a, GPoint b)
            {
                return !object.ReferenceEquals(a, b) ? (
                    !object.ReferenceEquals(a, null) && !object.ReferenceEquals(b, null) ? a.X.Equals(b.X) && a.Y.Equals(b.Y) : false
                ) : true;
            }

            public int GetHashCode(GPoint x)
            {
                return this.GetHashCode();
            }
        }

        #endregion

    }
}