using System.Windows.Controls;
using WMagic;
using WMaper.Misc.View.Ware;

namespace WMaper.Misc.View
{
    public sealed partial class Atlas : UserControl
    {
        #region 变量

        // 瓦片层
        private Scene tile;
        // 叠加层
        private Scene pile;
        // 注记层
        private Scene note;
        // 绘图层
        private Scene draw;
        // 地标层
        private Scene mark;
        // 提示层
        private Scene tips;

        #endregion

        #region 属性

        public Scene Tile
        {
            get { return this.tile; }
        }

        public Scene Pile
        {
            get { return this.pile; }
        }

        public Scene Note
        {
            get { return this.note; }
        }

        public Scene Draw
        {
            get { return this.draw; }
        }

        public Scene Mark
        {
            get { return this.mark; }
        }

        public Scene Tips
        {
            get { return this.tips; }
        }

        #endregion

        #region 构造函数

        public Atlas()
        {
            InitializeComponent();
            {
                this.tile = this.TileLayer;
                this.pile = this.PileLayer;
                this.note = this.NoteLayer;
                this.draw = this.DrawLayer;
                this.mark = this.MarkLayer;
                this.tips = this.TipsLayer;
            }
        }

        public Atlas(int sort)
            : this()
        {
            Panel.SetZIndex(this, sort);
        }

        #endregion

        #region 函数方法

        public void Dispose()
        {
            if (!MatchUtils.IsEmpty(this.tile))
            {
                this.tile.DamageVisual();
                {
                    this.tile = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.pile))
            {
                this.pile.DamageVisual();
                {
                    this.pile = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.note))
            {
                this.note.DamageVisual();
                {
                    this.note = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.draw))
            {
                this.draw.DamageVisual();
                {
                    this.draw = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.mark))
            {
                this.mark.DamageVisual();
                {
                    this.mark = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.tips))
            {
                this.tips.DamageVisual();
                {
                    this.tips = null;
                }
            }
            // 删除图层
            {
                (this.Content as Canvas).Children.Clear();
            }
        }

        #endregion
    }
}