using System;
using System.Windows.Media;
using WMagic;

namespace WMaper.Meta.Embed
{
    public sealed class Tool
    {
        #region 变量

        // 是否启用
        private bool allow;
        // 工具名称
        private string label;
        // 聚焦图片
        private ImageSource press;
        // 失焦图片
        private ImageSource loose;
        // 触发回调
        private Action<int> visit;

        #endregion

        #region 属性方法

        public bool Allow
        {
            get { return this.allow; }
            set { this.allow = value; }
        }

        public string Label
        {
            get { return this.label; }
            set { this.label = value; }
        }

        public ImageSource Press
        {
            get { return this.press; }
            set
            {
                if (!MatchUtils.IsEmpty(this.press = value))
                {
                    // 冻结图像
                    if (this.press.CanFreeze)
                    {
                        this.press.Freeze();
                    }
                }
            }
        }

        public ImageSource Loose
        {
            get { return this.loose; }
            set
            {
                if (!MatchUtils.IsEmpty(this.loose = value))
                {
                    // 冻结图像
                    if (this.loose.CanFreeze)
                    {
                        this.loose.Freeze();
                    }
                }
            }
        }

        public Action<int> Visit
        {
            get { return this.visit; }
            set { this.visit = value; }
        }

        #endregion

        #region 构造函数

        public Tool(string label)
            : this(label, null, null)
        { }

        public Tool(string label, ImageSource press, ImageSource loose)
            : this(label, press, loose, null)
        { }

        public Tool(string label, ImageSource press, ImageSource loose, Action<int> visit)
        {
            this.allow = true;
            this.label = label;
            this.press = press;
            this.loose = loose;
            this.visit = visit;
        }

        #endregion
    }
}