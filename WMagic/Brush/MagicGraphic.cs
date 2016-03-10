using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WMagic.Brush.Basic;
using WMagic.Brush.Shape;

namespace WMagic.Brush
{

    /// <summary>
    /// 线型枚举
    /// </summary>
    public enum GLinear
    {
        DOT,
        DASH,
        SOLID,
        DASHDOT
    }

    /// <summary>
    /// 2D绘图接口类
    /// </summary>
    public class MagicGraphic
    {
        #region 变量

        // 图元序列
        private long count;
        // 缓存对象
        private AShape cache;
        // 绘图面板
        private IGraphic panel;
        // 绘图画布
        private FrameworkElement canvas;

        #endregion

        #region 依赖属性

        // 注册图元宿主依赖
        internal static readonly DependencyProperty Nexus = DependencyProperty.Register(
            "_$WMagic.Brush.Nexus_", typeof(AShape), typeof(DrawingVisual), new PropertyMetadata(null)
        );

        #endregion

        #region 构造函数

        public MagicGraphic(IGraphic panel)
        {
            if (!MatchUtils.IsEmpty(panel))
            {
                this.count = 0;
                this.cache = null;
                this.panel = panel;
                // 事件绑定
                {
                    this.InitializeEventListener(this.panel as FrameworkElement);
                }
            }
        }

        #endregion

        #region 属性方法

        internal string Index
        {
            get { return StringUtils.toString(this.count++); }
        }

        internal IGraphic Panel
        {
            get { return this.panel; }
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 清空舞台
        /// </summary>
        public void ClearStage()
        {
            // 几何对象
            AShape shape = null;
            // 图形对象
            DrawingVisual brush = null;
            for (int i = this.panel.AmountVisual() - 1; i > -1; i--)
            {
                if (!MatchUtils.IsEmpty(brush = this.panel.ObtainVisual(i) as DrawingVisual) && !MatchUtils.IsEmpty(shape = brush.GetValue(MagicGraphic.Nexus) as AShape))
                {
                    shape.Earse();
                    {
                        shape = null;
                        brush = null;
                    }
                }
            }
        }

        /// <summary>
        /// 刷新舞台
        /// </summary>
        public void RenewStage()
        {
            // 几何对象
            AShape shape = null;
            // 图形对象
            DrawingVisual brush = null;
            for (int i = this.panel.AmountVisual() - 1; i > -1; i--)
            {
                if (!MatchUtils.IsEmpty(brush = this.panel.ObtainVisual(i) as DrawingVisual) && !MatchUtils.IsEmpty(shape = brush.GetValue(MagicGraphic.Nexus) as AShape))
                {
                    shape.Paint();
                    {
                        shape = null;
                        brush = null;
                    }
                }
            }
        }

        /// <summary>
        /// 创建线条
        /// </summary>
        public GLine CreateLine()
        {
            return new GLine(this);
        }

        /// <summary>
        /// 创建矩形
        /// </summary>
        public GRect CreateRect()
        {
            return new GRect(this);
        }

        /// <summary>
        /// 创建多边形
        /// </summary>
        public GPoly CreatePoly()
        {
            return new GPoly(this);
        }

        /// <summary>
        /// 创建圆形
        /// </summary>
        public GOval CreateOval()
        {
            return new GOval(this);
        }

        /// <summary>
        /// 创建扇形
        /// </summary>
        public GArch CreateArch()
        {
            return new GArch(this);
        }

        /// <summary>
        /// 创建曲线
        /// </summary>
        public GWave CreateWave()
        {
            return new GWave(this);
        }

        /// <summary>
        /// 创建图标
        /// </summary>
        public GIcon CreateIcon()
        {
            return new GIcon(this);
        }

        /// <summary>
        /// 创建文本
        /// </summary>
        public GText CreateText()
        {
            return null;
        }

        #endregion

        #region 事件响应

        /// <summary>
        /// 初始化事件监听
        /// </summary>
        private void InitializeEventListener(FrameworkElement canvas)
        {
            (this.canvas = canvas).ToolTip = null;
            {
                canvas.MouseDown += new MouseButtonEventHandler(OnMouseDownEventListener);
                canvas.MouseMove += new MouseEventHandler(OnMouseMoveEventListener);
                canvas.MouseUp += new MouseButtonEventHandler(OnMouseUpEventListener);
                canvas.MouseLeave += new MouseEventHandler(OnMouseLeaveEventListener);
                canvas.MouseWheel += new MouseWheelEventHandler(OnMouseWheelEventListener);
                canvas.ContextMenuOpening += new ContextMenuEventHandler(OnContextMenuOpeningEventListener);
            }
        }

        /// <summary>
        /// 鼠标按下事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDownEventListener(object sender, MouseButtonEventArgs e)
        {
            if (!MatchUtils.IsEmpty(cache))
            {
                cache.OnMouseDown(e);
            }
        }

        /// <summary>
        /// 鼠标移动事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMoveEventListener(object sender, MouseEventArgs e)
        {
            AShape shape = null;
            try
            {
                HitTestResult target = VisualTreeHelper.HitTest(this.canvas, e.GetPosition(this.canvas));
                if (!MatchUtils.IsEmpty(target) && !MatchUtils.IsEmpty(target.VisualHit))
                {
                    shape = (target.VisualHit as DrawingVisual).GetValue(MagicGraphic.Nexus) as AShape;
                }
            }
            catch
            {
                shape = null;
            }
            finally
            {
                if (!MatchUtils.IsEmpty(shape))
                {
                    this.canvas.Cursor = shape.Mouse;
                    this.canvas.ToolTip = shape.Title;
                    try
                    {
                        shape.OnMouseMove(e);
                    }
                    finally
                    {
                        if (!shape.Equals(cache))
                        {
                            try
                            {
                                if (!MatchUtils.IsEmpty(cache))
                                {
                                    cache.OnMouseLeave(e);
                                }
                            }
                            finally
                            {
                                (cache = shape).OnMouseEnter(e);
                            }
                        }
                    }
                }
                else
                {
                    this.canvas.Cursor = null;
                    this.canvas.ToolTip = null;
                    try
                    {
                        if (!MatchUtils.IsEmpty(cache))
                        {
                            cache.OnMouseLeave(e);
                        }
                    }
                    finally
                    {
                        cache = null;
                    }
                }
            }
        }

        /// <summary>
        /// 鼠标弹起事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUpEventListener(object sender, MouseButtonEventArgs e)
        {
            if (!MatchUtils.IsEmpty(cache))
            {
                cache.OnMouseUp(e);
            }
        }

        /// <summary>
        /// 鼠标离开事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeaveEventListener(object sender, MouseEventArgs e)
        {
            if (!MatchUtils.IsEmpty(cache))
            {
                cache.OnMouseLeave(e);
            }
        }

        /// <summary>
        /// 滚动鼠标滚轮事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseWheelEventListener(object sender, MouseWheelEventArgs e)
        {
            if (!MatchUtils.IsEmpty(cache))
            {
                cache.OnMouseWheel(e);
            }
        }

        /// <summary>
        /// 菜单弹出事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnContextMenuOpeningEventListener(object sender, ContextMenuEventArgs e)
        {
            if (!MatchUtils.IsEmpty(cache))
            {
                cache.OnContextMenuOpening(e);
            }
        }

        #endregion
    }
}