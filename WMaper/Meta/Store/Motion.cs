using System;
using WMaper.Meta.Radio;

namespace WMaper.Meta.Store
{
    public sealed class Motion<_INFORMATION_>
    {
        #region 变量

        private Action<Msger> fun;
        private _INFORMATION_ msg;

        #endregion

        #region 属性方法

        public Action<Msger> Fun
        {
            get { return this.fun; }
        }

        public _INFORMATION_ Msg
        {
            get { return this.msg; }
        }

        #endregion

        #region 构造函数

        public Motion(Action<Msger> fun, _INFORMATION_ msg)
        {
            this.fun = fun;
            this.msg = msg;
        }

        #endregion
    }
}