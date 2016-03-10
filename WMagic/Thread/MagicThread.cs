using System;
using System.Threading;
using System.Threading.Tasks;

namespace WMagic.Thread
{
    /// <remarks>
    /// -----------------------------------------------------------------------
    /// 部件名：WMagicThread
    /// 工程名：WMagic
    /// 版权：CopyRight (c) 2013
    /// 创建人：WY
    /// 描述：线程类
    /// 创建日期：2013.05.17
    /// 修改人：ZFL
    /// 修改日期：2013.05.20
    /// -----------------------------------------------------------------------
    /// </remarks>

    /// <summary>
    ///  线程类
    /// </summary>
    public class MagicThread
    {
        #region 变量

        // 线程标识
        private string key;
        // 线程执行函数
        private Action fun;
        // 线程创建选项
        private TaskCreationOptions policy;
        // 线程取消管理
        private CancellationTokenSource reactor;

        #endregion

        #region 属性

        public bool IsCancel
        {
            get { return this.reactor != null ? this.reactor.IsCancellationRequested : true; }
        }

        public string Key
        {
            get { return this.key; }
            set
            {
                this.key = value;
            }
        }

        public Action Fun
        {
            get { return this.fun; }
            set
            {
                this.fun = value;
            }
        }

        public TaskCreationOptions Policy
        {
            get { return this.policy; }
            set
            {
                this.policy = value;
            }
        }

        public CancellationTokenSource Reactor
        {
            get
            {
                return this.reactor;
            }
            internal set
            {
                this.reactor = value;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">线程标识</param>
        /// <param name="fun">线程执行函数</param>
        public MagicThread(string key, Action fun)
            : this(key, fun, TaskCreationOptions.PreferFairness)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">线程标识</param>
        /// <param name="fun">线程执行函数</param>
        /// <param name="policy">线程创建可选行为</param>
        public MagicThread(string key, Action fun, TaskCreationOptions policy)
        {
            this.key = key;
            this.fun = fun;
            this.policy = policy;
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 取消线程
        /// </summary>
        public void Cancel()
        {
            if (this.reactor != null)
                this.reactor.Cancel();
        }

        #endregion
    }
}