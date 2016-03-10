using WMaper.Base;

namespace WMaper.Proj
{
    /// <summary>
    /// 投影类接口
    /// </summary>
    public interface Projcs
    {
        /// <summary>
        /// 解码函数
        /// </summary>
        /// <param name="crd"></param>
        /// <returns></returns>
        Coord Decode(Coord crd);

        /// <summary>
        /// 编码函数
        /// </summary>
        /// <param name="crd"></param>
        /// <returns></returns>
        Coord Encode(Coord crd);

        /// <summary>
        /// 解码函数
        /// </summary>
        /// <param name="bnd"></param>
        /// <returns></returns>
        Bound Decode(Bound bnd);

        /// <summary>
        /// 编码函数
        /// </summary>
        /// <param name="bnd"></param>
        /// <returns></returns>
        Bound Encode(Bound bnd);
    }
}