using System;
using System.Collections;
using WMagic;
using WMaper.Base;
using WMaper.Meta;
using WMaper.Meta.Param;
using WMaper.Plat;
using WMaper.Proj;

namespace WMaper.Norm.OGC
{
    /// <summary>
    /// Wms服务
    /// </summary>
    public sealed class Wms : Tile
    {
        #region 变量

        private string format;
        private string service;
        private string version;
        private string request;
        private string[] layer;
        private string[] style;
        private Hashtable query;
        private Func<String> path;

        #endregion

        #region 属性方法

        public string Format
        {
            get { return this.format; }
            set { this.format = value; }
        }

        public string Service
        {
            get { return this.service; }
            set { this.service = value; }
        }

        public string Version
        {
            get { return this.version; }
            set { this.version = value; }
        }

        public string Request
        {
            get { return this.request; }
            set { this.request = value; }
        }

        public string[] Layer
        {
            get { return this.layer; }
            set { this.layer = value; }
        }

        public string[] Style
        {
            get { return this.style; }
            set { this.style = value; }
        }

        public Hashtable Query
        {
            get { return this.query; }
            set { this.query = value; }
        }

        public Func<String> Path
        {
            get { return this.path; }
            set { this.path = value; }
        }

        #endregion

        #region 构造函数

        public Wms()
            : base()
        {
            this.format = "image/png";
            this.service = "WMS";
            this.version = "1.3.0";
            this.request = "GetMap";
        }

        public Wms(Option option)
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
                if (option.Exist("Service"))
                    this.Service = option.Fetch<String>("Service");
                if (option.Exist("Version"))
                    this.Version = option.Fetch<String>("Version");
                if (option.Exist("Request"))
                    this.Request = option.Fetch<String>("Request");
                if (option.Exist("Format"))
                    this.Format = option.Fetch<String>("Format");
                if (option.Exist("Layer"))
                    this.Layer = option.Fetch<String[]>("Layer");
                if (option.Exist("Style"))
                    this.Style = option.Fetch<String[]>("Style");
                if (option.Exist("Query"))
                    this.Query = option.Fetch<Hashtable>("Query");
                if (option.Exist("Path"))
                    this.Path = option.Fetch<Func<String>>("Path");
            }
        }

        #endregion

        #region 函数方法

        private string P2crs(Projcs p)
        {
            return !MatchUtils.IsEmpty(p) ? p.GetType().Name.Replace("_", ":") : "";
        }

        private string W2arr(String w)
        {
            return String.Join("", new string[]{
                w, "?SERVICE=", this.Service, "&VERSION=", this.Version, "&REQUEST=", this.Request, "&TRANSPARENT=TRUE&LAYERS=", !MatchUtils.IsEmpty(this.Layer) ? String.Join(",", this.Layer) : "", "&STYLES=", !MatchUtils.IsEmpty(this.Style) ? String.Join(",", this.Style) : "", "&WIDTH=" + this.Block.Wide, "&HEIGHT=" + this.Block.High, "&FORMAT=", this.Format, (
                    !MatchUtils.IsEmpty(this.Query) ? "&" + this.Q2req(this.Query) : ""
                )
            });
        }

        private string Q2req(Hashtable q)
        {
            if (!MatchUtils.IsEmpty(q))
            {
                ArrayList param = new ArrayList();
                {
                    foreach (DictionaryEntry e in q)
                    {
                        param.Add(e.Key + "=" + e.Value);
                    }
                }
                return String.Join("&", param.ToArray());
            }
            return "";
        }

        private string G2box(int r, int c, bool v)
        {
            Bound bbox = null;
            try
            {
                bbox = new Bound(
                    this.Px2crd(new Pixel(
                        c * this.Block.Wide,
                        r * this.Block.High
                    )),
                    this.Px2crd(new Pixel(
                        (c + 1) * this.Block.Wide,
                        (r + 1) * this.Block.High
                    ))
                );
            }
            catch
            {
                bbox = null;
            }
            return !Object.ReferenceEquals(bbox, null) ? (
                v ? bbox.Min.Lng + "," + bbox.Max.Lat + "," + bbox.Max.Lng + "," + bbox.Min.Lat : bbox.Min.Lat + "," + bbox.Max.Lng + "," + bbox.Max.Lat + "," + bbox.Min.Lng
            ) : "";
        }

        protected sealed override string Source(int l, int r, int c)
        {
            try
            {
                switch (this.version)
                {
                    case "1.0.0":
                    case "1.1.0":
                    case "1.1.1":
                        {
                            return this.W2arr(this.Path()) + "&SRS=" + this.P2crs(this.Projcs) + "&BBOX=" + this.G2box(r, c, true);
                        }
                    default:
                        {
                            return this.W2arr(this.Path()) + "&CRS=" + this.P2crs(this.Projcs) + "&BBOX=" + this.G2box(r, c, false);
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