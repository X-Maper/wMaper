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
    /// ArcGISRest瓦片类
    /// </summary>
    public class ArcGISRest : Tile
    {
        #region 变量

        private string version;
        private Func<String> path;

        #endregion

        #region 属性方法

        public string Version
        {
            get { return this.version; }
            set { this.version = value; }
        }

        public Func<String> Path
        {
            get { return this.path; }
            set { this.path = value; }
        }

        #endregion

        #region 构造函数

        public ArcGISRest()
            : base()
        {
            this.Version = "10";
        }

        public ArcGISRest(Option option)
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
                if (option.Exist("Version"))
                    this.Version = option.Fetch<String>("Version");
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
                switch (this.Version)
                {
                    case "10":
                        {
                            return this.Path() + "/tile/" + (this.Radix + this.Start + l) + "/" + r + "/" + c;
                        }
                    case "9":
                        {
                            return "#";
                        }
                    default:
                        {
                            return "#";
                        }
                }
            }
            catch
            {
                return "#";
            }
        }

        #endregion
    }
}