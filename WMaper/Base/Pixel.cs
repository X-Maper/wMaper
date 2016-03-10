using WMagic;

namespace WMaper.Base
{
    public sealed class Pixel
    {
        #region 变量

        private double x;
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

        public Pixel(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

        #region 函数方法

        public Pixel Clone()
        {
            return new Pixel(this.x, this.y);
        }

        public Pixel Ratio(double per)
        {
            {
                this.x *= per;
                this.y *= per;
            }
            return this;
        }

        public Pixel Offset(double x, double y)
        {
            {
                this.x += x;
                this.y += y;
            }
            return this;
        }

        public Pixel Offset(Pixel dev)
        {
            return this.Offset(dev.X, dev.Y);
        }

        public bool Compare(Maper drv, Pixel pel)
        {
            return this.x == pel.x && this.y == pel.y;
        }

        public bool Compare(Maper drv, Coord crd)
        {
            return !MatchUtils.IsEmpty(drv) && !MatchUtils.IsEmpty(drv.Netmap) ? this.Compare(drv, drv.Netmap.Crd2px(crd)) : false;
        }

        public double Distance(Maper drv, Coord crd)
        {
            return !MatchUtils.IsEmpty(drv) && !MatchUtils.IsEmpty(drv.Netmap) ? this.Distance(drv, drv.Netmap.Crd2px(crd)) : 0.0;
        }

        public double Distance(Maper drv, Pixel pel)
        {
            return !MatchUtils.IsEmpty(drv) && !MatchUtils.IsEmpty(drv.Netmap) ? System.Math.Sqrt(System.Math.Pow(this.x - pel.x, 2) + System.Math.Pow(this.y - pel.y, 2)) / drv.Netmap.Craft * drv.Netmap.Deg2sc() * 0.0254 : 0.0;
        }

        #endregion
    }
}