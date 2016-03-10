using System;
using WMaper.Base;
using WMaper.Core;
using WMaper.Meta.Radio;

namespace WMaper.Plat
{
    public abstract class Vect : Geog
    {
        public sealed override int Fusion(Maper drv)
        {
            throw new NotImplementedException();
        }

        public sealed override Pixel Cur2px(Pixel cur)
        {
            throw new NotImplementedException();
        }

        public sealed override Pixel Crd2px(Coord crd)
        {
            throw new NotImplementedException();
        }

        public sealed override Coord Px2crd(Pixel pel)
        {
            throw new NotImplementedException();
        }

        public sealed override double Deg2sc(double deg)
        {
            throw new NotImplementedException();
        }

        public sealed override void Access(bool use)
        {
            throw new NotImplementedException();
        }

        public sealed override void Zoomto(int num)
        {
            throw new NotImplementedException();
        }

        public sealed override void Moveto(Coord crd, bool swf)
        {
            throw new NotImplementedException();
        }

        public sealed override void Render(Maper drv)
        {
            throw new NotImplementedException();
        }

        public sealed override void Redraw(Msger msg)
        {
            throw new NotImplementedException();
        }

        public sealed override void Redraw()
        {
            throw new NotImplementedException();
        }
    }
}