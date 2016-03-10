using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using WMagic.Brush.Basic;

namespace WMaper.Misc.View.Ware
{
    public sealed class Scene : Canvas, IGraphic
    {
        private List<Visual> shapes = new List<Visual>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public Scene()
        {
            this.Background = new SolidColorBrush(Colors.Transparent);
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return this.Children.Count + this.shapes.Count;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            int count;
            {
                count = this.Children.Count;
            }
            return count > index ? this.Children[index] : this.shapes[index - count];
        }

        /// <summary>
        /// 获取Amount
        /// </summary>
        /// <returns></returns>
        public int AmountVisual()
        {
            return this.VisualChildrenCount;
        }

        /// <summary>
        /// 销毁Visual
        /// </summary>
        public void DamageVisual()
        {
            base.Children.Clear();
            {
                foreach (Visual visual in this.shapes)
                {
                    try
                    {
                        base.RemoveVisualChild(visual);
                        base.RemoveLogicalChild(visual);
                    }
                    catch
                    {
                        continue;
                    }
                }
                // 清空缓存
                this.shapes.Clear();
            }
        }

        /// <summary>
        /// 获取Visual
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Visual ObtainVisual(int index)
        {
            return this.GetVisualChild(index);
        }

        /// <summary>
        /// 添加Visual
        /// </summary>
        /// <param name="visual"></param>
        public void AppendVisual(Visual visual)
        {
            this.shapes.Add(visual);
            {
                base.AddVisualChild(visual);
                base.AddLogicalChild(visual);
            }
        }

        /// <summary>
        /// 移除Visual
        /// </summary>
        /// <param name="visual"></param>
        public void RemoveVisual(Visual visual)
        {
            this.shapes.Remove(visual);
            {
                base.RemoveVisualChild(visual);
                base.RemoveLogicalChild(visual);
            }
        }

    }
}