using System;
using System.Collections.Generic;
using WMagic;
using WMagic.Thread;
using WMaper.Meta.Radio;
using WMaper.Meta.Store;

namespace WMaper.Base
{
    /// <summary>
    /// 事件类
    /// </summary>
    public sealed class Event
    {
        #region 变量

        // 优先
        private int priority;
        // 最先
        private int foremost;
        // 执行状态
        private bool complete;
        // 目标观众
        private Maper audience;
        // 观察者集
        private List<Action<Msger>> observer;
        // 事件工作队列
        private Queue<Motion<Msger>> schedule;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Event(Maper audience)
        {
            this.priority = 0;
            this.foremost = 0;
            this.priority = 0;
            this.complete = true;
            this.audience = audience;
            this.observer = new List<Action<Msger>>();
            this.schedule = new Queue<Motion<Msger>>();
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 销毁观察者
        /// </summary>
        public void Dispose()
        {
            this.observer.Clear();
            this.schedule.Clear();
            {
                this.priority = 0;
                this.foremost = 0;
                this.observer = null;
                this.schedule = null;
                this.audience = null;
                this.complete = true;
            }
        }

        /// <summary>
        /// 删除观察者
        /// </summary>
        /// <param name="fun">事件处理</param>
        public bool Detach(Action<Msger> fun)
        {
            int index = this.observer.IndexOf(fun);
            if (index > -1)
            {
                try
                {
                    this.observer.Remove(fun);
                }
                catch
                {
                    return false;
                }
                return this.foremost >= index ? this.priority-- >= 0 && this.foremost-- >= 0 : (
                    this.priority >= index ? this.priority-- >= 0 : true
                );
            }
            return true;
        }

        /// <summary>
        /// 添加观察者
        /// </summary>
        /// <param name="fun">事件处理</param>
        public bool Attach(Action<Msger> fun)
        {
            return this.Attach(fun, -1);
        }

        /// <summary>
        /// 添加观察者
        /// </summary>
        /// <param name="fun">事件处理</param>
        /// <param name="low">优先等级</param>
        public bool Attach(Action<Msger> fun, int low)
        {
            switch (low)
            {
                case 0:
                    {
                        try
                        {
                            this.observer.Insert(this.priority++, fun);
                        }
                        catch
                        {
                            {
                                this.priority--;
                            }
                            return false;
                        }
                        return true;
                    }
                case 1:
                    {
                        try
                        {
                            this.observer.Insert((0 * this.priority++) + this.foremost++, fun);
                        }
                        catch
                        {
                            {
                                this.priority--;
                                this.foremost--;
                            }
                            return false;
                        }
                        return true;
                    }
                default:
                    {
                        try
                        {
                            this.observer.Add(fun);
                        }
                        catch
                        {
                            return false;
                        }
                        return true;
                    }
            }
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="msg">消息</param>
        public void Broadcast(Object msg)
        {
            // 生成队列
            foreach (Action<Msger> fun in this.observer)
            {
                this.schedule.Enqueue(new Motion<Msger>(
                    fun, new Msger(this, msg)
                ));
            }
            // 执行队列
            if (this.complete)
            {
                try
                {
                    while (!(this.complete = this.schedule.Count.Equals(0)))
                    {
                        // 多线程调度
                        this.audience.Thread.Attach(new MagicThread(StampUtils.GetSequence(), new Func<IEnumerable<Motion<Msger>>, Action>((motions) =>
                        {
                            return (() =>
                            {
                                if (!MatchUtils.IsEmpty(motions))
                                {
                                    foreach (Motion<Msger> motion in motions)
                                    {
                                        try
                                        {
                                            motion.Fun(motion.Msg);
                                        }
                                        catch
                                        { }
                                    }
                                }
                            });
                        })(this.BatchTask(this.schedule, 100))));
                    }
                }
                catch
                {
                    {
                        this.schedule.Clear();
                    }
                    this.complete = true;
                }
            }
        }

        #endregion

        #region 私有方法

        private IEnumerable<Motion<Msger>> BatchTask(Queue<Motion<Msger>> queue, int count)
        {
            for (int i = 0; i < count; i++)
            {
                lock (this.schedule)
                {
                    if (queue.Count > 0) yield return queue.Dequeue();
                }
            }
        }

        #endregion
    }
}