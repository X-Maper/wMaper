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
    /// Wmts服务
    /// </summary>
    public sealed class Wmts : Tile
    {
        #region 变量

        private string layer;
        private string style;
        private string matrix;
        private string format;
        private string service;
        private string request;
        private string version;
        private string[] assign;
        private Hashtable query;
        private Func<String> path;

        #endregion

        #region 属性方法

        public string Layer
        {
            get { return this.layer; }
            set { this.layer = value; }
        }

        public string Style
        {
            get { return this.style; }
            set { this.style = value; }
        }

        public string Matrix
        {
            get { return this.matrix; }
            set { this.matrix = value; }
        }

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

        public string Request
        {
            get { return this.request; }
            set { this.request = value; }
        }

        public string Version
        {
            get { return this.version; }
            set { this.version = value; }
        }

        public string[] Assign
        {
            get { return this.assign; }
            set { this.assign = value; }
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

        public Wmts()
            : base()
        {
            this.format = "image/png";
            this.service = "WMTS";
            this.version = "1.0.0";
            this.request = "GetTile";
        }

        public Wmts(Option option)
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
                if (option.Exist("Layer"))
                    this.Layer = option.Fetch<String>("Layer");
                if (option.Exist("Style"))
                    this.Style = option.Fetch<String>("Style");
                if (option.Exist("Matrix"))
                    this.Matrix = option.Fetch<String>("Matrix");
                if (option.Exist("Format"))
                    this.Format = option.Fetch<String>("Format");
                if (option.Exist("Service"))
                    this.Service = option.Fetch<String>("Service");
                if (option.Exist("Request"))
                    this.Request = option.Fetch<String>("Request");
                if (option.Exist("Version"))
                    this.Version = option.Fetch<String>("Version");
                if (option.Exist("Assign"))
                    this.Assign = option.Fetch<String[]>("Assign");
                if (option.Exist("Query"))
                    this.Query = option.Fetch<Hashtable>("Query");
                if (option.Exist("Path"))
                    this.Path = option.Fetch<Func<String>>("Path");
            }
        }

        #endregion

        #region 函数方法

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

        protected sealed override string Source(int l, int r, int c)
        {
            l += this.Radix + this.Start;
            try
            {
                return String.Join("", new string[] { 
                    this.Path(), "?SERVICE=", this.Service, "&REQUEST=", this.Request, "&VERSION=", this.Version, "&LAYER=", this.Layer, "&STYLE=", this.Style, "&TILEMATRIXSET=", this.Matrix, "&TILEMATRIX=", !MatchUtils.IsEmpty(this.Assign) ? this.Assign[l] : Convert.ToString(l), "&TILEROW=" + r, "&TILECOL=" + c, "&FORMAT=", this.Format, (
                        !MatchUtils.IsEmpty(this.Query) ? "&" + this.Q2req(this.Query) : ""
                    )
                });
            }
            catch
            {
                return "#";
            }
        }

        #endregion
    }
}