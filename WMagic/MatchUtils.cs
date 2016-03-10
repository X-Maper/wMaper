using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace WMagic
{
    /// <remarks>
    /// -----------------------------------------------------------------------
    /// 部件名：MatchUtils
    /// 工程名：WMagic
    /// 版权：CopyRight (c) 2013
    /// 创建人：WY
    /// 描述：校验辅助类
    /// 创建日期：2013.05.13
    /// 修改人：
    /// 修改日期：
    /// -----------------------------------------------------------------------
    /// </remarks>

    /// <summary>
    /// 校验辅助类
    /// </summary>
    public class MatchUtils
    {

        private static readonly Point RULE_POINT = new Point();

        /// <summary>
        /// 校验对象是否为空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>是否为空</returns>
        public static bool IsEmpty(object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// 校验字符串是否为空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是否为空</returns>
        public static bool IsEmpty(string str)
        {
            return str == null || "".Equals(str);
        }

        /// <summary>
        /// 校验字节流是否为空
        /// </summary>
        /// <param name="bit">字节流</param>
        /// <returns>是否为空</returns>
        public static bool IsEmpty(byte[] bit)
        {
            return bit == null || bit.Length == 0;
        }

        /// <summary>
        /// 校验精度数组是否为空
        /// </summary>
        /// <param name="dbl">精度数组</param>
        /// <returns>是否为空</returns>
        public static bool IsEmpty(double[] dbl)
        {
            return dbl == null || dbl.Length == 0;
        }

        /// <summary>
        /// 校验字符数组是否为空
        /// </summary>
        /// <param name="str">字符数组</param>
        /// <returns>是否为空</returns>
        public static bool IsEmpty(string[] str)
        {
            return str == null || str.Length == 0;
        }

        /// <summary>
        /// 校验对象数组是否为空
        /// </summary>
        /// <param name="obj">对象数组</param>
        /// <returns>是否为空</returns>
        public static bool IsEmpty(object[] obj)
        {
            return obj == null || obj.Length == 0;
        }

        /// <summary>
        /// 校验目标点是否为空
        /// </summary>
        /// <param name="pnt">目标点</param>
        /// <returns>是否为空</returns>
        public static bool IsEmpty(Point pnt)
        {
            return pnt == null || RULE_POINT.Equals(pnt);
        }

        /// <summary>
        /// 校验映射是否为空
        /// </summary>
        /// <param name="map">映射</param>
        /// <returns>是否为空</returns>
        public static bool IsEmpty(Hashtable map)
        {
            return map == null || map.Count == 0;
        }

        /// <summary>
        /// 校验列表是否为空
        /// </summary>
        /// <param name="lst">列表</param>
        /// <returns>是否为空</returns>
        public static bool IsEmpty(ArrayList lst)
        {
            return lst == null || lst.Count == 0;
        }

        /// <summary>
        /// 校验列表是否为空
        /// </summary>
        /// <param name="lst">列表</param>
        /// <returns>是否为空</returns>
        public static bool IsEmpty(List<object> lst)
        {
            return lst == null || lst.Count == 0;
        }

        /// <summary>
        /// 校验字符串是否为网址
        /// </summary>
        /// <param name="url">网址</param>
        /// <returns>是否网址</returns>
        public static bool IsUrl(string url)
        {
            return Regex.IsMatch(url, @"^(((HT|F)TPS?:\/)?\/|(\.+\/)+|[\w\-]+[\.\/])[\w\-]+([:\.\/][\w\-]+)*\/?(\?(\w+(=[^\s]*)?&?)+)?(#\w*)?$", RegexOptions.IgnoreCase);
        }
    }
}