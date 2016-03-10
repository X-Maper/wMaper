using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WMagic.Brush.Basic
{
    /// <summary>
    /// 绘图接口
    /// </summary>
    public interface IGeometry
    {
        /// <summary>
        /// 添加Visual
        /// </summary>
        /// <param name="visual">Visual对象</param>
        void AppendVisual(Visual visual);

        /// <summary>
        /// 移除Visual
        /// </summary>
        /// <param name="visual">Visual对象</param>
        void RemoveVisual(Visual visual);
    }
}
