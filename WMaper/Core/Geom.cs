using System.Collections.Generic;
using System.Windows.Input;
using WMagic;
using WMagic.Brush.Basic;
using WMaper.Base;
using WMaper.Meta;
using WMaper.Meta.Radio;

namespace WMaper.Core
{
    public abstract class Geom : Layer
    {
        #region 变量

        // 是否可见
        private bool matte;
        // 是否修改
        private bool amend;
        // 显示范围
        private Arise arise;
        // 显示光标
        private Cursor mouse;
        // 几何对象
        private AShape handle;

        #endregion

        #region 构造函数

        public Geom()
            : base()
        {
            this.arise = null;
            this.matte = false;
            this.amend = false;
            this.handle = null;
        }

        #endregion

        #region 属性方法

        public bool Matte
        {
            get { return this.matte; }
            set { this.matte = value; }
        }

        public bool Amend
        {
            get { return this.amend; }
            set { this.amend = value; }
        }

        public Arise Arise
        {
            get { return this.arise; }
            set { this.arise = value; }
        }

        public Cursor Mouse
        {
            get { return this.mouse; }
            set { this.mouse = value; }
        }

        public AShape Handle
        {
            get { return this.handle; }
            set { this.handle = value; }
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 点集合地理坐标转化为Canvas坐标
        /// </summary>
        /// <param name="way"></param>
        /// <returns></returns>
        protected List<GPoint> Fit4r(List<Coord> way)
        {
            List<GPoint> fit = new List<GPoint>();
            // 坐标转换
            {
                if (!MatchUtils.IsEmpty(way))
                {
                    GPoint point = null;
                    {
                        foreach (Coord crd in way)
                        {
                            if (!MatchUtils.IsEmpty(point = this.Fit4p(crd)))
                            {
                                fit.Add(point);
                            }
                        }
                    }
                }
            }
            return fit;
        }

        /// <summary>
        /// 点地理坐标转换为Canvas坐标 
        /// </summary>
        /// <param name="crd">地理坐标</param>
        /// <returns>Canvas坐标</returns>
        protected GPoint Fit4p(Coord crd)
        {
            Pixel pel = null;
            try
            {
                if (!MatchUtils.IsEmpty(crd))
                {
                    pel = this.Target.Netmap.Crd2px(crd).Offset(new Pixel(
                        -this.Target.Netmap.Nature.X,
                        -this.Target.Netmap.Nature.Y
                    ));
                }
            }
            catch
            {
                pel = null;
            }
            return !MatchUtils.IsEmpty(pel) ? new GPoint(pel.X, pel.Y) : null;
        }

        /// <summary>
        /// 辐射地理范围转像素范围
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        protected GExtra Fit4e(GExtra ext)
        {
            if (!MatchUtils.IsEmpty(ext))
            {
                double d2m = this.Target.Netmap.Craft * WMaper.Units.M / this.Target.Netmap.Deg2sc();
                {
                    if (d2m > 0)
                    {
                        return new GExtra(ext.X * d2m, ext.Y * d2m);
                    }
                }
            }
            return new GExtra();
        }

        /// <summary>
        /// 置顶对象
        /// </summary>
        public void Summit()
        {
            if (!MatchUtils.IsEmpty(this.Target) && !MatchUtils.IsEmpty(this.handle) && this.Target.Enable && this.Enable)
            {
                this.handle.Front();
            }
        }

        /// <summary>
        /// 渲染对象
        /// </summary>
        /// <param name="drv"></param>
        public sealed override void Render(Maper drv)
        {
            if (!MatchUtils.IsEmpty(this.Target = drv) && this.Target.Enable)
            {
                this.Index = MatchUtils.IsEmpty(this.Index) ? (this.Index = StampUtils.GetTimeStamp()) : this.Index;
                if (drv.Symbol.Draw.ContainsKey(this.Index))
                {
                    Geom geom = drv.Symbol.Draw[this.Index];
                    if (this.Equals(geom))
                    {
                        this.Redraw();
                    }
                    else
                    {
                        {
                            geom.Remove();
                        }
                        this.Render(drv);
                    }
                }
                else
                {
                    if (!MatchUtils.IsEmpty(this.Facade = drv.Vessel.Draw))
                    {
                        // 监听广播
                        this.Observe(drv.Listen.ZoomEvent, this.Redraw);
                        this.Observe(drv.Listen.SwapEvent, this.Redraw);
                        // 绘制图形
                        try
                        {
                            (drv.Symbol.Draw[this.Index] = this).Redraw();
                        }
                        catch
                        {
                            drv.Symbol.Draw.Remove(this.Index);
                        }
                        finally
                        {
                            if (!drv.Symbol.Draw.ContainsKey(this.Index))
                            {
                                // 移除监听
                                this.Obscure(drv.Listen.ZoomEvent, this.Redraw);
                                this.Obscure(drv.Listen.SwapEvent, this.Redraw);
                                {
                                    this.Facade = null;
                                    this.Target = null;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绘制对象
        /// </summary>
        /// <param name="msg"></param>
        public sealed override void Redraw(Msger msg)
        {
            if (!MatchUtils.IsEmpty(this.Target) && !MatchUtils.IsEmpty(this.Target.Netmap) && !MatchUtils.IsEmpty(this.Facade) && this.Target.Enable && this.Enable)
            {
                if (MatchUtils.IsEmpty(msg))
                {
                    this.Redraw();
                }
                else
                {
                    if (!MatchUtils.IsEmpty(this.handle))
                    {
                        // Swap Event.
                        if (this.Target.Listen.ZoomEvent.Equals(msg.Chan))
                        {
                            this.Redraw(); return;
                        }
                        // Swap Event.
                        if (this.Target.Listen.SwapEvent.Equals(msg.Chan))
                        {
                            if (!(msg.Info as Geog).Cover)
                            {
                                this.Redraw();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        public sealed override void Remove()
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable && !MatchUtils.IsEmpty(this.handle))
            {
                if (this.Obscure(this.Target.Listen.ZoomEvent, this.Redraw))
                {
                    if (!this.Obscure(this.Target.Listen.SwapEvent, this.Redraw))
                    {
                        this.Observe(this.Target.Listen.ZoomEvent, this.Redraw);
                    }
                    else
                    {
                        if (!this.Target.Symbol.Draw.Remove(this.Index))
                        {
                            this.Observe(this.Target.Listen.ZoomEvent, this.Redraw);
                            this.Observe(this.Target.Listen.SwapEvent, this.Redraw);
                        }
                        else
                        {
                            // 擦除图形
                            this.handle.Earse();
                            {
                                this.Facade = null;
                                this.handle = null;
                                this.Target = null;
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}