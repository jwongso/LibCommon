using System;
using System.Linq;

namespace LibCommon
{
    public abstract class GeoArea
    {
        protected GeoArea() { }

        public abstract bool Contains(GeoCoordinate point);

        public abstract GeoRect Bbox();

        public abstract bool IsValid();
    }
}
