using System.Windows;
using WMaper;
using WMaper.Base;
using WMaper.Meta.Param;
using WMaper.Plug;
using WMaper.Protocol;

namespace WMaperTest
{
    /// <summary>
    /// AppDemo.xaml 的交互逻辑
    /// </summary>
    public partial class AppDemo : Window
    {
        private Maper wingis;

        public AppDemo()
        {
            InitializeComponent();
            {
                wingis = new Maper(this.wMpaer, (new Option("Assets", "zh_Hans")).Append(
                    "Limits", new Bound(new Coord(-180, 90), new Coord(180, -90))
                ).Append(
                    "Center", new Coord(119.28, 26.08)
                ));
                // Google卫星
                wingis.Include(new Google(Option.Create("Level", 7).Append("Style", "s").Append("Title", "谷歌卫星")));
                // 天地图
                wingis.Include(new MapWorld(Option.Create("Level", 7).Append("Style", "vec").Append("Title", "天地图")));
                // Google标注
                wingis.Include(new Google(Option.Create("Level", 7).Append("Style", "h").Append("Title", "谷歌标注").Append("Cover", true).Append("Allow", false)));
                // 控制条
                wingis.Include(new Slide());
                // 图层项
                wingis.Include(new Genre());
                // 比例尺
                wingis.Include(new Scale());
            }
        }
    }
}
