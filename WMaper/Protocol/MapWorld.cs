using System;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WMagic;
using WMaper.Base;
using WMaper.Meta.Param;
using WMaper.Plat;
using WMaper.Proj.Epsg;

namespace WMaper.Protocol
{
    /// <summary>
    /// 天地图瓦片类
    /// </summary>
    public sealed class MapWorld : Tile
    {
        private static readonly Regex AMEND_REGEX = new Regex("^(VEC|TER)$", RegexOptions.IgnoreCase);
        private static readonly Regex STYLE_REGEX = new Regex("^(VEC|IMG|TER|C(V|I|T)A)$", RegexOptions.IgnoreCase);

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
                if (MapWorld.STYLE_REGEX.IsMatch(value))
                {
                    this.style = value.ToLower();
                }
            }
        }

        #endregion

        #region 构造函数

        public MapWorld()
            : base()
        {
            // 默认配置
            this.Radix = 1;
            this.Close = 19;
            this.Style = "vec";
            this.Units = WMaper.Units.DD;
            this.Projcs = new EPSG_4326();
            this.Origin = new Coord(-180, 90);
            this.Extent = new Bound(new Coord(-180, 90), new Coord(180, -90));
            this.Factor = new double[] { 0.703125, 0.3515625, 0.17578125, 0.087890625, 0.0439453125, 0.02197265625, 0.010986328125, 0.0054931640625, 0.00274658203125, 0.001373291015625, 6.866455078125E-4, 3.4332275390625E-4, 1.71661376953125E-4, 8.58306884765625E-5, 4.291534423828125E-5, 2.1457672119140625E-5, 1.0728836059570313E-5, 5.36441802978515625E-6, 2.682209014892578E-6 };
        }

        public MapWorld(Option option)
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

        protected sealed override ImageSource Repair(int l, int r, int c)
        {
            return new BitmapImage(new Uri("http://api.tianditu.com/img/" + (MapWorld.AMEND_REGEX.IsMatch(this.style) ? this.style : "") + "404.png", UriKind.Absolute));
        }

        protected sealed override string Source(int l, int r, int c)
        {
            return "http://t" + (new Random()).Next(0, 7) + ".tianditu.com/" + this.Style + "_c/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=" + this.Style + "&STYLE=default&TILEMATRIXSET=c&TILEMATRIX=" + (this.Radix + this.Start + l) + "&TILEROW=" + r + "&TILECOL=" + c + "&FORMAT=tiles";
        }

        #endregion
    }
}