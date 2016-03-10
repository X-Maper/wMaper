namespace WMagic
{
    /// <remarks>
    /// -----------------------------------------------------------------------
    /// 部件名：StringUtils
    /// 工程名：WMagic
    /// 版权：CopyRight (c) 2013
    /// 创建人：WY
    /// 描述：字符串辅助类
    /// 创建日期：2013.06.09
    /// 修改人：
    /// 修改日期：
    /// -----------------------------------------------------------------------
    /// </remarks>

    /// <summary>
    /// 字符串辅助类
    /// </summary>
    public class StringUtils
    {
        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <param name="num">整型数</param>
        /// <returns>字符串</returns>
        public static string toString(int num)
        {
            return StringUtils.toString(num + "");
        }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <param name="num">长整型数</param>
        /// <returns>字符串</returns>
        public static string toString(long num)
        {
            return StringUtils.toString(num + "");
        }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <param name="num">单精度数</param>
        /// <returns>字符串</returns>
        public static string toString(float num)
        {
            return StringUtils.toString(num + "");
        }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <param name="num">双精度数</param>
        /// <returns>字符串</returns>
        public static string toString(double num)
        {
            return StringUtils.toString(num + "");
        }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字符串</returns>
        public static string toString(string str)
        {
            return StringUtils.toString(str, null);
        }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="ret">期望值</param>
        /// <returns>字符串</returns>
        public static string toString(string str, string ret)
        {
            return MatchUtils.IsEmpty(str) ? str : ret;
        }
    }
}