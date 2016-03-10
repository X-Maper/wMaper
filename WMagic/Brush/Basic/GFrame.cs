using System.Windows.Media;

namespace WMagic.Brush.Basic
{
    public class GFrame
    {

        #region 变量

        // 边框大小
        private int thick;
        // 边框颜色
        private Color color;
        // 边框样式
        private GLinear style;

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

        public GLinear Style
        {
            get { return this.style; }
            set { this.style = value; }
        }

        #endregion

        #region 构造函数

        public GFrame()
            : this(1)
        { }

        public GFrame(int thick)
            : this(thick, GLinear.SOLID)
        { }

        public GFrame(int thick, Color color)
            : this(thick, color, GLinear.SOLID)
        { }

        public GFrame(int thick, GLinear style)
            : this(thick, Color.FromRgb(255, 0, 0), style)
        { }

        public GFrame(int thick, Color color, GLinear style)
        {
            this.thick = thick;
            this.color = color;
            this.style = style;
        }

        #endregion

    }
}