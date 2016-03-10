using System;
using WMagic;
using WMagic.Brush.Shape;
using WMaper.Core;
using WMaper.Meta.Param;

namespace WMaper.Plot
{
    public sealed class Text : Geom
    {
        #region 变量

        #endregion

        #region 属性方法

        #endregion

        #region 构造函数

        public Text()
            : base()
        { }

        public Text(Option option)
            : this()
        { }

        #endregion

        #region 函数方法

        public sealed override void Redraw()
        {
            if (!MatchUtils.IsEmpty(this.Target) && !MatchUtils.IsEmpty(this.Target.Netmap) && !MatchUtils.IsEmpty(this.Facade) && this.Target.Enable && this.Enable)
            {
                bool hide = this.Matte || !this.Viewble(this.Arise);
                {
                    if (!MatchUtils.IsEmpty(this.Handle))
                    {
                        if (!(this.Handle.Matte = hide))
                        {
                            GText v_text = this.Handle as GText;
                            {

                            }
                        }
                        // 重绘图形
                        this.Facade.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (!MatchUtils.IsEmpty(this.Handle))
                            {
                                this.Handle.Paint();
                            }
                        }));
                    }
                    else
                    {
                        if (!((this.Handle = this.Target.Sketch.CreateText()).Matte = hide))
                        {
                            GText v_text = this.Handle as GText;
                            {

                            }
                            // 绘制图形
                            this.Facade.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (!MatchUtils.IsEmpty(this.Handle))
                                {
                                    v_text.Paint();
                                }
                            }));
                        }
                    }
                }
            }
        }

        #endregion
    }
}