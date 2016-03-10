using WMaper.Base;

namespace WMaper.Proj.Epsg
{
    /// <summary>
    /// WGS 84坐标参考系统
    /// </summary>
    public sealed class EPSG_4326 : Projcs
    {
        /// <summary>
        /// 解码函数
        /// </summary>
        /// <param name="crd"></param>
        /// <returns></returns>
        public Coord Decode(Coord crd)
        {
            return crd;
        }

        /// <summary>
        /// 编码函数
        /// </summary>
        /// <param name="crd"></param>
        /// <returns></returns>
        public Coord Encode(Coord crd)
        {
            return crd;
        }

        /// <summary>
        /// 解码函数
        /// </summary>
        /// <param name="bnd"></param>
        /// <returns></returns>
        public Bound Decode(Bound bnd)
        {
            return bnd;
        }

        /// <summary>
        /// 编码函数
        /// </summary>
        /// <param name="bnd"></param>
        /// <returns></returns>
        public Bound Encode(Bound bnd)
        {
            return bnd;
        }
    }
}