using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace WMagic.Thread
{
    /// <remarks>
    /// -----------------------------------------------------------------------
    /// 部件名：WMagicThreadFactory
    /// 工程名：WMagic
    /// 版权：CopyRight (c) 2013
    /// 创建人：WY
    /// 描述：线程工厂类
    /// 创建日期：2013.05.17
    /// 修改人：ZFL
    /// 修改日期：2013.05.20
    /// -----------------------------------------------------------------------
    /// </remarks>

    /// <summary>
    ///  线程工厂类
    /// </summary>
    public class MagicThreadFactory
    {
        #region 变量

        // 线程键-值对集合
        private Hashtable taskLibrary;
        // 线程集合取消管理
        private CancellationTokenSource taskControl;
        // 任务执行选项
        private TaskContinuationOptions execOptions;
        // 任务创建选项
        private TaskCreationOptions taskOptions;
        // 任务排队计划
        private TaskScheduler taskProvide;
        // 任务创建执行工厂
        private TaskFactory taskFactory;

        #endregion

        #region 属性

        public bool IsDestroy
        {
            get { return this.taskControl.IsCancellationRequested; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public MagicThreadFactory()
            : this(new CancellationTokenSource())
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="taskControl">线程集合取消管理</param>
        public MagicThreadFactory(CancellationTokenSource taskControl)
            : this(taskControl, TaskCreationOptions.PreferFairness, TaskContinuationOptions.PreferFairness, TaskScheduler.Default)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="taskControl">线程集合取消管理</param>
        /// <param name="taskOptions">任务创建选项</param>
        /// <param name="execOptions">任务执行选项</param>
        /// <param name="taskProvide">任务排队计划</param>
        public MagicThreadFactory(CancellationTokenSource taskControl, TaskCreationOptions taskOptions, TaskContinuationOptions execOptions, TaskScheduler taskProvide)
        {
            this.taskFactory = new TaskFactory((this.taskControl = taskControl).Token, this.taskOptions = taskOptions, this.execOptions = execOptions, this.taskProvide = taskProvide);
            {
                // 初始化任务集合
                this.taskLibrary = Hashtable.Synchronized(new Hashtable());
            }
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 清空未完成任务线程
        /// </summary>
        public void Destroy()
        {
            this.taskControl.Cancel();
            {
                this.taskLibrary.Clear();
            }
        }

        /// <summary>
        /// 撤销某一项任务线程
        /// </summary>
        /// <param name="key">线程标识</param>
        public void Detach(string key)
        {
            MagicThread thread = null;
            {
                lock (this.taskLibrary)
                {
                    if (this.taskLibrary.ContainsKey(key))
                    {
                        thread = this.taskLibrary[key] as MagicThread;
                    }
                }
            }
            // 撤销线程
            this.Detach(thread);
        }

        /// <summary>
        /// 撤销某一项任务线程
        /// </summary>
        /// <param name="thread">线程</param>
        public void Detach(MagicThread thread)
        {
            if (thread != null)
            {
                thread.Cancel();
                {
                    lock (this.taskLibrary)
                    {
                        if (this.taskLibrary.ContainsKey(thread.Key))
                        {
                            this.taskLibrary.Remove(thread.Key);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 执行创建线程，执行结束清除
        /// </summary>
        /// <param name="thread">线程</param>
        public void Attach(MagicThread thread)
        {
            this.Attach(thread, new CancellationTokenSource());
        }

        /// <summary>
        /// 执行创建线程，执行结束清除
        /// </summary>
        /// <param name="thread">线程</param>
        /// <param name="reactor">线程取消管理</param>
        public void Attach(MagicThread thread, CancellationTokenSource reactor)
        {
            if (!this.IsDestroy && !reactor.IsCancellationRequested)
            {
                // 加入线程集合
                lock (this.taskLibrary)
                {
                    if (this.taskLibrary.ContainsKey(thread.Key))
                    {
                        {
                            thread.Reactor = null;
                        }
                        return;
                    }
                    else
                    {
                        {
                            thread.Reactor = reactor;
                        }
                        this.taskLibrary.Add(thread.Key, thread);
                    }
                }
                // 启动线程
                if (!MatchUtils.IsEmpty(thread.Reactor))
                {
                    (this.taskFactory.StartNew(thread.Fun, reactor.Token, thread.Policy != TaskCreationOptions.PreferFairness ? thread.Policy : this.taskOptions, this.taskProvide)).ContinueWith((task) =>
                    {
                        // 线程执行完在集合中删除
                        lock (this.taskLibrary)
                        {
                            if (this.taskLibrary.ContainsKey(thread.Key))
                            {
                                this.taskLibrary.Remove(thread.Key);
                            }
                        }
                    });
                }
            }
        }

        /// <summary>
        /// 执行创建线程组，执行结束清除
        /// </summary>
        /// <param name="threads">线程组</param>
        public void Attach(MagicThread[] threads)
        {
            this.Attach(threads, new CancellationTokenSource());
        }

        /// <summary>
        /// 执行创建线程组，执行结束清除
        /// </summary>
        /// <param name="threads">线程组</param>
        /// <param name="reactor">线程组取消管理</param>
        public void Attach(MagicThread[] threads, CancellationTokenSource reactor)
        {
            Parallel.ForEach(threads, (thread) =>
            {
                this.Attach(thread, reactor);
            });
        }

        /// <summary>
        /// 搜索线程
        /// </summary>
        /// <param name="key">线程标识</param>
        /// <returns>MagicThread</returns>
        public MagicThread Search(string key)
        {
            MagicThread thread = null;
            {
                lock (this.taskLibrary)
                {
                    if (this.taskLibrary.ContainsKey(key))
                    {
                        thread = this.taskLibrary[key] as MagicThread;
                    }
                }
            }
            return thread;
        }

        #endregion
    }
}