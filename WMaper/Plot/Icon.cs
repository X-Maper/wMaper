using System;
using System.Windows.Input;
using System.Windows.Media;
using WMagic;
using WMagic.Brush.Basic;
using WMagic.Brush.Shape;
using WMaper.Base;
using WMaper.Core;
using WMaper.Meta;
using WMaper.Meta.Param;

namespace WMaper.Plot
{
    /// <summary>
    /// 图标层
    /// </summary>
    public sealed class Icon : Geom
    {
        #region 变量

        // 位置
        private Coord point;
        // 斜率
        private double slope;
        // 边框
        private GFrame frame;
        // 尺寸
        private GDimen dimen;
        // 锚点
        private GCalib calib;
        // 图源
        private ImageSource image;

        #endregion

        #region 属性方法

        public Coord Point
        {
            get { return this.point; }
            set { this.point = value; }
        }

        public double Slope
        {
            get { return this.slope; }
            set { this.slope = value; }
        }

        public GFrame Frame
        {
            get { return this.frame; }
            set { this.frame = value; }
        }

        public GDimen Dimen
        {
            get { return this.dimen; }
            set { this.dimen = value; }
        }

        public GCalib Calib
        {
            get { return this.calib; }
            set { this.calib = value; }
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

        #endregion

        #region 构造函数

        public Icon()
            : base()
        {
            this.slope = 0;
            this.image = null;
            this.frame = null;
            this.dimen = null;
            this.calib = null;
            this.point = null;
        }

        public Icon(Option option)
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
                    this.Matte = option.Fetch<bool>("Matte");
                if (option.Exist("Amend"))
                    this.Amend = option.Fetch<bool>("Amend");
                if (option.Exist("Alpha"))
                    this.Alpha = option.Fetch<int>("Alpha");
                if (option.Exist("Slope"))
                    this.Slope = option.Fetch<double>("Slope");
                if (option.Exist("Arise"))
                    this.Arise = option.Fetch<Arise>("Arise");
                if (option.Exist("Point"))
                    this.Point = option.Fetch<Coord>("Point");
                if (option.Exist("Frame"))
                    this.Frame = option.Fetch<GFrame>("Frame");
                if (option.Exist("Dimen"))
                    this.Dimen = option.Fetch<GDimen>("Dimen");
                if (option.Exist("Calib"))
                    this.Calib = option.Fetch<GCalib>("Calib");
                if (option.Exist("Image"))
                    this.Image = option.Fetch<ImageSource>("Image");
            }
        }

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
                            GIcon v_icon = this.Handle as GIcon;
                            {
                                v_icon.Mouse = this.Mouse;
                                v_icon.Title = this.Title;
                                v_icon.Alpha = this.Alpha;
                                v_icon.Slope = this.slope;
                                v_icon.Image = this.image;
                                v_icon.Frame = this.frame;
                                v_icon.Dimen = this.dimen;
                                v_icon.Calib = this.calib;
                                v_icon.Point = this.Fit4p(this.point);
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
                        if (!((this.Handle = this.Target.Sketch.CreateIcon()).Matte = hide))
                        {
                            GIcon v_icon = this.Handle as GIcon;
                            {
                                v_icon.Matte = hide;
                                v_icon.Mouse = this.Mouse;
                                v_icon.Title = this.Title;
                                v_icon.Alpha = this.Alpha;
                                v_icon.Slope = this.slope;
                                v_icon.Image = this.image;
                                v_icon.Frame = this.frame;
                                v_icon.Dimen = this.dimen;
                                v_icon.Calib = this.calib;
                                v_icon.Point = this.Fit4p(this.point);
                            }
                            // 绘制图形
                            this.Facade.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (!MatchUtils.IsEmpty(this.Handle))
                                {
                                    v_icon.Paint();
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