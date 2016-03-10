using WMagic;
using WMaper.Plug;

namespace WMaper.Meta.Store
{
    /// <summary>
    /// 控件缓存
    /// </summary>
    public sealed class Widget
    {
        #region 变量

        private Genre genre;
        private Slide slide;
        private Scale scale;
        private Eagle eagle;
        private Tools tools;
        private Menus menus;
        private Popup popup;
        private About about;
        private Await await;

        #endregion

        #region 属性

        public Genre Genre
        {
            get { return this.genre; }
            set { this.genre = value; }
        }

        public Slide Slide
        {
            get { return this.slide; }
            set { this.slide = value; }
        }

        public Scale Scale
        {
            get { return this.scale; }
            set { this.scale = value; }
        }

        public Eagle Eagle
        {
            get { return this.eagle; }
            set { this.eagle = value; }
        }

        public Tools Tools
        {
            get { return this.tools; }
            set { this.tools = value; }
        }

        public Menus Menus
        {
            get { return this.menus; }
            set { this.menus = value; }
        }

        public Popup Popup
        {
            get { return this.popup; }
            set { this.popup = value; }
        }

        public About About
        {
            get { return this.about; }
            set { this.about = value; }
        }

        public Await Await
        {
            get { return this.await; }
            set { this.await = value; }
        }

        #endregion

        #region 构造函数

        public Widget()
        {
            this.genre = null;
            this.slide = null;
            this.scale = null;
            this.eagle = null;
            this.tools = null;
            this.menus = null;
            this.popup = null;
            this.about = null;
            this.await = null;
        }

        #endregion

        #region 函数方法

        public void Dispose()
        {
            if (!MatchUtils.IsEmpty(this.genre))
            {
                this.genre.Remove();
                {
                    this.genre = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.slide))
            {
                this.slide.Remove();
                {
                    this.slide = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.scale))
            {
                this.scale.Remove();
                {
                    this.scale = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.eagle))
            {
                this.eagle.Remove();
                {
                    this.eagle = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.tools))
            {
                this.tools.Remove();
                {
                    this.tools = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.menus))
            {
                this.menus.Remove();
                {
                    this.menus = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.popup))
            {
                this.popup.Remove();
                {
                    this.popup = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.about))
            {
                this.about.Remove();
                {
                    this.about = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.await))
            {
                this.await.Remove();
                {
                    this.await = null;
                }
            }
        }

        #endregion
    }
}