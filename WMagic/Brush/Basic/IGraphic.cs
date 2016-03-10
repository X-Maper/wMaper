using System.Windows.Media;

namespace WMagic.Brush.Basic
{
    /// <summary>
    /// 绘图接口
    /// </summary>
    public interface IGraphic
    {
        /// <summary>
        /// 获取画板的子对象数目
        /// </summary>
        /// <returns>总数</returns>
        int AmountVisual();

        /// <summary>
        /// 销毁Visual
        /// </summary>
        void DamageVisual();

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

        /// <summary>
        /// 获取相应索引的对象
        /// </summary>
        /// <param name="index">Visual索引</param>
        /// <returns></returns>
        Visual ObtainVisual(int index);
    }
}