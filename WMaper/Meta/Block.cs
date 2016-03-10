namespace WMaper.Meta
{
    /// <summary>
    /// 图块类
    /// </summary>
    public sealed class Block
    {
        #region 变量

        // 图块宽度
        private int wide;
        // 图块高度
        private int high;

        #endregion

        #region 属性方法

        public int Wide
        {
            get { return this.wide; }
            set { this.wide = value; }
        }

        public int High
        {
            get { return this.high; }
            set { this.high = value; }
        }

        #endregion

        #region 构造函数

        public Block()
            : this(0, 0)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="wide">宽度</param>
        /// <param name="high">高度</param>
        public Block(int wide, int high)
        {
            this.wide = wide;
            this.high = high;
        }

        #endregion
    }
}