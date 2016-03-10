using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using WMagic;

namespace WMaper.Misc.View.Plug
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public sealed partial class About : UserControl
    {
        #region 常量

        private const string TEXTBLOCK_TEMPLATE = @"<TextBlock xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{0}</TextBlock>";

        #endregion

        #region 变量

        private bool ready;
        // 控件对象
        private WMaper.Plug.About about;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public About()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public About(int sort)
            : this()
        {
            Panel.SetZIndex(this, sort);
        }

        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="about"></param>
        public About(WMaper.Plug.About about, int sort)
            : this(sort)
        {
            this.ready = false;
            {
                (this.about = about).Facade = this.AboutDecor;
                {
                    // 语言资源
                    this.AboutResource.MergedDictionaries.Add(
                        this.about.Target.Assets.Language
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
                if ("left".Equals(this.about.Anchor))
                {
                    Canvas.SetLeft(this, 18);
                    {
                        this.AboutGrid.SizeChanged += new SizeChangedEventHandler((obj, evt) =>
                        {
                            Canvas.SetBottom(this, this.AboutGrid.ActualHeight);
                        });
                    }
                }
                else
                {
                    Canvas.SetRight(this, 18);
                    {
                        this.AboutGrid.SizeChanged += new SizeChangedEventHandler((obj, evt) =>
                        {
                            Canvas.SetBottom(this, this.AboutGrid.ActualHeight);
                            {
                                Canvas.SetRight(this, this.AboutGrid.ActualWidth + 18);
                            }
                        });
                    }
                }
                // 显示版权文本
                if (!MatchUtils.IsEmpty(this.about.Author))
                {
                    TextBlock author = null;
                    try
                    {
                        author = (TextBlock)XamlReader.Parse(
                            String.Format(TEXTBLOCK_TEMPLATE, this.about.Author)
                        );
                    }
                    catch
                    {
                        author = null;
                    }
                    finally
                    {
                        if (!MatchUtils.IsEmpty(author))
                        {
                            author.FontSize = 12;
                            author.FontStyle = FontStyles.Normal;
                            author.FontWeight = FontWeights.Normal;
                            author.Background = Brushes.Transparent;
                            author.FontFamily = new FontFamily("SimSun");
                            author.Foreground = new SolidColorBrush(
                                Color.FromRgb((byte)103, (byte)104, (byte)125)
                            );
                            author.TextWrapping = TextWrapping.NoWrap;
                            // 绑定事件
                            foreach (Inline link in author.Inlines)
                            {
                                if (link is Hyperlink)
                                {
                                    (link as Hyperlink).Click += (obj, evt) =>
                                    {
                                        // 浏览链接
                                        Process.Start((obj as Hyperlink).NavigateUri + "");
                                    };
                                }
                            }
                            // 加载控件
                            this.AboutGrid.Children.Add(author);
                        }
                    }
                }
            }
        }

        #endregion
    }
}