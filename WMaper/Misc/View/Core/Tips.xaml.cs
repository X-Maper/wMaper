using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WMagic;
using WMaper.Base;

namespace WMaper.Misc.View.Core
{
    public sealed partial class Tips : UserControl
    {
        #region 变量

        private bool make;

        private WMaper.Core.Tips tips;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Tips()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tips"></param>
        public Tips(WMaper.Core.Tips tips)
            : this()
        {
            this.make = false;
            {
                (this.tips = tips).Facade = this.TipsLayer;
            }
            this.TipsGrid.LayoutUpdated += this.Paint;
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
                        Pixel pixel = this.tips.Target.Netmap.Crd2px(this.tips.Point);
                        try
                        {
                            pixel = this.tips.Target.Netmap.Crd2px(this.tips.Point);
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
                                double devX = !MatchUtils.IsEmpty(this.tips.Drift) ? this.tips.Drift.X : 0, devY = !MatchUtils.IsEmpty(this.tips.Drift) ? this.tips.Drift.Y : 0;
                                {
                                    Canvas.SetTop(this, Math.Round(devY + pixel.Y - this.tips.Target.Netmap.Nature.Y - this.TipsGrid.ActualHeight));
                                    Canvas.SetLeft(this, Math.Round(devX + pixel.X - this.tips.Target.Netmap.Nature.X - this.TipsGrid.ActualWidth * 0.5));
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 构建提示框
        /// </summary>
        public void Build()
        {
            if (!MatchUtils.IsEmpty(this.tips) && !MatchUtils.IsEmpty(this.tips.Target) && !MatchUtils.IsEmpty(this.tips.Target.Netmap) && this.tips.Target.Enable && this.tips.Enable)
            {
                // 提示光标
                this.Cursor = this.tips.Mouse;
                {
                    // 提示标题
                    this.TipsTitle.Content = this.tips.Title;
                    // 提示透明
                    this.tips.Facade.Opacity = this.tips.Alpha / 100.0;
                    // 提示色彩
                    this.TipsTitle.Foreground = new SolidColorBrush(this.tips.Color);
                    // 提示字体
                    if (MatchUtils.IsEmpty(this.tips.Fonts))
                    {
                        this.TipsTitle.FontSize = 14;
                        this.TipsTitle.FontStyle = FontStyles.Normal;
                        this.TipsTitle.FontWeight = FontWeights.Bold;
                    }
                    else
                    {
                        this.TipsTitle.FontSize = this.tips.Fonts.Size;
                        this.TipsTitle.FontStyle = this.tips.Fonts.Ital ? FontStyles.Italic : FontStyles.Normal;
                        this.TipsTitle.FontWeight = this.tips.Fonts.Bold ? FontWeights.Bold : FontWeights.Normal;
                    }
                    // 提示内容
                    this.TipsBody.Children.Clear();
                    {
                        FrameworkElement component = new FrameworkElement();
                        if (!MatchUtils.IsEmpty(this.tips.Quote))
                        {
                            if (this.tips.Quote is String)
                            {
                                string ctx = this.tips.Quote as String;
                                if (MatchUtils.IsUrl(ctx))
                                {
                                    // 添加浏览器控件
                                    WebBrowser webBrowser = new WebBrowser();
                                    try
                                    {
                                        webBrowser.Source = new Uri(ctx, UriKind.RelativeOrAbsolute);
                                    }
                                    catch
                                    {
                                        webBrowser = null;
                                    }
                                    finally
                                    {
                                        if (webBrowser != null)
                                        {
                                            component = webBrowser;
                                        }
                                    }
                                }
                                else
                                {
                                    // 添加文本内容
                                    TextBox textBox = new TextBox();
                                    try
                                    {
                                        textBox.Text = ctx;
                                        textBox.FontSize = 12;
                                        textBox.IsReadOnly = true;
                                        textBox.IsHitTestVisible = false;
                                        textBox.IsReadOnlyCaretVisible = false;
                                        textBox.FontStyle = FontStyles.Normal;
                                        textBox.FontWeight = FontWeights.Normal;
                                        textBox.TextWrapping = TextWrapping.Wrap;
                                        textBox.FontFamily = new FontFamily("SimSun");
                                        textBox.BorderThickness = (
                                            textBox.Padding = new Thickness(0, 0, 0, 0)
                                        );
                                        textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                                        textBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                                    }
                                    catch
                                    {
                                        textBox = null;
                                    }
                                    finally
                                    {
                                        if (textBox != null)
                                        {
                                            component = textBox;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (this.tips.Quote is FrameworkElement)
                                {
                                    // 初始控件绑定
                                    DependencyObject depend = (this.tips.Quote as FrameworkElement).Parent;
                                    if (!MatchUtils.IsEmpty(depend))
                                    {
                                        depend.SetValue(ContentProperty, null);
                                        {
                                            depend = null;
                                        }
                                    }
                                    // 添加控件内容
                                    ScrollViewer scrollViewer = new ScrollViewer();
                                    {
                                        scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                                        scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                                    }
                                    try
                                    {
                                        scrollViewer.Content = this.tips.Quote as FrameworkElement;
                                    }
                                    catch
                                    {
                                        scrollViewer = null;
                                    }
                                    finally
                                    {
                                        if (scrollViewer != null)
                                        {
                                            component = scrollViewer;
                                        }
                                    }
                                }
                            }
                        }
                        // 设置尺寸
                        {
                            if (MatchUtils.IsEmpty(this.tips.Gauge))
                            {
                                component.Width = 135.0;
                                component.Height = 35.0;
                            }
                            else
                            {
                                component.Width = this.tips.Gauge.W;
                                component.Height = this.tips.Gauge.H;
                            }
                            component.Margin = new Thickness(8, 5, 8, 5);
                        }
                        this.TipsBody.Children.Add(component);
                    }
                }
                // 强制绘制
                this.Adjust();
            }
        }

        /// <summary>
        /// 纠偏提示
        /// </summary>
        public void Adjust()
        {
            if (Visibility.Visible.Equals(this.Visibility))
            {
                this.make = true;
                {
                    this.TipsGrid.InvalidateArrange();
                }
            }
        }

        /// <summary>
        /// 关闭提示框
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="evt"></param>
        private void TipsCtrl_MouseLeftButtonDown(object obj, MouseButtonEventArgs evt)
        {
            if (!MatchUtils.IsEmpty(this.tips) && !MatchUtils.IsEmpty(this.tips.Target) && this.tips.Target.Enable && (evt.Handled = this.tips.Enable) && evt.ClickCount == 1)
            {
                MouseButtonEventHandler stopHnd = null;
                {
                    this.TipsCtrl.MouseLeftButtonUp += (stopHnd = new MouseButtonEventHandler((stopObj, stopEvt) =>
                    {
                        this.TipsCtrl.MouseLeftButtonUp -= stopHnd;
                        {
                            stopHnd = null;
                        }
                        this.tips.Remove();
                    }));
                }
            }
        }

        #endregion
    }
}