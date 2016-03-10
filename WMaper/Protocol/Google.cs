using System;
using System.Text.RegularExpressions;
using WMagic;
using WMaper.Base;
using WMaper.Meta.Param;
using WMaper.Plat;
using WMaper.Proj.Epsg;

namespace WMaper.Protocol
{
    /// <summary>
    /// Google瓦片类
    /// </summary>
    public sealed class Google : Tile
    {
        private static readonly Regex STYLE_REGEX = new Regex("^((M|S|R)(,(H|T(RAFFIC)?))*|(H|T(RAFFIC)?))$", RegexOptions.IgnoreCase);

        #region 变量

        // 地图类型
        private string style;

        #endregion

        #region 属性方法

        public string Style
        {
            get { return this.style; }
            set
            {
                if (Google.STYLE_REGEX.IsMatch(value))
                {
                    this.style = value.ToLower();
                }
            }
        }

        #endregion

        #region 构造函数

        public Google()
            : base()
        {
            // 默认配置
            this.Close = 22;
            this.Style = "m";
            this.Units = WMaper.Units.M;
            this.Projcs = new EPSG_900913();
            this.Origin = new Coord(-180.00000002503606, 85.05112878196637);
            this.Extent = new Bound(new Coord(-180.00000002503606, 85.05112878196637), new Coord(180.00000002503606, -85.05112878196637));
            this.Factor = new double[] { 156543.03390625, 78271.516953125, 39135.7584765625, 19567.87923828125, 9783.939619140625, 4891.9698095703125, 2445.9849047851562, 1222.9924523925781, 611.4962261962891, 305.74811309814453, 152.87405654907226, 76.43702827453613, 38.218514137268066, 19.109257068634033, 9.554628534317017, 4.777314267158508, 2.388657133579254, 1.194328566789627, 0.5971642833948135, 0.29858214169740677, 0.14929107084870338, 0.07464553542435169 };
        }

        public Google(Option option)
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
                if (option.Exist("Style"))
                    this.Style = option.Fetch<string>("Style");
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
                if (option.Exist("Close"))
                    this.Close = option.Fetch<int>("Close");
                if (option.Exist("Start"))
                    this.Start = option.Fetch<int>("Start");
                if (option.Exist("Extent"))
                    this.Extent = option.Fetch<Bound>("Extent");
            }
        }

        #endregion

        #region 函数方法

        protected sealed override string Source(int l, int r, int c)
        {
            return "http://mt" + (new Random()).Next(0, 3) + ".google.cn/vt/lyrs=" + this.Style + "&hl=zh-CN&gl=CN&x=" + c + "&y=" + r + "&z=" + (this.Radix + this.Start + l);
        }

        #endregion
    }
}