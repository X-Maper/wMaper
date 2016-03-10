using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WMagic;

namespace WMaper.Misc.View.Plug
{
    public sealed partial class Slide : UserControl
    {
        #region 变量

        private bool ready;
        // 控件对象
        private WMaper.Plug.Slide slide;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Slide()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Slide(int sort)
            : this()
        {
            Panel.SetZIndex(this, sort);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="slide"></param>
        public Slide(WMaper.Plug.Slide slide, int sort)
            : this(sort)
        {
            this.ready = false;
            {
                (this.slide = slide).Facade = this.SlideDecor;
                {
                    // 语言资源
                    this.SlideResource.MergedDictionaries.Add(
                        this.slide.Target.Assets.Language
                    );
                }
            }
        }

        #endregion

        #region 函数方法

        #region Slide

        /// <summary>
        /// 构建缩放控件
        /// </summary>
        public void Build()
        {
            // 判断类型
            if (this.slide.Simple)
            {
                if (!this.ready && (this.ready = true))
                {
                    this.SlideLite.Visibility = Visibility.Visible;
                    this.SlideFull.Visibility = Visibility.Collapsed;
                    {
                        // 设置位置
                        if ("left".Equals(this.slide.Anchor))
                        {
                            Canvas.SetTop(this, 18);
                            Canvas.SetLeft(this, 18);
                        }
                        else
                        {
                            Canvas.SetTop(this, 18);
                            Canvas.SetRight(this, 53);
                        }
                    }
                }
            }
            else
            {
                if (!this.ready && (this.ready = true))
                {
                    this.SlideFull.Visibility = Visibility.Visible;
                    this.SlideLite.Visibility = Visibility.Collapsed;
                    {
                        // 设置位置
                        if ("left".Equals(this.slide.Anchor))
                        {
                            Canvas.SetTop(this, 18);
                            Canvas.SetLeft(this, 18);
                        }
                        else
                        {
                            Canvas.SetTop(this, 18);
                            Canvas.SetRight(this, 66);
                        }
                    }
                }
                // 绘制层级
                if (!MatchUtils.IsEmpty(this.slide.Target) && !MatchUtils.IsEmpty(this.slide.Target.Netmap) && !MatchUtils.IsEmpty(this.slide.Target.Netmap.Factor))
                {
                    int level = this.slide.Target.Netmap.Level, count = this.slide.Target.Netmap.Factor.Length, range = count - level;
                    {
                        this.SlideFullBar.Height = (count + 1) * 7 - 1;
                        // 滑块位置
                        Canvas.SetTop(this.SlideFullMask, range * 7 - 1);
                        Canvas.SetTop(this.SlideFullCtrl, range * 7 - 4);
                    }
                }
                else
                {
                    this.SlideFullBar.Height = 0;
                    // 滑块位置
                    Canvas.SetTop(this.SlideFullMask, 0);
                    Canvas.SetTop(this.SlideFullCtrl, 0);
                }
            }
        }

        #endregion

        #region SlideGrid_Full

        #region SlideGrid_Pan

        private void SlideFullP2n_MouseEnter(object obj, MouseEventArgs evt)
        {
            this.SlideFullPan.Background = this.FindResource("SLIDE_12") as ImageBrush;
        }

        private void SlideFullP2n_MouseLeave(object obj, MouseEventArgs evt)
        {
            this.SlideFullPan.Background = this.FindResource("SLIDE_13") as ImageBrush;
        }

        private void SlideFullP2n_MouseLeftButtonDown(object obj, MouseButtonEventArgs evt)
        {
            if (!MatchUtils.IsEmpty(this.slide.Target) && !MatchUtils.IsEmpty(this.slide.Target.Netmap))
            {
                MouseButtonEventHandler stopHnd = null;
                {
                    this.SlideFullP2n.MouseLeftButtonUp += (stopHnd = new MouseButtonEventHandler((stopObj, stopEvt) =>
                    {
                        this.SlideFullP2n.MouseLeftButtonUp -= stopHnd;
                        {
                            stopHnd = null;
                        }
                        this.slide.Target.Netmap.Moveto(
                            this.slide.Target.Netmap.Crd2px(this.slide.Target.Center).Offset(0, -this.slide.Target.Screen.Y), true
                        );
                    }));
                }
            }
        }

        private void SlideFullP2w_MouseEnter(object obj, MouseEventArgs evt)
        {
            this.SlideFullPan.Background = this.FindResource("SLIDE_11") as ImageBrush;
        }

        private void SlideFullP2w_MouseLeave(object obj, MouseEventArgs evt)
        {
            this.SlideFullPan.Background = this.FindResource("SLIDE_13") as ImageBrush;
        }

        private void SlideFullP2w_MouseLeftButtonDown(object obj, MouseButtonEventArgs evt)
        {
            if (!MatchUtils.IsEmpty(this.slide.Target) && !MatchUtils.IsEmpty(this.slide.Target.Netmap))
            {
                MouseButtonEventHandler stopHnd = null;
                {
                    this.SlideFullP2w.MouseLeftButtonUp += (stopHnd = new MouseButtonEventHandler((stopObj, stopEvt) =>
                    {
                        this.SlideFullP2w.MouseLeftButtonUp -= stopHnd;
                        {
                            stopHnd = null;
                        }
                        this.slide.Target.Netmap.Moveto(
                           this.slide.Target.Netmap.Crd2px(this.slide.Target.Center).Offset(-this.slide.Target.Screen.X, 0), true
                        );
                    }));
                }
            }
        }

        private void SlideFullP2e_MouseEnter(object obj, MouseEventArgs evt)
        {
            this.SlideFullPan.Background = this.FindResource("SLIDE_09") as ImageBrush;
        }

        private void SlideFullP2e_MouseLeave(object obj, MouseEventArgs evt)
        {
            this.SlideFullPan.Background = this.FindResource("SLIDE_13") as ImageBrush;
        }

        private void SlideFullP2e_MouseLeftButtonDown(object obj, MouseButtonEventArgs evt)
        {
            if (!MatchUtils.IsEmpty(this.slide.Target) && !MatchUtils.IsEmpty(this.slide.Target.Netmap))
            {
                MouseButtonEventHandler stopHnd = null;
                {
                    this.SlideFullP2e.MouseLeftButtonUp += (stopHnd = new MouseButtonEventHandler((stopObj, stopEvt) =>
                    {
                        this.SlideFullP2e.MouseLeftButtonUp -= stopHnd;
                        {
                            stopHnd = null;
                        }
                        this.slide.Target.Netmap.Moveto(
                           this.slide.Target.Netmap.Crd2px(this.slide.Target.Center).Offset(this.slide.Target.Screen.X, 0), true
                        );
                    }));
                }
            }
        }

        private void SlideFullP2s_MouseEnter(object obj, MouseEventArgs evt)
        {
            this.SlideFullPan.Background = this.FindResource("SLIDE_10") as ImageBrush;
        }

        private void SlideFullP2s_MouseLeave(object obj, MouseEventArgs evt)
        {
            this.SlideFullPan.Background = this.FindResource("SLIDE_13") as ImageBrush;
        }

        private void SlideFullP2s_MouseLeftButtonDown(object obj, MouseButtonEventArgs evt)
        {
            if (!MatchUtils.IsEmpty(this.slide.Target) && !MatchUtils.IsEmpty(this.slide.Target.Netmap))
            {
                MouseButtonEventHandler stopHnd = null;
                {
                    this.SlideFullP2s.MouseLeftButtonUp += (stopHnd = new MouseButtonEventHandler((stopObj, stopEvt) =>
                    {
                        this.SlideFullP2s.MouseLeftButtonUp -= stopHnd;
                        {
                            stopHnd = null;
                        }
                        this.slide.Target.Netmap.Moveto(
                           this.slide.Target.Netmap.Crd2px(this.slide.Target.Center).Offset(0, this.slide.Target.Screen.Y), true
                        );
                    }));
                }
            }
        }

        #endregion

        #region SlideFullZmi

        private void SlideFullZmi_MouseLeftButtonDown(object obj, MouseButtonEventArgs evt)
        {
            if (!MatchUtils.IsEmpty(this.slide.Target) && !MatchUtils.IsEmpty(this.slide.Target.Netmap))
            {
                MouseButtonEventHandler stopHnd = null;
                {
                    this.SlideFullZmi.MouseLeftButtonUp += (stopHnd = new MouseButtonEventHandler((stopObj, stopEvt) =>
                    {
                        this.SlideFullZmi.MouseLeftButtonUp -= stopHnd;
                        {
                            stopHnd = null;
                        }
                        this.slide.Target.Netmap.Zoomto(this.slide.Target.Netmap.Level + 1);
                    }));
                }
            }
        }

        #endregion

        #region SlideFullBar

        private void SlideFullBar_MouseLeftButtonDown(object obj, MouseButtonEventArgs evt)
        {
            if (!MatchUtils.IsEmpty(this.slide.Target) && !MatchUtils.IsEmpty(this.slide.Target.Netmap) && !MatchUtils.IsEmpty(this.slide.Target.Netmap.Factor))
            {
                MouseButtonEventHandler stopHnd = null;
                {
                    this.SlideFullBar.MouseLeftButtonUp += (stopHnd = new MouseButtonEventHandler((stopObj, stopEvt) =>
                    {
                        this.SlideFullBar.MouseLeftButtonUp -= stopHnd;
                        {
                            stopHnd = null;
                        }
                        this.slide.Target.Netmap.Zoomto(this.slide.Target.Netmap.Factor.Length - Convert.ToInt32(Math.Floor(
                            evt.GetPosition(this.SlideFullBar).Y / 7 + 1
                        )));
                    }));
                }
            }
        }

        #endregion

        #region SlideFullCtrl

        private void SlideFullCtrl_MouseLeftButtonDown(object obj, MouseButtonEventArgs evt)
        {
            if (evt.ClickCount == 1)
            {
                if (!MatchUtils.IsEmpty(this.slide.Target) && !MatchUtils.IsEmpty(this.slide.Target.Netmap) && !MatchUtils.IsEmpty(this.slide.Target.Netmap.Factor))
                {
                    // 捕获光标
                    if (this.SlideFullCtrl.CaptureMouse() && (evt.Handled = true))
                    {
                        int count = this.slide.Target.Netmap.Factor.Length, route = count * 7 - 4, level = -1;
                        {
                            Point mpoint = new Point();
                            MouseEventHandler mevent = null;
                            MouseButtonEventHandler sevent = null;
                            // 绑定事件
                            this.slide.Target.Facade.MouseMove += (mevent = new MouseEventHandler(delegate(object moveObj, MouseEventArgs moveEvt)
                            {
                                if (!MatchUtils.IsEmpty(mpoint = moveEvt.GetPosition(this.SlideFullBar)) && mpoint.Y < route && mpoint.Y > 0)
                                {
                                    level = Convert.ToInt32(Math.Floor(mpoint.Y / 7));
                                    // 滑块位置
                                    {
                                        Canvas.SetTop(this.SlideFullMask, level * 7 + 6);
                                        Canvas.SetTop(this.SlideFullCtrl, level * 7 + 3);
                                    }
                                }
                            }));
                            this.slide.Target.Facade.MouseLeftButtonUp += (sevent = new MouseButtonEventHandler(delegate(object stopObj, MouseButtonEventArgs stopEvt)
                            {
                                // 注销事件
                                try
                                {
                                    this.slide.Target.Facade.MouseMove -= mevent;
                                    this.slide.Target.Facade.MouseLeftButtonUp -= sevent;
                                }
                                catch
                                { }
                                finally
                                {
                                    // 释放光标
                                    this.SlideFullCtrl.ReleaseMouseCapture();
                                    {
                                        if (level > -1)
                                        {
                                            // 缩放地图
                                            this.slide.Target.Netmap.Zoomto(count - level - 1);
                                        }
                                    }
                                    // 释放内存
                                    mevent = null;
                                    sevent = null;
                                }
                            }));
                        }
                    }
                }
            }
        }

        #endregion

        #region SlideFullZmo

        private void SlideFullZmo_MouseLeftButtonDown(object obj, MouseButtonEventArgs evt)
        {
            if (!MatchUtils.IsEmpty(this.slide.Target) && !MatchUtils.IsEmpty(this.slide.Target.Netmap))
            {
                MouseButtonEventHandler stopHnd = null;
                {
                    this.SlideFullZmo.MouseLeftButtonUp += (stopHnd = new MouseButtonEventHandler((stopObj, stopEvt) =>
                    {
                        this.SlideFullZmo.MouseLeftButtonUp -= stopHnd;
                        {
                            stopHnd = null;
                        }
                        this.slide.Target.Netmap.Zoomto(this.slide.Target.Netmap.Level - 1);
                    }));
                }
            }
        }

        #endregion

        #endregion

        #region SlideGrid_Lite

        private void SlideLiteZmi_MouseLeftButtonDown(object obj, MouseButtonEventArgs evt)
        {
            if (!MatchUtils.IsEmpty(this.slide.Target) && !MatchUtils.IsEmpty(this.slide.Target.Netmap))
            {
                MouseButtonEventHandler stopHnd = null;
                {
                    this.SlideLiteZmi.MouseLeftButtonUp += (stopHnd = new MouseButtonEventHandler((stopObj, stopEvt) =>
                    {
                        this.SlideLiteZmi.MouseLeftButtonUp -= stopHnd;
                        {
                            stopHnd = null;
                        }
                        this.slide.Target.Netmap.Zoomto(this.slide.Target.Netmap.Level + 1);
                    }));
                }
            }
        }

        private void SlideLiteZmo_MouseLeftButtonDown(object obj, MouseButtonEventArgs evt)
        {
            if (!MatchUtils.IsEmpty(this.slide.Target) && !MatchUtils.IsEmpty(this.slide.Target.Netmap))
            {
                MouseButtonEventHandler stopHnd = null;
                {
                    this.SlideLiteZmo.MouseLeftButtonUp += (stopHnd = new MouseButtonEventHandler((stopObj, stopEvt) =>
                    {
                        this.SlideLiteZmo.MouseLeftButtonUp -= stopHnd;
                        {
                            stopHnd = null;
                        }
                        this.slide.Target.Netmap.Zoomto(this.slide.Target.Netmap.Level - 1);
                    }));
                }
            }
        }

        #endregion

        #endregion
    }
}