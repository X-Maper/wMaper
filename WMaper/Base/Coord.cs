using WMagic;

namespace WMaper.Base
{
    public sealed class Coord
    {
        #region 变量

        private double lng;
        private double lat;

        #endregion

        #region 属性方法

        public double Lng
        {
            get { return this.lng; }
            set { this.lng = value; }
        }

        public double Lat
        {
            get { return this.lat; }
            set { this.lat = value; }
        }

        #endregion

        #region 构造函数

        public Coord(double lng, double lat)
        {
            this.lng = lng;
            this.lat = lat;
        }

        #endregion

        #region 函数方法

        public Coord Clone()
        {
            return new Coord(this.lng, this.lat);
        }

        public Coord Ratio(double per)
        {
            {
                this.lng *= per;
                this.lat *= per;
            }
            return this;
        }

        public Coord Offset(double lng, double lat)
        {
            {
                this.lng += lng;
                this.lat += lat;
            }
            return this;
        }

        public Coord Offset(Coord dev)
        {
            return this.Offset(dev.Lng, dev.Lat);
        }

        public bool Compare(Maper drv, Coord crd)
        {
            return this.lng == crd.lng && this.lat == crd.lat;
        }

        public bool Compare(Maper drv, Pixel pel)
        {
            return !MatchUtils.IsEmpty(drv) && !MatchUtils.IsEmpty(drv.Netmap) ? this.Compare(drv, drv.Netmap.Px2crd(pel)) : false;
        }

        public double Distance(Maper drv, Coord crd)
        {
            return !MatchUtils.IsEmpty(drv) && !MatchUtils.IsEmpty(drv.Netmap) ? drv.Netmap.Crd2px(this).Distance(drv, crd) : 0.0;
        }

        public double Distance(Maper drv, Pixel pel)
        {
            return !MatchUtils.IsEmpty(drv) && !MatchUtils.IsEmpty(drv.Netmap) ? drv.Netmap.Crd2px(this).Distance(drv, pel) : 0.0;
        }

        #endregion
    }
}