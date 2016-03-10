using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using WMagic;
using WMagic.Brush;
using WMagic.Cache;
using WMagic.Thread;
using WMaper.Base;
using WMaper.Core;
using WMaper.Lang;
using WMaper.Meta.Param;
using WMaper.Meta.Store;
using WMaper.Misc;
using WMaper.Misc.View;

namespace WMaper
{
    // 地图单位
    public static class Units
    {
        // 米
        public static readonly double M = 39.3701;
        // 度
        public static readonly double DD = 4374780.0;
        // 英寸
        public static readonly double IN = 1.0;
        // 英尺
        public static readonly double FT = 12.0;
        // 千米
        public static readonly double KM = 39370.1;
    }

    // 地图对象
    public sealed class Maper
    {
        private string version = "1.0";

        public string Version
        {
            get { return this.version; }
        }

        #region 变量

        // 是否漫游
        private bool ramble;
        // 是否启用
        private bool enable;
        // 是否手动
        private bool manual;
        // 地图对象
        private Geog netmap;
        // 地图中心
        private Coord center;
        // 地图资源
        private Share sharer;
        // 地图面板
        private Panel facade;
        // 地图图层
        private Atlas vessel;
        // 地图容器
        private Canvas canvas;
        // 地图配置
        private Option option;
        // 地图语言
        private Assets assets;
        // 地图屏幕
        private Screen screen;
        // 地图符号
        private Symbol symbol;
        // 地图控件
        private Widget widget;
        // 地图监听
        private Listen listen;
        // 地图缓存
        private MagicCache memory;
        // 地图画笔
        private MagicGraphic sketch;
        // 地图线程
        private MagicThreadFactory thread;

        #endregion

        #region 属性方法

        public bool Ramble
        {
            get { return this.ramble; }
            set { this.ramble = value; }
        }

        public bool Enable
        {
            get { return this.enable; }
            set { this.enable = value; }
        }

        public bool Manual
        {
            get { return this.manual; }
            set { this.manual = value; }
        }

        public Geog Netmap
        {
            get { return this.netmap; }
            set { this.netmap = value; }
        }

        public Coord Center
        {
            get { return this.center; }
            set { this.center = value; }
        }

        public Share Sharer
        {
            get { return this.sharer; }
        }

        public Panel Facade
        {
            get { return this.facade; }
        }

        public Atlas Vessel
        {
            get { return this.vessel; }
        }

        public Canvas Canvas
        {
            get { return this.canvas; }
        }

        public Option Option
        {
            get { return this.option; }
        }

        public Assets Assets
        {
            get { return this.assets; }
        }

        public Screen Screen
        {
            get { return this.screen; }
        }

        public Symbol Symbol
        {
            get { return this.symbol; }
        }

        public Widget Widget
        {
            get { return this.widget; }
        }

        public Listen Listen
        {
            get { return this.listen; }
        }

        public MagicCache Memory
        {
            get { return this.memory; }
        }

        public MagicGraphic Sketch
        {
            get { return this.sketch; }
        }

        public MagicThreadFactory Thread
        {
            get { return this.thread; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="facade">地图面板</param>
        /// <param name="option">地图配置</param>
        public Maper(Panel facade, Option option)
        {
            if (!MatchUtils.IsEmpty(this.facade = facade) && !MatchUtils.IsEmpty(this.option = option) && (
                this.enable = true
            ))
            {
                // 初始光标
                this.facade.Cursor = (this.sharer = new Share()).FreeCur;
                {
                    // 初始参数
                    this.ramble = this.option.Exist("Ramble") ? this.option.Fetch<bool>("Ramble") : true;
                    this.assets = new Assets(
                        this.option.Exist("Assets") ? this.option.Fetch<string>("Assets") : null
                    );
                }
                // 初始缓存
                this.screen = new Screen(
                    this.facade
                );
                this.symbol = new Symbol();
                this.widget = new Widget();
                this.listen = new Listen();
                {
                    this.listen.DragEvent = new Event(this);
                    this.listen.ZoomEvent = new Event(this);
                    this.listen.SwapEvent = new Event(this);
                }
                // 初始尺寸
                Binding wide = new Binding();
                wide.Source = this.facade;
                wide.Mode = BindingMode.OneWay;
                wide.Path = new PropertyPath("ActualWidth");
                Binding high = new Binding();
                high.Source = this.facade;
                high.Mode = BindingMode.OneWay;
                high.Path = new PropertyPath("ActualHeight");
                // 初始画布
                (this.canvas = new Canvas()).Background = new SolidColorBrush(Color.FromRgb(229, 227, 223));
                {
                    // 裁切显示区
                    this.canvas.ClipToBounds = true;
                    // 自适应宽度
                    this.canvas.SetBinding(Canvas.WidthProperty, wide);
                    // 自适应高度
                    this.canvas.SetBinding(Canvas.HeightProperty, high);
                }
                // 初始地图集
                this.vessel = new Atlas(10);
                {
                    // 自适应宽度
                    this.vessel.SetBinding(UserControl.WidthProperty, wide);
                    // 自适应高度
                    this.vessel.SetBinding(UserControl.HeightProperty, high);
                }
                // 实例化图层
                this.canvas.Children.Add(this.vessel);
                this.facade.Children.Add(this.canvas);
                // 初始化画笔
                this.sketch = new MagicGraphic(this.vessel.Draw);
                {
                    // 初始化线程
                    this.thread = new MagicThreadFactory();
                    // 初始化缓存
                    this.memory = new MagicCache(
                        "MaperCache_" + StampUtils.GetTimeStamp(), 300, 75, TimeSpan.Parse("00:10:00")
                    );
                }
                // 初始化布局
                this.facade.SizeChanged += (obj, evt) =>
                {
                    if (!this.screen.W.Equals(this.facade.ActualWidth) || !this.screen.H.Equals(this.facade.ActualHeight))
                    {
                        // 调整屏幕大小
                        this.screen.X = (this.screen.W = this.facade.ActualWidth) / 2;
                        this.screen.Y = (this.screen.H = this.facade.ActualHeight) / 2;
                        // 调整地图大小
                        if (!MatchUtils.IsEmpty(this.netmap))
                        {
                            this.netmap.Resize();
                        }
                    }
                };
                // 初始化漫游
                this.facade.MouseLeftButtonDown += new MouseButtonEventHandler(delegate(object downObj, MouseButtonEventArgs downEvt)
                {
                    if (!MatchUtils.IsEmpty(this.netmap) && this.netmap.Enable && this.enable && this.ramble && (
                        downEvt.Handled = downEvt.ClickCount.Equals(1)
                    ))
                    {
                        if (Mouse.Capture(this.facade, CaptureMode.SubTree))
                        {
                            Point spoint = downEvt.GetPosition(this.facade), epoint = new Point();
                            {
                                // 拖拽光标
                                this.facade.Cursor = this.sharer.DragCur;
                                {
                                    MouseEventHandler mevent = null;
                                    MouseButtonEventHandler sevent = null;
                                    // 绑定事件
                                    this.facade.MouseMove += (mevent = new MouseEventHandler(delegate(object moveObj, MouseEventArgs moveEvt)
                                    {
                                        // 捕捉光标
                                        if (this.manual = !(MatchUtils.IsEmpty(epoint = moveEvt.GetPosition(this.facade)) || Point.Equals(spoint, epoint)))
                                        {
                                            Canvas.SetTop(this.vessel, this.netmap.Nature.T - (epoint.Y = (spoint.Y - epoint.Y)));
                                            Canvas.SetLeft(this.vessel, this.netmap.Nature.L - (epoint.X = (spoint.X - epoint.X)));
                                        }
                                    }));
                                    this.facade.MouseLeftButtonUp += (sevent = new MouseButtonEventHandler(delegate(object stopObj, MouseButtonEventArgs stopEvt)
                                    {
                                        // 恢复光标
                                        this.facade.Cursor = this.sharer.FreeCur;
                                        {
                                            stopEvt.Handled = true;
                                            try
                                            {
                                                this.facade.MouseMove -= mevent;
                                                this.facade.MouseLeftButtonUp -= sevent;
                                            }
                                            catch
                                            { }
                                            finally
                                            {
                                                // 释放光标
                                                Mouse.Capture(this.facade, CaptureMode.None);
                                                try
                                                {
                                                    if (!MatchUtils.IsEmpty(this.center) && this.manual)
                                                    {
                                                        this.Netmap.Moveto(this.netmap.Crd2px(this.center).Offset(new Pixel(epoint.X, epoint.Y)));
                                                    }
                                                }
                                                catch
                                                { }
                                                finally
                                                {
                                                    this.manual = false;
                                                    {
                                                        spoint.X = 0;
                                                        spoint.Y = 0;
                                                        epoint.X = 0;
                                                        epoint.Y = 0;
                                                    }
                                                    mevent = null;
                                                    sevent = null;
                                                }
                                            }
                                        }
                                    }));
                                }
                            }
                        }
                    }
                });
            }
        }

        #endregion

        #region 函数方法

        public void Dispose()
        {
            if (!MatchUtils.IsEmpty(this.thread))
            {
                this.thread.Destroy();
                {
                    this.thread = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.widget))
            {
                this.widget.Dispose();
                {
                    this.widget = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.symbol))
            {
                this.symbol.Dispose();
                {
                    this.symbol = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.listen))
            {
                this.listen.Dispose();
                {
                    this.listen = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.vessel))
            {
                this.vessel.Dispose();
                {
                    this.vessel = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.option))
            {
                this.option.Clear();
                {
                    this.option = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.sharer))
            {
                this.sharer.Dispose();
                {
                    this.sharer = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.canvas))
            {
                this.canvas.Children.Clear();
                {
                    this.canvas = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.facade))
            {
                this.facade.Children.Clear();
                {
                    this.facade = null;
                }
            }
            // 内存释放
            {
                this.assets = null;
                this.screen = null;
                this.center = null;
                this.sketch = null;
                this.netmap = null;
            }
        }

        public bool Refresh(Layer elem)
        {
            try
            {
                elem.Redraw();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Refresh(Decor elem)
        {
            try
            {
                elem.Redraw();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Exclude(Layer elem)
        {
            try
            {
                elem.Remove();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Exclude(Decor elem)
        {
            try
            {
                elem.Remove();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Include(Layer elem)
        {
            try
            {
                elem.Render(this);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Include(Decor elem)
        {
            try
            {
                elem.Render(this);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public Bound Viewbox()
        {
            return this.Viewbox(true);
        }

        public Bound Viewbox(bool view)
        {
            if (!MatchUtils.IsEmpty(this.center) && !MatchUtils.IsEmpty(this.netmap))
            {
                Pixel middle = this.netmap.Crd2px(this.center);
                if (!MatchUtils.IsEmpty(middle))
                {
                    return view ? new Bound(
                        this.netmap.Px2crd(new Pixel(middle.X - this.screen.X, middle.Y - this.screen.Y)),
                        this.netmap.Px2crd(new Pixel(middle.X + this.screen.X, middle.Y + this.screen.Y))
                    ) : new Bound(
                        this.netmap.Px2crd(new Pixel(middle.X - this.screen.W, middle.Y - this.screen.H)),
                        this.netmap.Px2crd(new Pixel(middle.X + this.screen.W, middle.Y + this.screen.H))
                    );
                }
            }
            return null;
        }

        #endregion
    }
}