using System;
using System.Collections.Generic;
using System.Linq;

namespace LibCommon
{
    public class GeoPolygon : GeoPolyline
    {
        public GeoPolygon()
        {
        }

        public GeoPolygon(IList<GeoCoordinate> points)
        {
            foreach (GeoCoordinate g in points)
            {
                Points().Add(g);
            }
        }

        public GeoPolygon(GeoRect geoRect)
        {
            GeoCoordinate topLeft = new GeoCoordinate(geoRect.TopLeft);
            GeoCoordinate bottomRight = new GeoCoordinate(geoRect.BottomRight);

            Points().Add(topLeft);
            Points().Add(new GeoCoordinate(bottomRight.Latitude, topLeft.Longitude));
            Points().Add(bottomRight);
            Points().Add(new GeoCoordinate(topLeft.Latitude, bottomRight.Longitude));
        }

        public override bool Contains(GeoCoordinate point)
        {
            if (!IsValid())
            {
                return false;
            }

            int i, j;
            bool inside = false;

            for (i = 0, j = Points().Count - 1; i < Points().Count; j = i++)
            {
                if ((((Points(i).Latitude <= point.Latitude) && (point.Latitude < Points(j).Latitude)) ||
                    ((Points(j).Latitude <= point.Latitude) && (point.Latitude < Points(i).Latitude))) &&
                    (point.Longitude < (Points(j).Longitude - Points(i).Longitude) * 
                    (point.Latitude - Points(i).Latitude) / 
                    (Points(j).Latitude - Points(i).Latitude) + Points(i).Longitude))
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        public override bool IsValid()
        {
            if (Points().Count < 3)
            {
                return false;
            }

            return base.IsValid();
        }

        public override double LengthInKm()
        {
            double length = 0;

            if (IsValid())
            {
                length = base.LengthInKm();

                length += Points(0).CalculateAirDistanceInKm(Points(Points().Count - 1));
            }

            return length;
        }
    }
}
