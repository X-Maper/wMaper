namespace WMaper.Meta.Store
{
    /// <summary>
    /// 图层尺寸信息类
    /// </summary>
    public sealed class Nature
    {
        #region 变量

        // Canvas横轴原点偏移量l
        private double l;
        // Canvas纵轴原点偏移量t
        private double t;
        // 图层范围宽度
        private double w;
        // 图层范围高度
        private double h;
        // Canvas原点像素横坐标x
        private double x;
        // Canvas原点像素纵坐标y
        private double y;

        #endregion

        #region 属性方法

        public double L
        {
            get { return this.l; }
            set { this.l = value; }
        }

        public double T
        {
            get { return this.t; }
            set { this.t = value; }
        }

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

        public Nature()
            : this(0.0, 0.0)
        { }

        public Nature(double l, double t)
        {
            this.l = l;
            this.t = t;
        }

        public Nature(double l, double t, double w, double h, double x, double y)
        {
            this.l = l;
            this.t = t;
            this.w = w;
            this.h = h;
            this.x = x;
            this.y = y;
        }

        #endregion
    }
}