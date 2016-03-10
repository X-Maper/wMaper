using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WMagic;
using WMaper.Base;
using WMaper.Meta;
using WMaper.Meta.Param;

namespace WMaper.Core
{
    public sealed class Mark : Layer
    {
        #region 变量

        // 元素句柄
        private UserControl handle;
        // 是否可见
        private bool matte;
        // 是否修改
        private bool amend;
        // 元素标签
        private string label;
        // 显示范围
        private Arise arise;
        // 字体颜色
        private Color color;
        // 元素背景
        private Color scene;
        // 元素尺寸
        private Dimen dimen;
        // 元素锚点
        private Calib calib;
        // 元素位置
        private Coord point;
        // 显示光标
        private Cursor mouse;
        // 元素图源
        private ImageSource image;
        // 元素字体样式
        private WMaper.Meta.Fonts fonts;
        // 元素边框样式
        private WMaper.Meta.Frame frame;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Mark()
            : base()
        {
            this.handle = null;
            this.matte = false;
            this.amend = false;
            this.label = null;
            this.image = null;
            this.arise = null;
            this.fonts = null;
            this.frame = null;
            this.dimen = null;
            this.calib = null;
            this.point = null;
            this.color = Colors.Blue;
            this.scene = Colors.Transparent;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public Mark(Option option)
            : this()
        {
            if (!MatchUtils.IsEmpty(option))
            {
                // 用户配置
                if (option.Exist("Index"))
                    this.Index = option.Fetch<string>("Index");
                if (option.Exist("Group"))
                    this.Group = option.Fetch<string>("Group");
                if (option.Exist("Title"))
                    this.Title = option.Fetch<string>("Title");
                if (option.Exist("Mouse"))
                    this.Mouse = option.Fetch<Cursor>("Mouse");
                if (option.Exist("Matte"))
                    this.matte = option.Fetch<bool>("Matte");
                if (option.Exist("Amend"))
                    this.amend = option.Fetch<bool>("Amend");
                if (option.Exist("Alpha"))
                    this.Alpha = option.Fetch<int>("Alpha");
                if (option.Exist("Arise"))
                    this.Arise = option.Fetch<Arise>("Arise");
                if (option.Exist("Color"))
                    this.Color = option.Fetch<Color>("Color");
                if (option.Exist("Scene"))
                    this.Scene = option.Fetch<Color>("Scene");
                if (option.Exist("Dimen"))
                    this.Dimen = option.Fetch<Dimen>("Dimen");
                if (option.Exist("Calib"))
                    this.Calib = option.Fetch<Calib>("Calib");
                if (option.Exist("Point"))
                    this.Point = option.Fetch<Coord>("Point");
                if (option.Exist("Label"))
                    this.Label = option.Fetch<String>("Label");
                if (option.Exist("Image"))
                    this.Image = option.Fetch<ImageSource>("Image");
                if (option.Exist("Fonts"))
                    this.Fonts = option.Fetch<WMaper.Meta.Fonts>("Fonts");
                if (option.Exist("Frame"))
                    this.Frame = option.Fetch<WMaper.Meta.Frame>("Frame");
            }
        }

        #endregion

        #region 属性方法

        public UserControl Handle
        {
            get { return this.handle; }
        }

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

        public string Label
        {
            get { return this.label; }
            set { this.label = value; }
        }

        public Arise Arise
        {
            get { return this.arise; }
            set { this.arise = value; }
        }

        public Color Color
        {
            get { return this.color; }
            set { this.color = value; }
        }

        public Color Scene
        {
            get { return this.scene; }
            set { this.scene = value; }
        }

        public Dimen Dimen
        {
            get { return this.dimen; }
            set { this.dimen = value; }
        }

        public Calib Calib
        {
            get { return this.calib; }
            set { this.calib = value; }
        }

        public Coord Point
        {
            get { return this.point; }
            set { this.point = value; }
        }

        public Cursor Mouse
        {
            get { return this.mouse; }
            set { this.mouse = value; }
        }

        public ImageSource Image
        {
            get { return this.image; }
            set
            {
                if (!MatchUtils.IsEmpty(this.image = value))
                {
                    // 冻结图像
                    if (this.image.CanFreeze)
                    {
                        this.image.Freeze();
                    }
                }
            }
        }

        public WMaper.Meta.Fonts Fonts
        {
            get { return this.fonts; }
            set { this.fonts = value; }
        }

        public WMaper.Meta.Frame Frame
        {
            get { return this.frame; }
            set { this.frame = value; }
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 地标置前
        /// </summary>
        public void Summit()
        {
            if (!MatchUtils.IsEmpty(this.Target) && !MatchUtils.IsEmpty(this.Facade) && this.Target.Enable && this.Enable)
            {
                this.Target.Vessel.Mark.Children.Add(this.handle);
            }
        }

        /// <summary>
        /// 地标渲染函数
        /// </summary>
        /// <param name="drv"></param>
        public sealed override void Render(Maper drv)
        {
            if (!MatchUtils.IsEmpty(this.Target = drv) && this.Target.Enable)
            {
                this.Index = MatchUtils.IsEmpty(this.Index) ? (this.Index = StampUtils.GetTimeStamp()) : this.Index;
                if (drv.Symbol.Mark.ContainsKey(this.Index))
                {
                    Mark mark = drv.Symbol.Mark[this.Index];
                    if (this.Equals(mark))
                    {
                        this.Redraw();
                    }
                    else
                    {
                        {
                            mark.Remove();
                        }
                        this.Render(drv);
                    }
                }
                else
                {
                    // 监听广播
                    this.Observe(drv.Listen.DragEvent, this.Redraw);
                    this.Observe(drv.Listen.ZoomEvent, this.Redraw);
                    this.Observe(drv.Listen.SwapEvent, this.Redraw);
                    // 绘制图形
                    drv.Symbol.Mark.Add(this.Index, this);
                    try
                    {
                        this.Redraw();
                    }
                    catch
                    {
                        drv.Symbol.Mark.Remove(this.Index);
                    }
                    finally
                    {
                        if (!drv.Symbol.Mark.ContainsKey(this.Index))
                        {
                            // 移除监听
                            this.Obscure(drv.Listen.DragEvent, this.Redraw);
                            this.Obscure(drv.Listen.ZoomEvent, this.Redraw);
                            this.Obscure(drv.Listen.SwapEvent, this.Redraw);
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

        /// <summary>
        /// 绘制地标
        /// </summary>
        /// <param name="msg"></param>
        public sealed override void Redraw(WMaper.Meta.Radio.Msger msg)
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && !MatchUtils.IsEmpty(this.Target.Netmap) && this.Enable)
            {
                if (MatchUtils.IsEmpty(msg))
                {
                    this.Redraw();
                }
                else
                {
                    if (!MatchUtils.IsEmpty(this.Facade) && !MatchUtils.IsEmpty(this.handle))
                    {
                        if (this.Target.Listen.DragEvent.Equals(msg.Chan) || this.Target.Listen.ZoomEvent.Equals(msg.Chan) || (
                            this.Target.Listen.SwapEvent.Equals(msg.Chan) && !(msg.Info as Geog).Cover
                        ))
                        {
                            this.handle.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (!MatchUtils.IsEmpty(this.handle))
                                {
                                    this.handle.Visibility = (
                                        this.Matte || !this.Viewble(this.Arise, this.Point) ? Visibility.Collapsed : Visibility.Visible
                                    );
                                    {
                                        (this.handle as WMaper.Misc.View.Core.Mark).Adjust();
                                    }
                                }
                            }));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 重绘地标
        /// </summary>
        public sealed override void Redraw()
        {
            if (!MatchUtils.IsEmpty(this.Target) && !MatchUtils.IsEmpty(this.Target.Netmap) && this.Target.Enable && this.Enable)
            {
                if (MatchUtils.IsEmpty(this.handle))
                {
                    this.Target.Vessel.Mark.Children.Add(
                        this.handle = new Misc.View.Core.Mark(this)
                    );
                }
                // 绘制图形
                this.handle.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (!MatchUtils.IsEmpty(this.handle))
                    {
                        this.handle.Visibility = (
                            this.Matte || !this.Viewble(this.Arise, this.Point) ? Visibility.Collapsed : Visibility.Visible
                        );
                        {
                            (this.handle as WMaper.Misc.View.Core.Mark).Build();
                        }
                    }
                }));
            }
        }

        /// <summary>
        /// 移除地标
        /// </summary>
        public sealed override void Remove()
        {
            if (!MatchUtils.IsEmpty(this.Target) && this.Target.Enable && this.Enable)
            {
                if (this.Obscure(this.Target.Listen.DragEvent, this.Redraw))
                {
                    if (!this.Obscure(this.Target.Listen.ZoomEvent, this.Redraw))
                    {
                        this.Observe(this.Target.Listen.DragEvent, this.Redraw);
                    }
                    else
                    {
                        if (!this.Obscure(this.Target.Listen.SwapEvent, this.Redraw))
                        {
                            this.Observe(this.Target.Listen.DragEvent, this.Redraw);
                            this.Observe(this.Target.Listen.ZoomEvent, this.Redraw);
                        }
                        else
                        {
                            if (!this.Target.Symbol.Mark.Remove(this.Index))
                            {
                                this.Observe(this.Target.Listen.DragEvent, this.Redraw);
                                this.Observe(this.Target.Listen.ZoomEvent, this.Redraw);
                                this.Observe(this.Target.Listen.SwapEvent, this.Redraw);
                            }
                            else
                            {
                                this.Target.Vessel.Mark.Children.Remove(this.handle);
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
        }

        #endregion
    }
}