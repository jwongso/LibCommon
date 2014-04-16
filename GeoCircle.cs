using System;
using System.Linq;

namespace LibCommon
{
    public class GeoCircle : GeoArea
    {
        private GeoCoordinate mCenter = new GeoCoordinate();
        private double mRadiusInMeter = -1.0;

        public GeoCircle()
        {
        }

        public GeoCircle(GeoCoordinate geoCoordinate, double radiusInMeter)
        {
            mCenter = geoCoordinate;
            mRadiusInMeter = radiusInMeter;
        }

        public GeoCoordinate Center
        {
            get
            {
                return mCenter;
            }

            set
            {
                mCenter = value;
            }
        }

        public double RadiusInMeter
        {
            get
            {
                return mRadiusInMeter;
            }

            set
            {
                mRadiusInMeter = value;
            }
        }

        public override bool Contains(GeoCoordinate p)
        {
            double distanceKm = mCenter.CalculateAirDistanceInKm(p);
            double distanceInMeter = distanceKm * 1000;

            return (distanceInMeter < mRadiusInMeter);
        }

        public override GeoRect Bbox()
        {
            const double QEarthRadius = 6378.1370D;
            const double TopLeftDeg = 315.0;
            const double BottomRightDeg = 135.0;

            double latRad = mCenter.Latitude.ToRad();
            double lngRad = mCenter.Longitude.ToRad();
            double distanceKm = mRadiusInMeter / 1000;

            double tlLat = Math.Asin(Math.Sin(latRad) * Math.Cos(distanceKm/QEarthRadius) +
                                   Math.Cos(latRad) * Math.Sin(distanceKm / QEarthRadius) * 
                                   Math.Cos(GeoUtils.Deg2Rad(TopLeftDeg.ToRad())));
            double tlLng = lngRad + Math.Atan2(
                Math.Sin(TopLeftDeg.ToRad()) * Math.Sin(distanceKm / QEarthRadius) * Math.Cos(mCenter.Latitude), 
                Math.Cos(distanceKm) - Math.Sin(latRad) * Math.Sin(tlLat));

            tlLng = (tlLng + 3 * Math.PI) % (2 * Math.PI) - Math.PI;

            double brLat = Math.Asin(Math.Sin(latRad) * Math.Cos(distanceKm / QEarthRadius) +
                                   Math.Cos(latRad) * Math.Sin(distanceKm / QEarthRadius) *
                                   Math.Cos(GeoUtils.Deg2Rad(BottomRightDeg.ToRad())));
            double brLng = lngRad + Math.Atan2(
                Math.Sin(BottomRightDeg.ToRad()) * Math.Sin(distanceKm / QEarthRadius) * Math.Cos(mCenter.Latitude),
                Math.Cos(distanceKm) - Math.Sin(latRad) * Math.Sin(brLat));

            brLng = (brLng + 3 * Math.PI) % (2 * Math.PI) - Math.PI;

            return new GeoRect(
                new GeoCoordinate(tlLat.ToDeg(), tlLng.ToDeg()), 
                new GeoCoordinate(brLat.ToDeg(), brLng.ToDeg()));
        }

        public override bool IsValid()
        {
            return mCenter.IsValid() && mRadiusInMeter >= 0;
        }
    }
}
