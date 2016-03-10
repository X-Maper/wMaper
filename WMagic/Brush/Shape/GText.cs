using System.Windows.Media;
using WMagic.Brush.Basic;

namespace WMagic.Brush.Shape
{
    public class GText : AShape
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="graph">图元画板</param>
        public GText(MagicGraphic graph)
            : base(graph, new DrawingVisual())
        { }

        #endregion

        /// <summary>
        /// 线条渲染
        /// </summary>
        protected sealed override void Render()
        { }
    }
}