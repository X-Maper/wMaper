namespace WMaper.Meta
{
    /// <summary>
    /// 字体类
    /// </summary>
    public sealed class Fonts
    {
        #region 变量

        // 字体大小
        private int size;
        // 是否加粗
        private bool bold;
        // 是否倾斜
        private bool ital;

        #endregion

        #region 属性方法

        public int Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

        public bool Bold
        {
            get { return this.bold; }
            set { this.bold = value; }
        }

        public bool Ital
        {
            get { return this.ital; }
            set { this.ital = value; }
        }

        #endregion

        #region 构造函数

        public Fonts(int size)
            : this(size, false, false)
        { }

        public Fonts(int size, bool bold, bool ital)
        {
            this.size = size;
            this.bold = bold;
            this.ital = ital;
        }

        #endregion
    }
}