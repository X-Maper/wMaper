using System;
using WMagic;
using WMaper.Base;
using WMaper.Meta;
using WMaper.Meta.Param;
using WMaper.Plat;
using WMaper.Proj;

namespace WMaper.Norm.ARC
{
    /// <summary>
    /// ArcGISCache瓦片类
    /// </summary>
    public sealed class ArcGISCache : Tile
    {
        #region 变量

        private string format;
        private Func<String> path;

        #endregion

        #region 属性方法

        public string Format
        {
            get { return this.format; }
            set { this.format = value; }
        }

        public Func<String> Path
        {
            get { return this.path; }
            set { this.path = value; }
        }

        #endregion

        #region 构造函数

        public ArcGISCache()
            : base()
        {
            this.Format = "png";
        }

        public ArcGISCache(Option option)
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
                if (option.Exist("Format"))
                    this.Format = option.Fetch<string>("Format");
                if (option.Exist("Path"))
                    this.Path = option.Fetch<Func<String>>("Path");
            }
        }

        #endregion

        #region 函数方法

        protected sealed override string Source(int l, int r, int c)
        {
            try
            {
                return this.Path.Invoke() + "/L" + Convert.ToString(this.Radix + this.Start + l).PadLeft(2, '0') + "/R" + Convert.ToString(r, 16).PadLeft(8, '0') + "/C" + Convert.ToString(c, 16).PadLeft(8, '0') + "." + this.Format;
            }
            catch
            {
                return "#";
            }
        }

        #endregion
    }
}