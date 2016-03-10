using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using WMagic;
using WMagic.Anime;
using WMagic.Image;
using WMagic.Thread;
using WMaper.Base;
using WMaper.Core;
using WMaper.Meta;
using WMaper.Meta.Radio;
using WMaper.Meta.Store;
using WMaper.Misc.View.Ware;

namespace WMaper.Plat
{
    /// <summary>
    /// 瓦片图层类
    /// </summary>
    public abstract class Tile : Geog
    {
        // 漫游动画
        private static MagicAnime anime;

        #region 变量

        // 是否警示
        private bool alarm;
        // 瓦片属性
        private Block block;
        // 瓦片排序
        private Random upset;

        #endregion

        #region 属性方法

        public bool Alarm
        {
            get { return this.alarm; }
            set { this.alarm = value; }
        }

        public Block Block
        {
            get { return this.block; }
            set { this.block = value; }
        }

        #endregion

        #region 构造函数

        public Tile()
            : base()
        {
            this.alarm = true;
            this.upset = new Random();
            this.block = new Block(256, 256);
        }

        #endregion

        #region 抽象函数

        /// <summary>
        /// 获取单个瓦片图源
        /// </summary>
        /// <param name="l">地图层级</param>
        /// <param name="r">瓦片所在行</param>
        /// <param name="c">瓦片所在列</param>
        /// <returns>瓦片图源</returns>
        protected abstract string Source(int l, int r, int c);

        #endregion

        #region 函数方法

        protected virtual ImageSource Repair(int l, int r, int c)
        {
            return !this.Cover && this.alarm ? this.Target.Sharer.CauseSrc : this.Target.Sharer.BlankSrc;
        }

        /// <summary>
        /// 获得瓦片左上角X坐标
        /// </summary>
        /// <param name="l">地图层级</param>
        /// <param name="c">瓦片所在列</param>
        /// <param name="x">参照X轴坐标</param>
        /// <returns></returns>
        protected virtual long Axis4x(int l, int c, double x)
        {
            return Convert.ToInt64(Math.Round(c * this.block.Wide - x));
        }

        /// <summary>
        /// 获得瓦片左上角Y坐标
        /// </summary>
        /// <param name="l">地图层级</param>
        /// <param name="c">瓦片所在行</param>
        /// <param name="y">参照Y轴坐标</param>
        /// <returns></returns>
        protected virtual long Axis4y(int l, int r, double y)
        {
            return Convert.ToInt64(Math.Round(r * this.block.High - y));
        }

        /// <summary>
        /// 瓦片分辨率转比例尺
        /// </summary>
        /// <param name="deg">分辨率</param>
        /// <returns>比例尺</returns>
        public sealed override double Deg2sc(double deg)
        {
            return Math.Round(deg * this.Craft * this.Units);
        }

        /// <summary>
        /// 鼠标坐标转像素坐标
        /// </summary>
        /// <param name="cur">鼠标坐标</param>
        /// <returns>像素坐标</returns>
        public sealed override Pixel Cur2px(Pixel cur)
        {
            if (MatchUtils.IsEmpty(cur))
            {
                return null;
            }
            else
            {
                Pixel cur2px = this.Crd2px(this.Target.Center);
                try
                {
                    return !MatchUtils.IsEmpty(cur2px) ? new Pixel(
                        cur.X + cur2px.X - this.Target.Screen.L - this.Target.Screen.X,
                        cur.Y + cur2px.Y - this.Target.Screen.T - this.Target.Screen.Y
                    ) : null;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 经纬度坐标转像素坐标
        /// </summary>
        /// <param name="crd"></param>
        /// <returns></returns>
        public sealed override Pixel Crd2px(Coord crd)
        {
            if (MatchUtils.IsEmpty(crd))
            {
                return null;
            }
            else
            {
                Coord origin = this.Projcs.Decode(this.Origin), crd2px = this.Projcs.Decode(crd);
                try
                {
                    return !MatchUtils.IsEmpty(origin) && !MatchUtils.IsEmpty(crd2px) ? new Pixel(
                        (crd2px.Lng - origin.Lng) / this.Factor[this.Level],
                        (origin.Lat - crd2px.Lat) / this.Factor[this.Level]
                    ) : null;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 像素坐标转经纬度坐标 
        /// </summary>
        /// <param name="pel"></param>
        /// <returns></returns>
        public sealed override Coord Px2crd(Pixel pel)
        {
            if (MatchUtils.IsEmpty(pel))
            {
                return null;
            }
            else
            {
                Coord origin = this.Projcs.Decode(this.Origin);
                try
                {
                    return !MatchUtils.IsEmpty(origin) ? this.Projcs.Encode(new Coord(
                        origin.Lng + pel.X * this.Factor[this.Level],
                        origin.Lat - pel.Y * this.Factor[this.Level]
                    )) : null;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 图层缩放
        /// </summary>
        public sealed override void Zoomto(int num)
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Equals(this.Target.Netmap) && this.Target.Enable && this.Enable && this.Level != num)
            {
                double deg = 0.0;
                try
                {
                    deg = this.Factor[num];
                }
                catch
                {
                    deg = 0.0;
                }
                finally
                {
                    if (deg > 0)
                    {
                        Pixel middle = this.Crd2px(this.Target.Center);
                        if (!MatchUtils.IsEmpty(middle))
                        {
                            // 缩放比率
                            double ratio = this.Factor[this.Level] / this.Factor[this.Level = num];
                            {
                                // 缩略地图
                                this.Thumb(ratio, new Pixel(
                                    middle.X - this.Nature.X,
                                    middle.Y - this.Nature.Y
                                ));
                                // 地图属性
                                this.Grasp(middle.Ratio(ratio), this.Nature);
                                // 事件通知
                                this.Trigger(this.Target.Listen.ZoomEvent, ratio);
                            }
                            // 延时重绘
                            this.Target.Thread.Attach(new MagicThread(StampUtils.GetSequence(), () =>
                            {
                                {
                                    Thread.Sleep(700);
                                }
                                this.Build(this.Crd2px(this.Target.Center));
                            }));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 图层移动
        /// </summary>
        public sealed override void Moveto(Coord pos, bool swf)
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Equals(this.Target.Netmap) && this.Target.Enable && this.Enable)
            {
                try
                {
                    pos = this.Extent.Correct(this.Target, this.Target.Option.Exist("Limits") ? this.Target.Option.Fetch<Bound>("Limits").Correct(this.Target, pos) : pos);
                }
                catch
                {
                    pos = null;
                }
                finally
                {
                    if (pos != null && (!pos.Compare(this.Target, this.Target.Center) || (new StackFrame(1)).GetMethod().Equals(typeof(Geog).GetMethod("Resize"))))
                    {
                        // 动画移动
                        if (swf)
                        {
                            AnimeUtils.Launch(new Action<MagicAnime, Coord, Tile>((anime, point, maper) =>
                            {
                                try
                                {
                                    if (!MatchUtils.IsEmpty(Tile.anime) && !Tile.anime.Equals(anime))
                                    {
                                        Tile.anime.Abort();
                                    }
                                }
                                catch
                                {
                                    anime.Abort();
                                    {
                                        anime = null;
                                        point = null;
                                        maper = null;
                                    }
                                }
                                finally
                                {
                                    if (!MatchUtils.IsEmpty(Tile.anime = anime))
                                    {
                                        if (!maper.Equals(maper.Target.Netmap) || maper.Target.Manual)
                                        {
                                            maper.Noswf();
                                            {
                                                anime = null;
                                                point = null;
                                                maper = null;
                                            }
                                        }
                                        else
                                        {
                                            Pixel middle = maper.Crd2px(maper.Target.Center), finish = maper.Crd2px(point);
                                            if (!MatchUtils.IsEmpty(middle) && !MatchUtils.IsEmpty(finish))
                                            {
                                                double dev, inv;
                                                // 移动地图
                                                if (finish.Compare(maper.Target, middle.Offset(
                                                    (inv = Math.Abs(dev = finish.X - middle.X)) > maper.Speed ? maper.Speed * inv / dev : dev,
                                                    (inv = Math.Abs(dev = finish.Y - middle.Y)) > maper.Speed ? maper.Speed * inv / dev : dev
                                                )))
                                                {
                                                    maper.Noswf();
                                                    {
                                                        maper.Moveto(finish);
                                                    }
                                                }
                                                else
                                                {
                                                    maper.Moveto(middle);
                                                }
                                            }
                                        }
                                    }
                                }
                            }), new Object[] { pos, this });
                        }
                        else
                        {
                            Pixel middle = this.Crd2px(pos);
                            if (!MatchUtils.IsEmpty(middle))
                            {
                                // 地图位置
                                Canvas.SetTop(this.Target.Vessel, this.Nature.T = Math.Round(this.Nature.Y + this.Target.Screen.Y - middle.Y));
                                Canvas.SetLeft(this.Target.Vessel, this.Nature.L = Math.Round(this.Nature.X + this.Target.Screen.X - middle.X));
                                // 重绘地图
                                {
                                    this.Build(middle);
                                }
                                // 事件通知
                                this.Trigger(this.Target.Listen.DragEvent, this.Target.Center = pos);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 切换地图
        /// </summary>
        public sealed override void Access(bool use)
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (this.Cover)
                {
                    if ((this.Level = this.Fusion(this.Target)) > -1)
                    {
                        this.Allow = use;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (use && this.Allow && !this.Equals(this.Target.Netmap))
                    {
                        if (!MatchUtils.IsEmpty(this.Target.Netmap) && !MatchUtils.IsEmpty(this.Target.Netmap.Nature))
                        {
                            this.Nature = new Nature(this.Target.Netmap.Nature.L, this.Target.Netmap.Nature.T);
                            {
                                // 适配比例
                                double d2s = this.Target.Netmap.Deg2sc();
                                {
                                    double[] dev = this.Factor.Select(deg => Math.Abs(this.Deg2sc(deg) - d2s)).ToArray();
                                    if (!MatchUtils.IsEmpty(dev))
                                    {
                                        this.Level = Array.IndexOf(dev, dev.Min());
                                    }
                                }
                            }
                        }
                        // 切换地图
                        (this.Target.Netmap = this).Redraw();
                    }
                    else
                    {
                        return;
                    }
                }
                // 广播事件
                this.Trigger(this.Target.Listen.SwapEvent, this);
            }
        }

        /// <summary>
        /// 图层是否适配
        /// </summary>
        /// <param name="drv">匹配地图</param>
        /// <returns>匹配层级</returns>
        public sealed override int Fusion(Maper drv)
        {
            if (!MatchUtils.IsEmpty(drv) && !MatchUtils.IsEmpty(drv.Netmap) && !MatchUtils.IsEmpty(this.Factor))
            {
                double scale = drv.Netmap.Deg2sc() * 10;
                if (scale > 0)
                {
                    double stand;
                    // 计算容差
                    for (int i = 0, l = this.Factor.Length; i < l; i++)
                    {
                        if ((stand = Math.Round(scale / this.Deg2sc(this.Factor[i])) / 10) >= 1.0 && stand <= 1.1)
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 图层渲染函数
        /// </summary>
        /// <param name="drv">地图对象</param>
        /// <param name="job">回调函数</param>
        public sealed override void Render(Maper drv)
        {
            if (!MatchUtils.IsEmpty(this.Target = drv) && drv.Enable && !MatchUtils.IsEmpty(this.Factor))
            {
                this.Index = MatchUtils.IsEmpty(this.Index) ? (this.Index = StampUtils.GetTimeStamp()) : this.Index;
                if (this.Cover)
                {
                    if (drv.Symbol.Pile.ContainsKey(this.Index))
                    {
                        Geog geog = drv.Symbol.Pile[this.Index];
                        if (this.Equals(geog))
                        {
                            this.Access(this.Allow);
                        }
                        else
                        {
                            {
                                geog.Remove();
                            }
                            this.Render(drv);
                        }
                    }
                    else
                    {
                        int order = 1;
                        try
                        {
                            drv.Vessel.Pile.Children.Add(this.Facade = new Scene());
                        }
                        catch
                        {
                            order = 0;
                        }
                        finally
                        {
                            if (order == 1)
                            {
                                // 监听广播
                                this.Observe(drv.Listen.DragEvent, this.Redraw, 1);
                                this.Observe(drv.Listen.ZoomEvent, this.Redraw, 1);
                                this.Observe(drv.Listen.SwapEvent, this.Redraw, 1);
                                // 加载图层
                                drv.Symbol.Pile.Add(this.Index, this);
                                try
                                {
                                    this.Access(this.Allow);
                                }
                                catch
                                {
                                    drv.Symbol.Pile.Remove(this.Index);
                                }
                                finally
                                {
                                    if (drv.Symbol.Pile.ContainsKey(this.Index))
                                    {
                                        if (int.TryParse(this.Index, out order))
                                        {
                                            Canvas.SetZIndex(this.Facade, order);
                                        }
                                    }
                                    else
                                    {
                                        // 移除监听
                                        this.Obscure(drv.Listen.DragEvent, this.Redraw);
                                        this.Obscure(drv.Listen.ZoomEvent, this.Redraw);
                                        this.Obscure(drv.Listen.SwapEvent, this.Redraw);
                                        // 移除图层
                                        drv.Vessel.Pile.Children.Remove(this.Facade);
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
                else
                {
                    if (!drv.Symbol.Tile.ContainsKey(this.Index) && !MatchUtils.IsEmpty(
                        this.Facade = drv.Vessel.Tile
                    ))
                    {
                        drv.Symbol.Tile.Add(this.Index, this);
                        try
                        {
                            this.Access(MatchUtils.IsEmpty(drv.Netmap));
                        }
                        catch
                        {
                            drv.Symbol.Tile.Remove(this.Index);
                            {
                                this.Facade = null;
                                this.Target = null;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 图层重绘函数
        /// </summary>
        /// <param name="msg">重绘消息</param>
        public sealed override void Redraw(Msger msg)
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable && !MatchUtils.IsEmpty(this.Facade))
            {
                if (MatchUtils.IsEmpty(msg))
                {
                    this.Redraw();
                }
                else
                {
                    // Drag Event.
                    if (this.Target.Listen.DragEvent.Equals(msg.Chan))
                    {
                        if (this.Allow && !MatchUtils.IsEmpty(this.Nature))
                        {
                            if (this.Extent.Contain(this.Target, msg.Info as Coord))
                            {
                                this.Build(this.Crd2px(msg.Info as Coord));
                            }
                            else
                            {
                                this.Facade.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    this.Facade.Children.Clear();
                                }));
                            }
                        }
                        return;
                    }
                    // Zoom Event.
                    if (this.Target.Listen.ZoomEvent.Equals(msg.Chan))
                    {
                        if (this.Allow)
                        {
                            if ((this.Level = this.Fusion(this.Target)) > -1)
                            {
                                Pixel middle = this.Crd2px(this.Target.Center);
                                if (!MatchUtils.IsEmpty(this.Nature))
                                {
                                    // 缩略地图
                                    this.Thumb((double)msg.Info, new Pixel(
                                        middle.X / (double)msg.Info - this.Nature.X,
                                        middle.Y / (double)msg.Info - this.Nature.Y
                                    ));
                                    // 地图属性
                                    this.Grasp(middle, this.Nature);
                                }
                                else
                                {
                                    // 地图属性
                                    this.Grasp(middle, this.Nature = new Nature());
                                }
                                // 延时重绘
                                if (this.Extent.Contain(this.Target, this.Target.Center))
                                {
                                    this.Target.Thread.Attach(new MagicThread(StampUtils.GetSequence(), () =>
                                    {
                                        {
                                            Thread.Sleep(700);
                                        }
                                        this.Build(this.Crd2px(this.Target.Center));
                                    }));
                                }
                            }
                            else
                            {
                                if (!MatchUtils.IsEmpty(this.Nature))
                                {
                                    this.Nature = null;
                                    {
                                        this.Facade.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            this.Facade.Children.Clear();
                                        }));
                                    }
                                }
                            }
                        }
                        return;
                    }
                    // Swap Event.
                    if (this.Target.Listen.SwapEvent.Equals(msg.Chan))
                    {
                        Geog geog = msg.Info as Geog;
                        if (!geog.Cover || this.Equals(geog))
                        {
                            this.Redraw();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 图层重绘函数
        /// </summary>
        public sealed override void Redraw()
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable && !MatchUtils.IsEmpty(this.Facade))
            {
                if (this.Cover)
                {
                    if (this.Allow && (this.Level = this.Fusion(this.Target)) > -1)
                    {
                        Pixel middle = this.Crd2px(this.Target.Center);
                        if (!MatchUtils.IsEmpty(middle))
                        {
                            // 地图属性
                            this.Grasp(middle, (
                                MatchUtils.IsEmpty(this.Nature) ? this.Nature = new Nature() : this.Nature
                            ));
                            // 绘制地图
                            if (this.Extent.Contain(this.Target, this.Target.Center))
                            {
                                this.Build(middle);
                            }
                        }
                    }
                    else
                    {
                        if (!MatchUtils.IsEmpty(this.Nature))
                        {
                            this.Nature = null;
                            {
                                this.Facade.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    this.Facade.Children.Clear();
                                }));
                            }
                        }
                    }
                }
                else
                {
                    if (!MatchUtils.IsEmpty(this.Factor) && this.Allow && (
                        !MatchUtils.IsEmpty(this.Target.Center) || this.Target.Option.Exist("Center")
                    ))
                    {
                        Coord point = MatchUtils.IsEmpty(this.Target.Center) ? this.Target.Option.Fetch<Coord>("Center") : this.Target.Center;
                        if (!MatchUtils.IsEmpty(point))
                        {
                            bool done = true;
                            try
                            {
                                done = this.Factor[this.Level] > 0;
                            }
                            catch
                            {
                                done = this.Factor[this.Level = 0] > 0;
                            }
                            finally
                            {
                                if (done)
                                {
                                    Coord center = null;
                                    try
                                    {
                                        center = this.Extent.Correct(this.Target, this.Target.Option.Exist("Limits") ? this.Target.Option.Fetch<Bound>("Limits").Correct(this.Target, point) : point);
                                    }
                                    catch
                                    {
                                        center = null;
                                    }
                                    finally
                                    {
                                        if (center != null)
                                        {
                                            Pixel middle = this.Crd2px(center);
                                            if (!MatchUtils.IsEmpty(middle))
                                            {
                                                this.Target.Center = center;
                                                {
                                                    // 地图属性
                                                    this.Grasp(middle, (
                                                        MatchUtils.IsEmpty(this.Nature) ? this.Nature = new Nature() : this.Nature
                                                    ));
                                                }
                                                // 绘制地图
                                                this.Build(middle);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 私有方法

        private void Noswf()
        {
            if (!MatchUtils.IsEmpty(Tile.anime))
            {
                Tile.anime.Abort();
                {
                    Tile.anime = null;
                }
            }
        }

        private void Dimen(Bound bnd, Nature geo)
        {
            if (!MatchUtils.IsEmpty(bnd.Min) && !MatchUtils.IsEmpty(bnd.Max))
            {
                double deg = 0.0;
                try
                {
                    deg = this.Factor[this.Level];
                }
                catch
                {
                    deg = 0.0;
                }
                finally
                {
                    if (deg > 0)
                    {
                        geo.W = Math.Floor((bnd.Max.Lng - bnd.Min.Lng) / deg / this.block.Wide) * this.block.Wide;
                        geo.H = Math.Floor((bnd.Min.Lat - bnd.Max.Lat) / deg / this.block.High) * this.block.High;
                    }
                }
            }
        }

        /// <summary>
        /// 设置地图属性
        /// </summary>
        /// <param name="c2p">视图中点像素坐标</param>
        /// <param name="geo">图层尺寸信息</param>
        private void Grasp(Pixel c2p, Nature geo)
        {
            // 屏幕原点坐标
            geo.X = this.Target.Netmap.Nature.L - this.Target.Screen.X + c2p.X;
            geo.Y = this.Target.Netmap.Nature.T - this.Target.Screen.Y + c2p.Y;
            // 地图实际尺寸
            if (!MatchUtils.IsEmpty(this.Extent))
            {
                this.Dimen(new Bound(this.Projcs.Decode(this.Origin), this.Projcs.Decode(this.Extent.Max)), geo);
            }
        }

        /// <summary>
        /// 缩略地图
        /// </summary>
        /// <param name="per">缩放比例倒数</param>
        /// <param name="pos">视图中心相对Canvas的坐标</param>
        private void Thumb(double per, Pixel pos)
        {
            this.Facade.Dispatcher.BeginInvoke(new Action(() =>
            {
                // 缩略瓦片
                Image[] patchs = this.Facade.Children.Cast<Image>().ToArray();
                foreach (Image patch in patchs)
                {
                    // Thumb Size.
                    patch.Width *= per;
                    patch.Height *= per;
                    // Thumb Seat.
                    Canvas.SetTop(patch, Math.Round(pos.Y - (pos.Y - Canvas.GetTop(patch)) * per));
                    Canvas.SetLeft(patch, Math.Round(pos.X - (pos.X - Canvas.GetLeft(patch)) * per));
                }
            }));
        }

        private Assem Bound(Pixel c2p)
        {
            double zone;
            {
                return new Assem(
                    (zone = c2p.Y - this.Target.Screen.H) < 0 ? 0 : Convert.ToInt32(Math.Floor(zone / this.block.High)),
                    (zone = c2p.X - this.Target.Screen.W) < 0 ? 0 : Convert.ToInt32(Math.Floor(zone / this.block.Wide)),
                    (zone = c2p.Y + this.Target.Screen.H) < this.Nature.H ? Convert.ToInt32(Math.Floor(zone / this.block.High)) : (
                        this.Nature.H > this.block.High ? Convert.ToInt32(Math.Floor(this.Nature.H / this.block.High - 1)) : 0
                    ),
                    (zone = c2p.X + this.Target.Screen.W) < this.Nature.W ? Convert.ToInt32(Math.Floor(zone / this.block.Wide)) : (
                        this.Nature.W > this.block.Wide ? Convert.ToInt32(Math.Floor(this.Nature.W / this.block.Wide - 1)) : 0
                    )
                );
            }
        }

        private List<Patch> Patch(Pixel c2p)
        {
            Assem assem = null;
            try
            {
                // 计算瓦片行列
                assem = this.Bound(c2p);
            }
            catch
            {
                return null;
            }
            // 生成瓦片集合
            List<Patch> patchs = new List<Patch>();
            for (int i = assem.MinR; i <= assem.MaxR; i++)
            {
                for (int j = assem.MinC; j <= assem.MaxC; j++)
                {
                    patchs.Add(new Patch(this.Level + i + this.Index + j, this.Level, i, j));
                }
            }
            return patchs.OrderBy(x => this.upset.Next()).ToList();
        }

        /// <summary>
        /// 异步加载瓦片
        /// </summary>
        /// <param name="c2p">显示中心</param>
        private void Build(Pixel c2p)
        {
            // 计算栅格图片
            List<Patch> patchs = this.Patch(c2p);
            if (!MatchUtils.IsEmpty(patchs))
            {
                this.Facade.Dispatcher.BeginInvoke(new Action(() =>
                {
                    UIElementCollection ware = this.Facade.Children;
                    {
                        bool erase = true;
                        {
                            Image[] tiles = ware.Cast<Image>().ToArray();
                            foreach (Image tile in tiles)
                            {
                                erase = true;
                                {
                                    for (int i = patchs.Count - 1; i > -1; i--)
                                    {
                                        if (patchs[i].Uid.Equals(tile.Uid))
                                        {
                                            patchs.RemoveAt(i);
                                            {
                                                erase = false;
                                            }
                                            break;
                                        }
                                    }
                                    if (erase)
                                    {
                                        // 终止加载
                                        this.Target.Thread.Detach((MagicThread)tile.Tag);
                                        {
                                            tile.Uid = null;
                                            tile.Tag = null;
                                        }
                                        ware.Remove(tile);
                                    }
                                }
                            }
                            // 释放内存
                            Array.Clear(tiles, 0, tiles.Length);
                        }
                        // 加载瓦片
                        Image mesh = null;
                        {
                            foreach (Patch patch in patchs)
                            {
                                ware.Add(mesh = new Image());
                                {
                                    mesh.Uid = patch.Uid;
                                    mesh.Width = this.Block.Wide;
                                    mesh.Height = this.Block.High;
                                    mesh.Opacity = this.Alpha / 100.0;
                                    // 瓦片位置
                                    {
                                        Canvas.SetTop(mesh, this.Axis4y(patch.Num, patch.Row, this.Nature.Y));
                                        Canvas.SetLeft(mesh, this.Axis4x(patch.Num, patch.Col, this.Nature.X));
                                    }
                                }
                                // 异步下载
                                mesh.Tag = MagicImage.AsyncLoad(this.Source(patch.Num, patch.Row, patch.Col), this.Target.Memory, this.Target.Thread, new Action<ImageSource, int, int, int, Image>((data, l, r, c, tile) =>
                                {
                                    tile.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        if (!MatchUtils.IsEmpty(tile.Uid))
                                        {
                                            if (MatchUtils.IsEmpty(data))
                                            {
                                                try
                                                {
                                                    data = this.Repair(l, r, c);
                                                }
                                                catch
                                                {
                                                    data = null;
                                                }
                                                finally
                                                {
                                                    if (!MatchUtils.IsEmpty(data) && data.CanFreeze)
                                                    {
                                                        data.Freeze();
                                                    }
                                                }
                                            }
                                            // 绘制瓦片
                                            tile.Source = data;
                                        }
                                    }));
                                }), new Object[] { patch.Num, patch.Row, patch.Col, mesh });
                            }
                            // 释放内存
                            mesh = null;
                        }
                    }
                    // 释放内存
                    patchs.Clear();
                }));
            }
        }

        #endregion
    }
}