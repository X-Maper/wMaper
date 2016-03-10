using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WMagic;
using WMaper.Core;

namespace WMaper.Misc.View.Plug
{
    /// <summary>
    /// Genre.xaml 的交互逻辑
    /// </summary>
    public sealed partial class Genre : UserControl
    {
        #region 私类

        /// <summary>
        /// Genre选项类
        /// </summary>
        private class GenreOption
        {
            #region 变量

            // 索引标识
            private string index;
            // 显示标题
            private string title;
            // 显示圆角
            private string angle;
            // 显示颜色
            private string color;
            // 显示图片
            private ImageSource image;

            #endregion

            #region 构造函数

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="index">索引标识</param>
            /// <param name="title">显示标题</param>
            /// <param name="color">显示颜色</param>
            /// <param name="angle">显示圆角</param>
            /// <param name="image">显示图片</param>
            public GenreOption(string index, string title, string color, string angle, ImageSource image)
            {
                this.index = index;
                this.title = title;
                this.color = color;
                this.angle = angle;
                this.image = image;
            }

            #endregion

            #region 属性方法

            public string Index
            {
                get { return this.index; }
            }

            public string Title
            {
                get { return this.title; }
            }

            public string Angle
            {
                get { return this.angle; }
            }

            public string Color
            {
                get { return this.color; }
            }

            public ImageSource Image
            {
                get { return this.image; }
            }

            #endregion
        }

        #endregion

        #region 变量

        private bool ready;
        // 控件对象
        private WMaper.Plug.Genre genre;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Genre()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Genre(int sort)
            : this()
        {
            Panel.SetZIndex(this, sort);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Genre(WMaper.Plug.Genre genre, int sort)
            : this(sort)
        {
            this.ready = false;
            {
                (this.genre = genre).Facade = this.GenreDecor;
                {
                    // 语言资源
                    this.GenreResource.MergedDictionaries.Add(
                        this.genre.Target.Assets.Language
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
                // 设置位置
                if ("left".Equals(this.genre.Anchor))
                {
                    // 设置样式
                    this.GenreBox.Style = this.FindResource("GENRE_LIST_LEFT") as Style;
                    {
                        Canvas.SetTop(this, 18);
                        Canvas.SetLeft(this, 18);
                        // 适配位置
                        Grid.SetColumn(this.Genre1px, 0);
                        Grid.SetColumn(this.GenreBox, 1);
                        Grid.SetColumn(this.GenreBtnGrid, 0);
                        Grid.SetColumn(this.GenreBoxGrid, 1);
                    }
                }
                else
                {
                    // 设置样式
                    this.GenreBox.Style = this.FindResource("GENRE_LIST_RIGHT") as Style;
                    {
                        Canvas.SetTop(this, 18);
                        Canvas.SetRight(this, 53);
                        // 适配位置
                        Grid.SetColumn(this.Genre1px, 1);
                        Grid.SetColumn(this.GenreBox, 0);
                        Grid.SetColumn(this.GenreBtnGrid, 1);
                        Grid.SetColumn(this.GenreBoxGrid, 0);
                    }
                    this.GenreGrid.SizeChanged += new SizeChangedEventHandler((obj, evt) =>
                    {
                        Canvas.SetRight(this, this.GenreGrid.ActualWidth + 18);
                    });
                }
            }
            // 重绘控件
            if (Visibility.Visible.Equals(this.GenreBoxGrid.Visibility))
            {
                List<GenreOption> genreList = new List<GenreOption>();
                {
                    switch ((string)this.GenreBoxGrid.Tag)
                    {
                        case "MAP":
                            {
                                this.GenreBoxGrid.Margin = new Thickness(0, 0, 0, 0);
                                {
                                    Geog[] tiles = this.genre.Target.Symbol.Tile.Values.ToArray<Geog>();
                                    for (int i = 1, l = tiles.Length; i <= l; i++)
                                    {
                                        Geog tile = tiles[i - 1];
                                        {
                                            if (tile.Allow)
                                            {
                                                string radius = (
                                                    "left".Equals(this.genre.Anchor) ? (
                                                        l == 1 ? "0,5,5,0" : i == 1 ? "0,5,0,0" : i == l ? "0,0,5,5" : "0,0,0,0"
                                                    ) : (
                                                        l == 1 ? "5,0,0,5" : i == 1 ? "5,0,0,0" : i == l ? "0,0,5,5" : "0,0,0,0"
                                                    )
                                                );
                                                // 添加数据
                                                if (tile.Equals(this.genre.Target.Netmap))
                                                {
                                                    genreList.Add(
                                                        new GenreOption(tile.Index, tile.Title, "#0000FF", radius, this.FindResource("GENRE_07") as ImageSource)
                                                    );
                                                }
                                                else
                                                {
                                                    genreList.Add(
                                                        new GenreOption(tile.Index, tile.Title, "#000000", radius, this.FindResource("GENRE_08") as ImageSource)
                                                    );
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        case "LAP":
                            {
                                this.GenreBoxGrid.Margin = new Thickness(0, 45, 0, 0);
                                {
                                    Geog[] piles = this.genre.Target.Symbol.Pile.Values.ToArray<Geog>();
                                    for (int i = 1, l = piles.Length; i <= l; i++)
                                    {
                                        Geog pile = piles[i - 1];
                                        {
                                            string radius = (
                                                "left".Equals(this.genre.Anchor) ? (
                                                    l == 1 ? "0,5,5,0" : i == 1 ? "0,5,0,0" : i == l ? "0,0,5,5" : "0,0,0,0"
                                                ) : (
                                                    l == 1 ? "5,0,0,5" : i == 1 ? "5,0,0,0" : i == l ? "0,0,5,5" : "0,0,0,0"
                                                )
                                            );
                                            // 添加数据
                                            if (pile.Fusion(this.genre.Target) > -1 && pile.Extent.Contain(this.genre.Target, this.genre.Target.Center))
                                            {
                                                if (pile.Allow)
                                                {
                                                    genreList.Add(
                                                        new GenreOption(pile.Index, pile.Title, "#0000FF", radius, this.FindResource("GENRE_09") as ImageSource)
                                                    );
                                                }
                                                else
                                                {
                                                    genreList.Add(
                                                        new GenreOption(pile.Index, pile.Title, "#000000", radius, this.FindResource("GENRE_10") as ImageSource)
                                                    );
                                                }
                                            }
                                            else
                                            {
                                                genreList.Add(
                                                    new GenreOption(pile.Index, pile.Title, "#000000", radius, this.FindResource("GENRE_11") as ImageSource)
                                                );
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                    }
                    // 绑定数据
                    this.GenreBox.ItemsSource = genreList;
                }
            }
        }

        /// <summary>
        /// GenreMap离开事件
        /// GenreLap离开事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="evt"></param>
        private void GenreBtn_MouseLeave(object obj, MouseEventArgs evt)
        {
            this.GenreBoxGrid.Visibility = Visibility.Collapsed;
            {
                this.GenreMap.Source = this.FindResource("GENRE_01") as ImageSource;
                this.GenreLap.Source = this.FindResource("GENRE_04") as ImageSource;
            }
        }

        /// <summary>
        /// GenreMap进入事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="evt"></param>
        private void GenreMap_MouseEnter(object obj, MouseEventArgs evt)
        {
            if (!MatchUtils.IsEmpty(this.genre.Target) && this.genre.Target.Enable && this.genre.Enable)
            {
                if (!MatchUtils.IsEmpty(this.genre.Target.Symbol) && !MatchUtils.IsEmpty(this.genre.Target.Symbol.Tile) && this.genre.Target.Symbol.Tile.Count > 0)
                {
                    // 显示样式
                    this.GenreMap.Source = this.FindResource("left".Equals(this.genre.Anchor) ? "GENRE_03" : "GENRE_02") as ImageSource;
                    {
                        this.GenreBoxGrid.Tag = "MAP";
                    }
                    // 显示列表
                    this.GenreBoxGrid.Visibility = Visibility.Visible;
                    {
                        this.Build();
                    }
                }
            }
        }

        /// <summary>
        /// GenreLap进入事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="evt"></param>
        private void GenreLap_MouseEnter(object obj, MouseEventArgs evt)
        {
            if (!MatchUtils.IsEmpty(this.genre.Target) && this.genre.Target.Enable && this.genre.Enable)
            {
                if (!MatchUtils.IsEmpty(this.genre.Target.Symbol) && !MatchUtils.IsEmpty(this.genre.Target.Symbol.Pile) && this.genre.Target.Symbol.Pile.Count > 0)
                {
                    // 显示样式
                    this.GenreLap.Source = this.FindResource("left".Equals(this.genre.Anchor) ? "GENRE_06" : "GENRE_05") as ImageSource;
                    {
                        this.GenreBoxGrid.Tag = "LAP";
                    }
                    // 显示列表
                    this.GenreBoxGrid.Visibility = Visibility.Visible;
                    {
                        this.Build();
                    }
                }
            }
        }

        /// <summary>
        /// GenreBoxGrid离开事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="evt"></param>
        private void GenreBoxGrid_MouseLeave(object obj, MouseEventArgs evt)
        {
            Point point = evt.GetPosition(this.GenreBoxGrid);
            if (point.X < 0 || point.Y < 0 || point.X > this.GenreBoxGrid.ActualWidth || point.Y > this.GenreBoxGrid.ActualHeight)
            {
                this.GenreBoxGrid.Visibility = Visibility.Collapsed;
                {
                    this.GenreMap.Source = this.FindResource("GENRE_01") as ImageSource;
                    this.GenreLap.Source = this.FindResource("GENRE_04") as ImageSource;
                }
            }
        }

        /// <summary>
        /// GenreBoxGrid进入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenreBoxGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!MatchUtils.IsEmpty(this.genre.Target) && this.genre.Target.Enable && this.genre.Enable)
            {
                this.GenreBoxGrid.Visibility = Visibility.Visible;
                {
                    switch ((string)this.GenreBoxGrid.Tag)
                    {
                        case "MAP":
                            {
                                this.GenreMap.Source = this.FindResource("left".Equals(this.genre.Anchor) ? "GENRE_03" : "GENRE_02") as ImageSource;
                                break;
                            }
                        case "LAP":
                            {
                                this.GenreLap.Source = this.FindResource("left".Equals(this.genre.Anchor) ? "GENRE_06" : "GENRE_05") as ImageSource;
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// GenreBoxList选项改变事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="evt"></param>
        private void GenreBox_SelectionChanged(object obj, SelectionChangedEventArgs evt)
        {
            ContentPresenter content = this.GenreBox_SelectionVisual<ContentPresenter>(
                this.GenreBox.ItemContainerGenerator.ContainerFromItem(this.GenreBox.SelectedItem)
            );
            if (!MatchUtils.IsEmpty(content))
            {
                Grid grid = null;
                try
                {
                    grid = (Grid)content.ContentTemplate.FindName("GenreBoxItem", content);
                }
                catch
                {
                    grid = null;
                }
                finally
                {
                    if (grid != null)
                    {
                        switch ((string)this.GenreBoxGrid.Tag)
                        {
                            case "MAP":
                                {
                                    Geog tile = null;
                                    try
                                    {
                                        tile = this.genre.Target.Symbol.Tile[grid.Tag as string];
                                    }
                                    catch
                                    {
                                        tile = null;
                                    }
                                    finally
                                    {
                                        if (tile != null)
                                        {
                                            tile.Access(true);
                                        }
                                    }
                                    break;
                                }
                            case "LAP":
                                {
                                    Geog pile = null;
                                    try
                                    {
                                        pile = this.genre.Target.Symbol.Pile[grid.Tag as string];
                                    }
                                    catch
                                    {
                                        pile = null;
                                    }
                                    finally
                                    {
                                        if (pile != null)
                                        {
                                            pile.Access(!pile.Allow);
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            // 清空选中
            this.GenreBox.SelectedItem = null;
        }

        /// <summary>
        /// 获取ContentPresenter 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private DependencyVisual GenreBox_SelectionVisual<DependencyVisual>(DependencyObject obj) where DependencyVisual : DependencyObject
        {
            if (!MatchUtils.IsEmpty(obj))
            {
                DependencyObject root = null;
                DependencyVisual leaf = null;
                {
                    for (int i = 0, l = VisualTreeHelper.GetChildrenCount(obj); i < l; i++)
                    {
                        if (!MatchUtils.IsEmpty(root = VisualTreeHelper.GetChild(obj, i)))
                        {
                            if (root is DependencyVisual)
                            {
                                return (DependencyVisual)root;
                            }
                            else
                            {
                                if (!MatchUtils.IsEmpty(leaf = this.GenreBox_SelectionVisual<DependencyVisual>(root)))
                                {
                                    return leaf;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}