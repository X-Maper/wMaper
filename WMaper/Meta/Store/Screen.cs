using System;
using System.Windows.Controls;

namespace WMaper.Meta.Store
{
    /// <summary>
    /// 地图屏幕
    /// </summary>
    public sealed class Screen
    {
        #region 变量

        // 屏幕左移
        private double l;
        // 屏幕上移
        private double t;
        // 屏幕宽度
        private double w;
        // 屏幕高度
        private double h;
        // 半屏宽度
        private double x;
        // 半屏高度
        private double y;

        #endregion

        #region 属性方法

        public double L
        {
            get { return this.l; }
            set { this.l = value; }
        }

        public double T
        {
            get { return this.t; }
            set { this.t = value; }
        }

        public double W
        {
            get { return this.w; }
            set { this.w = value; }
        }

        public double H
        {
            get { return this.h; }
            set { this.h = value; }
        }

        public double X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        #endregion

        #region 构造函数

        public Screen()
        {
            this.l = 0;
            this.t = 0;
        }

        public Screen(Panel facade)
            : this()
        {
            if (!Double.NaN.Equals(facade.Width) && !Double.NaN.Equals(facade.Height))
            {
                this.x = (this.w = facade.Width) / 2;
                this.y = (this.h = facade.Height) / 2;
            }
            else
            {
                this.x = (this.w = !Double.NaN.Equals(facade.ActualWidth) ? facade.ActualWidth : 0) / 2;
                this.y = (this.h = !Double.NaN.Equals(facade.ActualHeight) ? facade.ActualHeight : 0) / 2;
            }
        }

        #endregion
    }
}