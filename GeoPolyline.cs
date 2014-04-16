using System;
using System.Collections.Generic;
using System.Linq;

namespace LibCommon
{
    public class GeoPolyline : GeoArea
    {
        private readonly List<GeoCoordinate> mPoints = new List<GeoCoordinate>();

        public GeoPolyline()
        {
        }

        public GeoPolyline(IList<GeoCoordinate> points)
        {
            foreach (GeoCoordinate g in points)
            {
                mPoints.Add(g);
            }
        }

        public override bool Contains(GeoCoordinate point)
        {
            foreach (GeoCoordinate g in mPoints)
            {
                if (g == point)
                {
                    return true;
                }
            }

            return false;
        }

        public override GeoRect Bbox()
        {
            double minLat = GeoCoordinate.MaxLatitude;
            double minLng = GeoCoordinate.MaxLongitude;
            double maxLat = GeoCoordinate.MinLatitude;
            double maxLng = GeoCoordinate.MinLongitude;

            foreach (GeoCoordinate g in mPoints)
            {
                if (g.Latitude <= minLat)
                {
                    minLat = g.Latitude;
                }

                if (g.Longitude <= minLng)
                {
                    minLng = g.Longitude;
                }

                if (g.Latitude >= maxLat)
                {
                    maxLat = g.Latitude;
                }

                if (g.Longitude >= maxLng)
                {
                    maxLng = g.Longitude;
                }
            }

            return new GeoRect(new GeoCoordinate(maxLat, minLng), 
                new GeoCoordinate(minLat, maxLng));
        }

        public override bool IsValid()
        {
            foreach (GeoCoordinate g in mPoints)
            {
                if (!g.IsValid())
                {
                    return false;
                }
            }

            if (mPoints.Count < 2)
            {
                return false;
            }

            return true;
        }

        public virtual double LengthInKm()
        {
            double distance = 0;

            for (int i = 0; i + 1 < mPoints.Count; ++i)
            {
                distance += mPoints[i].CalculateAirDistanceInKm(mPoints[i + 1]);
            }

            return distance;
        }

        public IList<GeoCoordinate> Points()
        {
            return mPoints;
        }

        public GeoCoordinate Points(int index)
        {
            return mPoints[index];
        }
    }
}
