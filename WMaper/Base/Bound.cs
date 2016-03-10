using WMagic;

namespace WMaper.Base
{
    /// <summary>
    /// 边界类
    /// </summary>
    public sealed class Bound
    {
        #region 变量

        // 最小坐标
        private Coord min;
        // 最大坐标 
        private Coord max;

        #endregion

        #region 属性方法

        public Coord Min
        {
            get { return this.min; }
            set { this.min = value; }
        }

        public Coord Max
        {
            get { return this.max; }
            set { this.max = value; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="min">最小坐标</param>
        /// <param name="max">最大坐标</param>
        public Bound(Coord min, Coord max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="minX">最小X</param>
        /// <param name="minY">最小Y</param>
        /// <param name="maxX">最大X</param>
        /// <param name="maxY">最大Y</param>
        public Bound(double minX, double minY, double maxX, double maxY)
        {
            this.min = new Coord(minX, maxY);
            this.max = new Coord(maxX, minY);
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns></returns>
        public Bound Clone()
        {
            return new Bound(this.min, this.max);
        }
        /// <summary>
        /// 计算中心点
        /// </summary>
        /// <returns></returns>
        public Coord Balance()
        {
            return new Coord((this.Min.Lng + this.Max.Lng) * 0.5, (this.Max.Lat + this.Min.Lat) * 0.5);
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <returns></returns>
        public bool Compare(Maper drv, Bound bnd)
        {
            return this.min.Compare(drv, bnd.min) && this.max.Compare(drv, bnd.max);
        }

        public Bound Correct(Maper drv, Bound bnd)
        {
            return new Bound(this.Correct(drv, bnd.min), this.Correct(drv, bnd.max));
        }

        public Coord Correct(Maper drv, Coord crd)
        {
            return new Coord(
                crd.Lng > this.min.Lng ? crd.Lng < this.max.Lng ? crd.Lng : this.max.Lng : this.min.Lng,
                crd.Lat > this.max.Lat ? crd.Lat < this.min.Lat ? crd.Lat : this.min.Lat : this.max.Lat
            );
        }

        public Pixel Correct(Maper drv, Pixel pel)
        {
            return !MatchUtils.IsEmpty(drv) && !MatchUtils.IsEmpty(drv.Netmap) ? drv.Netmap.Crd2px(this.Correct(drv, drv.Netmap.Px2crd(pel))) : null;
        }

        public bool Contain(Maper drv, Bound bnd)
        {
            return this.Contain(drv, bnd.min) && this.Contain(drv, bnd.max);
        }

        public bool Contain(Maper drv, Coord crd)
        {
            return crd.Lng >= this.min.Lng && crd.Lat >= this.max.Lat && crd.Lng <= this.max.Lng && crd.Lat <= this.min.Lat;
        }

        public bool Contain(Maper drv, Pixel pel)
        {
            return !MatchUtils.IsEmpty(drv) && !MatchUtils.IsEmpty(drv.Netmap) ? this.Contain(drv, drv.Netmap.Px2crd(pel)) : false;
        }

        #endregion
    }
}