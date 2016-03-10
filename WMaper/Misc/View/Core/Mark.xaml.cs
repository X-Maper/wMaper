using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WMagic;
using WMaper.Base;

namespace WMaper.Misc.View.Core
{
    public sealed partial class Mark : UserControl
    {
        #region 变量

        private bool make;

        private WMaper.Core.Mark mark;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Mark()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mark"></param>
        public Mark(WMaper.Core.Mark mark)
            : this()
        {
            this.make = false;
            {
                (this.mark = mark).Facade = this.MarkLayer;
            }
            this.MarkGrid.LayoutUpdated += this.Paint;
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 强制绘制
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="evt"></param>
        private void Paint(object obj, EventArgs evt)
        {
            if (this.make)
            {
                this.make = false;
                {
                    if (Visibility.Visible.Equals(this.Visibility))
                    {
                        Pixel pixel = null;
                        try
                        {
                            pixel = this.mark.Target.Netmap.Crd2px(this.mark.Point);
                        }
                        catch (NullReferenceException)
                        {
                            pixel = null;
                        }
                        finally
                        {
                            if (MatchUtils.IsEmpty(pixel))
                            {
                                this.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                double devX = !MatchUtils.IsEmpty(this.mark.Calib) ? this.mark.Calib.X : 0, devY = !MatchUtils.IsEmpty(this.mark.Calib) ? this.mark.Calib.Y : 0, wide = (this.MarkGrid.ActualWidth - this.MarkImage.ActualWidth) * 0.5, high = this.MarkGrid.ActualHeight - this.MarkImage.ActualHeight - (
                                    !MatchUtils.IsEmpty(this.mark.Frame) ? this.mark.Frame.Thick : 0
                                );
                                // 调整位置
                                {
                                    Canvas.SetTop(this, Math.Round(pixel.Y - this.mark.Target.Netmap.Nature.Y - devY - high));
                                    Canvas.SetLeft(this, Math.Round(pixel.X - this.mark.Target.Netmap.Nature.X - devX - wide));
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 构建地标
        /// </summary>
        public void Build()
        {
            if (!MatchUtils.IsEmpty(this.mark) && !MatchUtils.IsEmpty(this.mark.Target) && !MatchUtils.IsEmpty(this.mark.Target.Netmap) && this.mark.Target.Enable && this.mark.Enable)
            {
                // 地标光标
                this.Cursor = this.mark.Mouse;
                {
                    // 地标图片
                    this.MarkImage.Source = this.mark.Image;
                    // 地标标题
                    this.MarkLabel.ToolTip = this.mark.Title;
                    this.MarkImage.ToolTip = this.mark.Title;
                    this.TextFrame.ToolTip = this.mark.Title;
                    this.IconFrame.ToolTip = this.mark.Title;
                    // 地标文字
                    this.MarkLabel.Content = this.mark.Label;
                    // 地标透明
                    this.mark.Facade.Opacity = this.mark.Alpha / 100.0;
                    // 地标色彩
                    this.MarkLabel.Foreground = new SolidColorBrush(this.mark.Color);
                    this.MarkLabel.Background = new SolidColorBrush(this.mark.Scene);
                    // 地标字体
                    if (MatchUtils.IsEmpty(this.mark.Fonts))
                    {
                        this.MarkLabel.FontSize = 12;
                        this.MarkLabel.FontStyle = FontStyles.Normal;
                        this.MarkLabel.FontWeight = FontWeights.Normal;
                    }
                    else
                    {
                        this.MarkLabel.FontSize = this.mark.Fonts.Size;
                        this.MarkLabel.FontStyle = this.mark.Fonts.Ital ? FontStyles.Italic : FontStyles.Normal;
                        this.MarkLabel.FontWeight = this.mark.Fonts.Bold ? FontWeights.Bold : FontWeights.Normal;
                    }
                    // 地标尺寸
                    if (MatchUtils.IsEmpty(this.mark.Dimen))
                    {
                        this.MarkImage.Width = 0;
                        this.MarkImage.Height = 0;
                    }
                    else
                    {
                        this.MarkImage.Width = this.mark.Dimen.W;
                        this.MarkImage.Height = this.mark.Dimen.H;
                    }
                    // 地标边框
                    if (!MatchUtils.IsEmpty(this.mark.Frame))
                    {
                        switch (this.mark.Frame.Style)
                        {
                            case WMaper.Meta.Frame.Linear.DOT:
                                {
                                    this.IconFrame.StrokeDashArray = (
                                        this.TextFrame.StrokeDashArray = DoubleCollection.Parse("1,2")
                                    );
                                    break;
                                }
                            case WMaper.Meta.Frame.Linear.DASH:
                                {
                                    this.IconFrame.StrokeDashArray = (
                                        this.TextFrame.StrokeDashArray = DoubleCollection.Parse("2,2")
                                    );
                                    break;
                                }
                            case WMaper.Meta.Frame.Linear.DASHDOT:
                                {
                                    this.IconFrame.StrokeDashArray = (
                                        this.TextFrame.StrokeDashArray = DoubleCollection.Parse("2,2,1,2")
                                    );
                                    break;
                                }
                            default:
                                {
                                    this.IconFrame.StrokeDashArray = (
                                        this.TextFrame.StrokeDashArray = null
                                    );
                                    break;
                                }
                        }
                        this.MarkImage.Margin = (
                            this.MarkLabel.Padding = new Thickness(
                                this.IconFrame.StrokeThickness = (
                                    this.TextFrame.StrokeThickness = this.mark.Frame.Thick
                                )
                            )
                        );
                        this.IconFrame.Stroke = (
                            this.TextFrame.Stroke = new SolidColorBrush(this.mark.Frame.Color)
                        );
                        // 合并边框
                        {
                            (this.MarkImage.Parent as Grid).Margin = new Thickness(
                                0, 0 - this.mark.Frame.Thick, 0, 0
                            );
                        }
                    }
                    else
                    {
                        // 取消边框
                        this.MarkLabel.Padding = new Thickness(1);
                        {
                            (this.MarkImage.Parent as Grid).Margin = (
                                this.MarkImage.Margin = new Thickness(
                                    this.IconFrame.StrokeThickness = (
                                        this.TextFrame.StrokeThickness = 0
                                    )
                                )
                            );
                        }
                    }
                }
                // 强制绘制
                this.Adjust();
            }
        }

        /// <summary>
        /// 纠偏地标
        /// </summary>
        public void Adjust()
        {
            if (Visibility.Visible.Equals(this.Visibility))
            {
                this.make = true;
                {
                    this.MarkGrid.InvalidateArrange();
                }
            }
        }

        #endregion
    }
}