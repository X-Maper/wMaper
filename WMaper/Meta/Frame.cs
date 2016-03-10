using System.Windows.Media;

namespace WMaper.Meta
{
    public sealed class Frame
    {
        /// <summary>
        /// 线型枚举
        /// </summary>
        public enum Linear
        {
            DOT,
            DASH,
            SOLID,
            DASHDOT
        }

        #region 变量

        private int thick;
        private Color color;
        private Linear style;

        #endregion

        #region 属性方法

        public int Thick
        {
            get { return this.thick; }
            set { this.thick = value; }
        }

        public Color Color
        {
            get { return this.color; }
            set { this.color = value; }
        }

        public Linear Style
        {
            get { return this.style; }
            set { this.style = value; }
        }

        #endregion

        #region 构造函数

        public Frame()
            : this(1)
        { }

        public Frame(int thick)
            : this(thick, Linear.SOLID)
        { }

        public Frame(int thick, Color color)
            : this(thick, color, Linear.SOLID)
        { }

        public Frame(int thick, Linear style)
            : this(thick, Color.FromRgb(255, 0, 0), style)
        { }

        public Frame(int thick, Color color, Linear style)
        {
            this.thick = thick;
            this.color = color;
            this.style = style;
        }

        #endregion
    }
}