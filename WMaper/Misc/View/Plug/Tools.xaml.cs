using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WMagic;
using WMaper.Meta.Embed;

namespace WMaper.Misc.View.Plug
{
    /// <summary>
    /// Tools.xaml 的交互逻辑
    /// </summary>
    public sealed partial class Tools : UserControl
    {
        #region 属性

        private bool ready;
        // 控件对象
        private WMaper.Plug.Tools tools;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Tools()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Tools(int sort)
            : this()
        {
            Panel.SetZIndex(this, sort);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Tools(WMaper.Plug.Tools tools, int sort)
            : this(sort)
        {
            this.ready = false;
            {
                (this.tools = tools).Facade = this.ToolsDecor;
                {
                    // 语言资源
                    this.ToolsResource.MergedDictionaries.Add(
                        this.tools.Target.Assets.Language
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
            // 清空控件
            this.ToolsImg.Source = null;
            {
                this.ToolsList.Items.Clear();
            }
            // 重绘控件
            Tool[] toolsData = null;
            try
            {
                lock (this.tools.Action)
                {
                    toolsData = this.tools.Action.Where(x => x.Allow).ToArray();
                }
            }
            catch
            {
                toolsData = null;
            }
            finally
            {
                if (!MatchUtils.IsEmpty(toolsData))
                {
                    ListBoxItem toolsItem = null;
                    {
                        for (int i = 0, l = toolsData.Length; i < l; i++)
                        {
                            (toolsItem = new ListBoxItem()).Style = this.FindResource(
                                this.tools.Active == i ? "TOOLS_LIST_SEL" : "TOOLS_LIST_DEF"
                            ) as Style;
                            {
                                toolsItem.DataContext = toolsData[i];
                            }
                            // 添加数据
                            this.ToolsList.Items.Add(toolsItem);
                        }
                    }
                }
            }
            // 激活控件
            if (!this.ready)
            {
                // 设置位置
                if ("left".Equals(this.tools.Anchor))
                {
                    this.ToolsList.Style = this.FindResource("TOOLS_LIST_LEFT") as Style;
                    {
                        Canvas.SetTop(this, 108);
                        Canvas.SetLeft(this, 18);
                    }
                }
                else
                {
                    this.ToolsList.Style = this.FindResource("TOOLS_LIST_RIGHT") as Style;
                    {
                        Canvas.SetTop(this, 108);
                        Canvas.SetRight(this, 53);
                    }
                }
                // 初始工具
                if (this.ToolsList.Items.Count > 0)
                {
                    this.Apply(this.tools.Active);
                }
            }
            else
            {
                // 更新图标
                if (this.ToolsList.Items.Count > 0)
                {
                    ImageSource toolsImg = null;
                    try
                    {
                        toolsImg = ((Tool)((ListBoxItem)this.ToolsList.Items.GetItemAt(this.tools.Active)).DataContext).Press;
                    }
                    catch
                    {
                        toolsImg = null;
                    }
                    finally
                    {
                        if (toolsImg != null)
                        {
                            this.ToolsImg.Source = toolsImg;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 选择工具
        /// </summary>
        public void Apply(int tool)
        {
            if (!MatchUtils.IsEmpty(this.tools.Target) && this.tools.Target.Enable && this.tools.Enable)
            {
                if ((!this.ready || this.tools.Active != tool) && tool > -1 && this.ToolsList.Items.Count > tool)
                {
                    this.ToolsList.SelectedIndex = tool;
                }
            }
        }

        #endregion

        #region 鼠标事件

        /// <summary>
        /// 鼠标移进按钮
        /// </summary>
        private void ToolsCtx_MouseEnter(object obj, MouseEventArgs evt)
        {
            if (!MatchUtils.IsEmpty(this.tools.Target) && this.tools.Target.Enable && this.tools.Enable)
            {
                if (this.ToolsList.Items.Count > 0)
                {
                    this.ToolsBtn.Background = this.FindResource("TOOL_03") as ImageBrush;
                    {
                        this.ToolsList.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        /// <summary>
        /// 鼠标移出按钮
        /// </summary>
        private void ToolsBtn_MouseLeave(object obj, MouseEventArgs evt)
        {
            Point point = evt.GetPosition(this.ToolsBtn);
            if (point.X < 0 || point.Y < 0 || point.X > this.ToolsBtn.ActualWidth || point.Y > this.ToolsBtn.ActualHeight)
            {
                this.ToolsList.Visibility = Visibility.Collapsed;
                {
                    this.ToolsBtn.Background = this.FindResource("TOOL_04") as ImageBrush;
                }
            }
        }

        /// <summary>
        /// 鼠标移出菜单
        /// </summary>
        private void ToolsList_MouseLeave(object obj, MouseEventArgs evt)
        {
            Point point = evt.GetPosition(this.ToolsList);
            if (point.X < 0 || point.Y < 0 || point.X > this.ToolsList.ActualWidth || point.Y > this.ToolsList.ActualHeight)
            {
                this.ToolsList.Visibility = Visibility.Collapsed;
                {
                    this.ToolsBtn.Background = this.FindResource("TOOL_04") as ImageBrush;
                }
            }
        }

        /// <summary>
        /// 鼠标选中事件
        /// </summary>
        private void ToolsList_SelectionChanged(object obj, SelectionChangedEventArgs evt)
        {
            ListBox listBox = obj as ListBox;
            {
                if (!MatchUtils.IsEmpty(listBox) && listBox.SelectedIndex > -1)
                {
                    if (!this.ready && (this.ready = true))
                    {
                        this.ToolsList_SelectionControl(listBox);
                    }
                    else
                    {
                        if (listBox.SelectedIndex != this.tools.Active)
                        {
                            // 还原默认
                            ListBoxItem item = null;
                            try
                            {
                                item = (ListBoxItem)this.ToolsList.Items.GetItemAt(this.tools.Active);
                            }
                            catch
                            {
                                item = null;
                            }
                            finally
                            {
                                if (!MatchUtils.IsEmpty(item))
                                {
                                    item.Style = this.FindResource("TOOLS_LIST_DEF") as Style;
                                }
                            }
                            // 工具选中
                            this.ToolsList_SelectionControl(listBox);
                        }
                    }
                }
            }
            // 清除选项
            this.ToolsList.SelectedItem = null;
        }

        /// <summary>
        /// 工具选中操作
        /// </summary>
        private void ToolsList_SelectionControl(ListBox box)
        {
            Tool tool = null;
            try
            {
                this.ToolsImg.Source = (tool = (Tool)((ListBoxItem)box.SelectedItem).DataContext).Press;
            }
            catch
            {
                tool = null;
            }
            finally
            {
                if (tool == null)
                {
                    this.ToolsImg.Source = null;
                    {
                        this.tools.Active = -1;
                    }
                }
                else
                {
                    try
                    {
                        ((ListBoxItem)box.SelectedItem).Style = this.FindResource("TOOLS_LIST_SEL") as Style;
                        {
                            this.tools.Active = box.SelectedIndex;
                        }
                    }
                    catch
                    {
                        this.ToolsImg.Source = null;
                        {
                            this.tools.Active = -1;
                        }
                    }
                    finally
                    {
                        // 执行回调
                        if (!MatchUtils.IsEmpty(tool.Visit) && this.tools.Active > -1)
                        {
                            tool.Visit.Invoke(this.tools.Active);
                        }
                    }
                }
            }
        }

        #endregion
    }
}