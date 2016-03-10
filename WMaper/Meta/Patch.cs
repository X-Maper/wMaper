namespace WMaper.Meta
{
    /// <summary>
    /// 瓦片信息类
    /// </summary>
    public sealed class Patch
    {
        #region 变量

        // 瓦片级数
        private int num;
        // 瓦片所在行
        private int row;
        // 瓦片所在列
        private int col;
        // 瓦片标识
        private string uid;

        #endregion

        #region 属性

        public string Uid
        {
            get { return this.uid; }
        }

        public int Num
        {
            get { return this.num; }
        }

        public int Row
        {
            get { return this.row; }
        }

        public int Col
        {
            get { return this.col; }
        }

        #endregion

        #region 构造函数

        public Patch(string uid, int num, int row, int col)
        {
            this.uid = uid;
            this.num = num;
            this.row = row;
            this.col = col;
        }

        #endregion
    }
}