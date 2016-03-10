using System;
using System.Windows.Media;
using WMagic;
using WMaper.Base;
using WMaper.Meta;
using WMaper.Meta.Param;
using WMaper.Plat;
using WMaper.Proj;

namespace WMaper.Norm
{
    public class Puzzle : Tile
    {
        #region 变量

        private Func<int, int, double, long> pieceX;
        private Func<int, int, double, long> pieceY;
        private Func<int, int, int, String> pieceUrl;
        private Func<int, int, int, ImageSource> patchSrc;

        #endregion

        #region 属性方法

        public Func<int, int, double, long> PieceX
        {
            get { return this.pieceX; }
            set { this.pieceX = value; }
        }

        public Func<int, int, double, long> PieceY
        {
            get { return this.pieceY; }
            set { this.pieceY = value; }
        }

        public Func<int, int, int, String> PieceUrl
        {
            get { return this.pieceUrl; }
            set { this.pieceUrl = value; }
        }

        public Func<int, int, int, ImageSource> PatchSrc
        {
            get { return this.patchSrc; }
            set { this.patchSrc = value; }
        }

        #endregion

        #region 构造函数

        public Puzzle()
            : base()
        { }

        public Puzzle(Option option)
            : this()
        {
            if (!MatchUtils.IsEmpty(option))
            {
                // 用户配置
                if (option.Exist("Index"))
                    this.Index = option.Fetch<string>("Index");
                if (option.Exist("Group"))
                    this.Group = option.Fetch<string>("Group");
                if (option.Exist("Title"))
                    this.Title = option.Fetch<string>("Title");
                if (option.Exist("Allow"))
                    this.Allow = option.Fetch<bool>("Allow");
                if (option.Exist("Cover"))
                    this.Cover = option.Fetch<bool>("Cover");
                if (option.Exist("Alarm"))
                    this.Alarm = option.Fetch<bool>("Alarm");
                if (option.Exist("Speed"))
                    this.Speed = option.Fetch<int>("Speed");
                if (option.Exist("Alpha"))
                    this.Alpha = option.Fetch<int>("Alpha");
                if (option.Exist("Level"))
                    this.Level = option.Fetch<int>("Level");
                if (option.Exist("Radix"))
                    this.Radix = option.Fetch<int>("Radix");
                if (option.Exist("Craft"))
                    this.Craft = option.Fetch<int>("Craft");
                if (option.Exist("Block"))
                    this.Block = option.Fetch<Block>("Block");
                if (option.Exist("Units"))
                    this.Units = option.Fetch<double>("Units");
                if (option.Exist("Projcs"))
                    this.Projcs = option.Fetch<Projcs>("Projcs");
                if (option.Exist("Factor"))
                    if (!MatchUtils.IsEmpty(this.Factor = option.Fetch<double[]>("Factor")))
                    {
                        this.Close = this.Factor.Length;
                    }
                if (option.Exist("Extent"))
                    this.Extent = option.Fetch<Bound>("Extent");
                if (option.Exist("Origin"))
                {
                    this.Origin = option.Fetch<Coord>("Origin");
                }
                else
                {
                    if (!MatchUtils.IsEmpty(this.Extent))
                    {
                        this.Origin = this.Extent.Min;
                    }
                }
                if (option.Exist("Close"))
                    this.Close = option.Fetch<int>("Close");
                if (option.Exist("Start"))
                    this.Start = option.Fetch<int>("Start");
                if (option.Exist("PieceX"))
                    this.pieceX = option.Fetch<Func<int, int, double, long>>("PieceX");
                if (option.Exist("PieceY"))
                    this.pieceY = option.Fetch<Func<int, int, double, long>>("PieceY");
                if (option.Exist("PieceUrl"))
                    this.pieceUrl = option.Fetch<Func<int, int, int, String>>("PieceUrl");
                if (option.Exist("PatchSrc"))
                    this.patchSrc = option.Fetch<Func<int, int, int, ImageSource>>("PatchSrc");
            }
        }

        #endregion

        #region 函数方法

        protected sealed override ImageSource Repair(int l, int r, int c)
        {
            return !Object.ReferenceEquals(this.patchSrc, null) ? this.patchSrc.Invoke(l, r, c) : (
                !this.Cover && this.Alarm ? this.Target.Sharer.CauseSrc : this.Target.Sharer.BlankSrc
            );
        }

        protected sealed override string Source(int l, int r, int c)
        {
            return !Object.ReferenceEquals(this.pieceUrl, null) ? this.pieceUrl.Invoke(l, r, c) : "#";
        }

        protected sealed override long Axis4x(int l, int c, double x)
        {
            return !Object.ReferenceEquals(this.pieceX, null) ? this.pieceX.Invoke(l, c, x) : Convert.ToInt64(
                Math.Round(c * this.Block.Wide - x)
            );
        }

        protected sealed override long Axis4y(int l, int r, double y)
        {
            return !Object.ReferenceEquals(this.pieceY, null) ? this.pieceY.Invoke(l, r, y) : Convert.ToInt64(
                Math.Round(r * this.Block.High - y)
            );
        }

        #endregion
    }
}