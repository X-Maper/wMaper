using WMagic;
using WMaper.Base;
namespace WMaper.Meta.Store
{
    /// <summary>
    /// 地图监听
    /// </summary>
    public sealed class Listen
    {
        #region 变量

        // 地图拖动事件
        private Event dragEvent;
        // 地图缩放事件
        private Event zoomEvent;
        // 地图切换事件
        private Event swapEvent;

        #endregion

        #region 属性

        public Event DragEvent
        {
            get { return this.dragEvent; }
            set { this.dragEvent = value; }
        }

        public Event ZoomEvent
        {
            get { return this.zoomEvent; }
            set { this.zoomEvent = value; }
        }

        public Event SwapEvent
        {
            get { return this.swapEvent; }
            set { this.swapEvent = value; }
        }

        #endregion

        #region 函数

        public void Dispose()
        {
            if (!MatchUtils.IsEmpty(this.dragEvent))
            {
                this.dragEvent.Dispose();
                {
                    this.dragEvent = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.zoomEvent))
            {
                this.zoomEvent.Dispose();
                {
                    this.zoomEvent = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.swapEvent))
            {
                this.swapEvent.Dispose();
                {
                    this.swapEvent = null;
                }
            }
        }

        #endregion
    }
}