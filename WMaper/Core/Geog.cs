using System.Linq;
using System.Windows;
using WMagic;
using WMaper.Base;
using WMaper.Meta.Store;
using WMaper.Proj;
using WMaper.Proj.Epsg;

namespace WMaper.Core
{
    /// <summary>
    /// 栅格抽象类
    /// </summary>
    public abstract class Geog : Layer
    {
        #region 变量

        // 是否图层可用
        private bool allow;
        // 是否专题图层
        private bool cover;
        // 起始层级
        private int start;
        // 结束层级
        private int close;
        // 基数层级
        private int radix;
        // 当前层级
        private int level;
        // 拖动速度
        private int speed;
        // 瓦片分辨率
        private int craft;
        // 图层单位
        private double units;
        // 图层原点
        private Coord origin;
        // 图层范围
        private Bound extent;
        // 图层尺寸
        private Nature nature;
        // 图层投影
        private Projcs projcs;
        // 图层分辨率
        private double[] factor;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Geog()
            : base()
        {
            this.start = 0;
            this.close = 0;
            this.radix = 0;
            this.level = 0;
            this.speed = 30;
            this.craft = 72;
            this.allow = true;
            this.cover = false;
            this.units = WMaper.Units.DD;
            this.projcs = new EPSG_4326();
        }

        #endregion

        #region 属性方法

        public bool Allow
        {
            get { return this.allow; }
            set { this.allow = value; }
        }

        public bool Cover
        {
            get { return this.cover; }
            set { this.cover = value; }
        }

        public int Start
        {
            get { return this.start; }
            set
            {
                if (value >= 0)
                {
                    try
                    {
                        this.factor = this.factor.Skip(value).ToArray();
                    }
                    catch
                    {
                        return;
                    }
                    this.start = value;
                }
            }
        }

        public int Close
        {
            get { return this.close; }
            set
            {
                if (value > 0)
                {
                    try
                    {
                        this.factor = this.factor.Take(value).ToArray();
                    }
                    catch
                    {
                        return;
                    }
                    this.close = value;
                }
            }
        }

        public int Radix
        {
            get { return this.radix; }
            set { this.radix = value; }
        }

        public int Level
        {
            get { return this.level; }
            set { this.level = value; }
        }

        public int Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }

        public int Craft
        {
            get { return this.craft; }
            set { this.craft = value; }
        }

        public double Units
        {
            get { return this.units; }
            set { this.units = value; }
        }

        public Coord Origin
        {
            get { return this.origin; }
            set { this.origin = value; }
        }

        public Bound Extent
        {
            get { return this.extent; }
            set { this.extent = value; }
        }

        public Nature Nature
        {
            get { return this.nature; }
            set { this.nature = value; }
        }

        public Projcs Projcs
        {
            get { return this.projcs; }
            set { this.projcs = value; }
        }

        public double[] Factor
        {
            get { return this.factor; }
            set { this.factor = value; }
        }

        #endregion

        #region 抽象函数

        public abstract int Fusion(Maper drv);

        public abstract Pixel Cur2px(Pixel cur);

        public abstract Pixel Crd2px(Coord crd);

        public abstract Coord Px2crd(Pixel pel);

        public abstract double Deg2sc(double deg);

        public abstract void Access(bool use);

        public abstract void Zoomto(int num);

        public abstract void Moveto(Coord crd, bool swf);

        #endregion

        #region 函数方法

        public void Access()
        {
            this.Access(true);
        }

        public void Moveto(Coord crd)
        {
            this.Moveto(crd, false);
        }

        public void Moveto(Pixel pel)
        {
            this.Moveto(pel, false);
        }

        public void Moveto(Pixel pel, bool swf)
        {
            this.Moveto(this.Px2crd(pel), swf);
        }

        public double Deg2sc()
        {
            return !MatchUtils.IsEmpty(this.factor) && this.level >= 0 ? this.Deg2sc(this.factor[this.level]) : 0.0;
        }

        public Pixel Cur2px(Point pos)
        {
            return this.Cur2px(new Pixel(pos.X, pos.Y));
        }

        /// <summary>
        /// 布局适应函数
        /// </summary>
        public void Resize()
        {
            if (!MatchUtils.IsEmpty(this.Target.Center))
            {
                this.Moveto(this.Target.Center, false);
            }
        }

        /// <summary>
        /// 图层移除函数
        /// </summary>
        public sealed override void Remove()
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (this.Facade.Equals(this.Target.Vessel.Tile))
                {
                    if (this.Target.Symbol.Tile.Remove(this.Index))
                    {
                        this.Facade.Children.Clear();
                        {
                            this.Facade = null;
                            this.Target = null;
                        }
                    }
                }
                else
                {
                    if (this.Obscure(this.Target.Listen.DragEvent, this.Redraw))
                    {
                        if (!this.Obscure(this.Target.Listen.ZoomEvent, this.Redraw))
                        {
                            this.Observe(this.Target.Listen.DragEvent, this.Redraw, 1);
                        }
                        else
                        {
                            if (!this.Obscure(this.Target.Listen.SwapEvent, this.Redraw))
                            {
                                this.Observe(this.Target.Listen.DragEvent, this.Redraw, 1);
                                this.Observe(this.Target.Listen.ZoomEvent, this.Redraw, 1);
                            }
                            else
                            {
                                if (!this.Target.Symbol.Pile.Remove(this.Index))
                                {
                                    this.Observe(this.Target.Listen.DragEvent, this.Redraw, 1);
                                    this.Observe(this.Target.Listen.ZoomEvent, this.Redraw, 1);
                                    this.Observe(this.Target.Listen.SwapEvent, this.Redraw, 1);
                                }
                                else
                                {
                                    this.Target.Vessel.Pile.Children.Remove(this.Facade);
                                    {
                                        this.Facade = null;
                                        this.Nature = null;
                                        this.Target = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}