using System;
using System.Windows;
using WMagic;

namespace WMaper.Lang
{
    public sealed class Assets
    {
        private Uri DEFAULT_LANGUAGE = new Uri("pack://application:,,,/WMaper;component/Lang/Dict/zh_Hans.xaml", UriKind.RelativeOrAbsolute);

        #region 变量

        private ResourceDictionary language;

        public ResourceDictionary Language
        {
            get { return this.language; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Assets()
        {
            (this.language = new ResourceDictionary()).Source = DEFAULT_LANGUAGE;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dict">语言字典</param>
        public Assets(string dict)
        {
            {
                this.language = new ResourceDictionary();
            }
            this.Transform(dict);
        }

        #endregion

        #region 函数方法

        /// <summary>
        /// 语言转换
        /// </summary>
        /// <param name="dict">语言字典</param>
        public void Transform(string dict)
        {
            if (MatchUtils.IsEmpty(dict))
            {
                this.language.Source = DEFAULT_LANGUAGE;
            }
            else
            {
                try
                {
                    this.language.Source = new Uri("pack://application:,,,/WMaper;component/Lang/Dict/" + dict + ".xaml", UriKind.RelativeOrAbsolute);
                }
                catch
                {
                    this.language.Source = DEFAULT_LANGUAGE;
                }
            }
        }

        #endregion
    }
}