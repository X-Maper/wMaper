using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WMagic;

namespace WMaper.Misc
{
    /// <summary>
    /// Share.xaml 的交互逻辑
    /// </summary>
    public sealed partial class Share : UserControl
    {
        #region 变量

        // 光标资源
        private Cursor dragCur;
        private Cursor freeCur;
        // 图片资源
        private ImageSource blankSrc;
        private ImageSource causeSrc;

        #endregion

        #region 构造函数

        public Share()
        {
            InitializeComponent();
            {
                // 光标资源
                this.dragCur = ((TextBlock)this.Resources["CursorDrag"]).Cursor;
                this.freeCur = ((TextBlock)this.Resources["CursorFree"]).Cursor;
                // 图片资源
                if (!MatchUtils.IsEmpty(this.blankSrc = (ImageSource)this.Resources["SourceBlank"]) && this.blankSrc.CanFreeze)
                {
                    this.blankSrc.Freeze();
                }
                if (!MatchUtils.IsEmpty(this.causeSrc = (ImageSource)this.Resources["SourceCause"]) && this.causeSrc.CanFreeze)
                {
                    this.causeSrc.Freeze();
                }
            }
        }

        #endregion

        #region 属性方法

        public Cursor DragCur
        {
            get { return this.dragCur; }
        }

        public Cursor FreeCur
        {
            get { return this.freeCur; }
        }

        public ImageSource BlankSrc
        {
            get { return this.blankSrc; }
        }

        public ImageSource CauseSrc
        {
            get { return this.causeSrc; }
        }

        #endregion

        #region 函数方法

        public void Dispose()
        {
            this.Resources.Clear();
            {
                this.dragCur = null;
                this.freeCur = null;
                this.blankSrc = null;
                this.causeSrc = null;
            }
        }

        #endregion
    }
}