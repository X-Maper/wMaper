namespace WMaper.Meta
{
    /// <summary>
    /// 拼装信息类
    /// </summary>
    public sealed class Assem
    {

        #region 变量

        // 拼装最小行
        private int minR;
        // 拼装最大行
        private int maxR;
        // 拼装最小列
        private int minC;
        // 拼装最大列
        private int maxC;

        #endregion

        #region 属性

        public int MinR
        {
            get { return this.minR; }
        }

        public int MaxR
        {
            get { return this.maxR; }
        }

        public int MinC
        {
            get { return this.minC; }
        }

        public int MaxC
        {
            get { return this.maxC; }
        }

        #endregion

        #region 构造函数

        public Assem()
            : this(0, 0, 0, 0)
        { }

        public Assem(int minR, int minC, int maxR, int maxC)
        {
            this.minR = minR;
            this.minC = minC;
            this.maxR = maxR;
            this.maxC = maxC;
        }

        #endregion
    }
}