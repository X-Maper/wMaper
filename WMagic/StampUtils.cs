using System;
using System.Runtime.CompilerServices;

namespace WMagic
{
    /// <remarks>
    /// -----------------------------------------------------------------------
    /// 部件名：StampUtils
    /// 工程名：WMagic
    /// 版权：CopyRight (c) 2013
    /// 创建人：WY
    /// 描述：标识戳类
    /// 创建日期：2013.07.24
    /// 修改人：
    /// 修改日期：
    /// -----------------------------------------------------------------------
    /// </remarks>

    /// <summary>
    /// 标识戳类
    /// </summary>
    public class StampUtils
    {
        #region 常量

        private static readonly DateTime JAN1ST1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion

        #region 变量

        private static ulong SEQUENCE64 = 0x0000000000000000;

        #endregion

        /// <summary>
        /// 获得序列号
        /// </summary>
        /// <returns>序列号</returns>
        public static string GetSequence()
        {
            return (SEQUENCE64++ % 0xFFFFFFFFFFFFFFFF).ToString();
        }

        /// <summary>
        /// 获得时间戳
        /// </summary>
        /// <returns>时间戳</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string GetTimeStamp()
        {
            {
                System.Threading.Thread.Sleep(1);
            }
            return ((long)(DateTime.UtcNow - JAN1ST1970).TotalMilliseconds) + "";
        }
    }
}