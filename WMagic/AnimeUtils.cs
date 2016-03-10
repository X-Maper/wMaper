using System;
using WMagic.Anime;

namespace WMagic
{
    /// <remarks>
    /// -----------------------------------------------------------------------
    /// 部件名：AnimeUtils
    /// 工程名：WMagic
    /// 版权：CopyRight (c) 2013
    /// 创建人：WY
    /// 描述：图片辅助类
    /// 创建日期：2013.09.22
    /// 修改人：
    /// 修改日期：
    /// -----------------------------------------------------------------------
    /// </remarks>

    /// <summary>
    /// 动画辅助类
    /// </summary>
    public class AnimeUtils
    {
        /// <summary>
        /// 运行动画
        /// </summary>
        /// <param name="schedule">动画任务</param>
        /// <returns>MagicAnime</returns>
        public static MagicAnime Launch(Delegate schedule)
        {
            return new MagicAnime(schedule);
        }

        /// <summary>
        /// 运行动画
        /// </summary>
        /// <param name="schedule">动画任务</param>
        /// <param name="interval">时间间隔</param>
        /// <returns>MagicAnime</returns>
        public static MagicAnime Launch(Delegate schedule, long interval)
        {
            return new MagicAnime(schedule, interval);
        }

        /// <summary>
        /// 运行动画
        /// </summary>
        /// <param name="schedule">动画任务</param>
        /// <param name="interact">动画参数</param>
        /// <returns>MagicAnime</returns>
        public static MagicAnime Launch(Delegate schedule, Object[] interact)
        {
            return new MagicAnime(schedule, interact);
        }

        /// <summary>
        /// 运行动画
        /// </summary>
        /// <param name="schedule">动画任务</param>
        /// <param name="interact">动画参数</param>
        /// <param name="interval">时间间隔</param>
        /// <returns>MagicAnime</returns>
        public static MagicAnime Launch(Delegate schedule, Object[] interact, long interval)
        {
            return new MagicAnime(schedule, interact, interval);
        }

    }
}