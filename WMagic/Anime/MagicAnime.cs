using System;
using System.Linq;
using System.Windows.Media;

namespace WMagic.Anime
{
    public class MagicAnime
    {
        #region 变量

        // 是否激活
        private bool activate;
        // 动画间隔
        private long interval;
        // 互动参数
        private Object[] interact;
        // 动画任务
        private Delegate schedule;
        // 当前时间
        private DateTime datetime;

        #endregion

        #region 构造函数

        public MagicAnime(Delegate schedule)
            : this(schedule, 0L)
        { }

        public MagicAnime(Delegate schedule, long interval)
            : this(schedule, null, interval)
        { }

        public MagicAnime(Delegate schedule, Object[] interact)
            : this(schedule, interact, 0L)
        { }

        public MagicAnime(Delegate schedule, Object[] interact, long interval)
        {
            this.datetime = DateTime.Now;
            {
                this.schedule = schedule;
                this.interval = interval;
                this.interact = interact;
            }
            this.Initialize();
        }

        #endregion

        #region 函数方法

        private void Initialize()
        {
            CompositionTarget.Rendering += this.Start;
        }

        private void Start(object obj, EventArgs evt)
        {
            this.activate = true;
            {
                if (Convert.ToInt64(DateTime.Now.Subtract(this.datetime).TotalMilliseconds) >= this.interval)
                {
                    this.datetime = DateTime.Now;
                    {
                        if (MatchUtils.IsEmpty(this.interact))
                        {
                            this.schedule.DynamicInvoke(new Object[] { this });
                        }
                        else
                        {
                            this.schedule.DynamicInvoke((new Object[] { this }).Concat(this.interact).ToArray());
                        }
                    }
                }
            }
        }

        public void Abort()
        {
            if (this.activate)
            {
                CompositionTarget.Rendering -= this.Start;
                {
                    this.schedule = null;
                    this.interact = null;
                    this.activate = false;
                    this.datetime = DateTime.Now;
                }
            }
        }

        #endregion
    }
}