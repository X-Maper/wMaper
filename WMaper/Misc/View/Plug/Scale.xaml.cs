using System;
using System.Windows;
using System.Windows.Controls;
using WMagic;

namespace WMaper.Misc.View.Plug
{
    /// <summary>
    /// Scale.xaml 的交互逻辑
    /// </summary>
    public sealed partial class Scale : UserControl
    {
        #region 变量

        private bool ready;
        // 控件对象
        private WMaper.Plug.Scale scale;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Scale()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Scale(int sort)
            : this()
        {
            Panel.SetZIndex(this, sort);
        }

        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="scale"></param>
        public Scale(WMaper.Plug.Scale scale, int sort)
            : this(sort)
        {
            this.ready = false;
            {
                (this.scale = scale).Facade = this.ScaleDecor;
                {
                    // 语言资源
                    this.ScaleResource.MergedDictionaries.Add(
                        this.scale.Target.Assets.Language
                    );
                }
            }
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 构建控件
        /// </summary>
        public void Build()
        {
            if (!this.ready && (this.ready = true))
            {
                // 适配位置
                if ("left".Equals(this.scale.Anchor))
                {
                    Canvas.SetLeft(this, 18);
                    {
                        this.ScaleGrid.SizeChanged += new SizeChangedEventHandler((obj, evt) =>
                        {
                            Canvas.SetBottom(this, this.ScaleGrid.ActualHeight + 18);
                        });
                    }
                }
                else
                {
                    this.ScaleGrid.SizeChanged += new SizeChangedEventHandler((obj, evt) =>
                    {
                        Canvas.SetRight(this, this.ScaleGrid.ActualWidth + 18);
                        Canvas.SetBottom(this, this.ScaleGrid.ActualHeight + 18);
                    });
                }
            }
            // 显示比例
            if (!MatchUtils.IsEmpty(this.scale.Target) && !MatchUtils.IsEmpty(this.scale.Target.Netmap))
            {
                double msc = 2.286 * this.scale.Target.Netmap.Deg2sc() / this.scale.Target.Netmap.Craft, exp = Math.Pow(10, Math.Floor(Math.Log10(msc)));
                {
                    // Scale Width.
                    this.ScaleGrid.Width = (
                        msc = Math.Round(msc / exp) * exp
                    ) * WMaper.Units.M * this.scale.Target.Netmap.Craft / this.scale.Target.Netmap.Deg2sc() + 6;
                    // Scale Label.
                    this.ScaleText.Content = (
                        msc < 1000 ? msc + (this.FindResource("SCALE_M") as String) : msc / 1000 + (this.FindResource("SCALE_KM") as String)
                    );
                }
            }
        }

        #endregion
    }
}