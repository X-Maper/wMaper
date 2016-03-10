using System;
using WMagic;
using WMaper.Base;

namespace WMaper.Proj.Epsg
{
    /// <summary>
    /// 墨卡托坐标参考系统
    /// </summary>
    public sealed class EPSG_900913 : Projcs
    {
        /// <summary>
        /// 解码函数
        /// </summary>
        /// <param name="wgs"></param>
        /// <returns></returns>
        public Coord Decode(Coord wgs)
        {
            return !MatchUtils.IsEmpty(wgs) ? new Coord(wgs.Lng * 20037508.34 / 180, Math.Log(Math.Tan((90 + wgs.Lat) * Math.PI / 360)) / Math.PI * 20037508.34) : null;
        }

        /// <summary>
        /// 编码函数
        /// </summary>
        /// <param name="mkt"></param>
        /// <returns></returns>
        public Coord Encode(Coord mkt)
        {
            return !MatchUtils.IsEmpty(mkt) ? new Coord(180 * mkt.Lng / 20037508.34, 180 / Math.PI * (2 * Math.Atan(Math.Exp(mkt.Lat / 20037508.34 * Math.PI)) - Math.PI / 2)) : null;
        }

        /// <summary>
        /// 解码函数
        /// </summary>
        /// <param name="wgs"></param>
        /// <returns></returns>
        public Bound Decode(Bound wgs)
        {
            return !MatchUtils.IsEmpty(wgs) ? new Bound(this.Decode(wgs.Min), this.Decode(wgs.Max)) : null;
        }

        /// <summary>
        /// 编码函数
        /// </summary>
        /// <param name="mkt"></param>
        /// <returns></returns>
        public Bound Encode(Bound mkt)
        {
            return !MatchUtils.IsEmpty(mkt) ? new Bound(this.Decode(mkt.Min), this.Decode(mkt.Max)) : null;
        }
    }
}