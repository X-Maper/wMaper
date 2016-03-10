using System;
using WMagic;
using WMaper.Base;
using WMaper.Meta.Param;
using WMaper.Norm.ARC;
using WMaper.Proj.Epsg;

namespace WMaper.Protocol
{
    /// <summary>
    /// ArcGISOnline瓦片
    /// </summary>
    public sealed class ArcGISOnline : ArcGISRest
    {
        #region 构造函数

        public ArcGISOnline()
            : base()
        {
            // 默认配置
            this.Close = 19;
            this.Craft = 96;
            this.Units = WMaper.Units.M;
            this.Projcs = new EPSG_900913();
            this.Origin = new Coord(-180.00000002503606, 85.05112878196637);
            this.Extent = new Bound(new Coord(-180.00000002503606, 85.05112878196637), new Coord(180.00000002503606, -85.05112878196637));
            this.Factor = new double[] { 156543.033928, 78271.5169639999, 39135.7584820001, 19567.8792409999, 9783.93962049996, 4891.96981024998, 2445.98490512499, 1222.99245256249, 611.49622628138, 305.748113140558, 152.874056570411, 76.4370282850732, 38.2185141425366, 19.1092570712683, 9.55462853563415, 4.77731426794937, 2.38865713397468, 1.19432856685505, 0.597164283559817 };
        }

        public ArcGISOnline(Option option)
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
                if (option.Exist("Close"))
                    this.Close = option.Fetch<int>("Close");
                if (option.Exist("Start"))
                    this.Start = option.Fetch<int>("Start");
                if (option.Exist("Extent"))
                    this.Extent = option.Fetch<Bound>("Extent");
                if (option.Exist("Path"))
                    this.Path = option.Fetch<Func<String>>("Path");
            }
        }

        #endregion
    }
}