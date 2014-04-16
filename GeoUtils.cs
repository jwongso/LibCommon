using System;
using System.Linq;

namespace LibCommon
{
    public static class GeoUtils
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static double ToRad(this double val)
        {
            return Deg2Rad(val);
        }

        public static double ToDeg(this double val)
        {
            return Rad2Deg(val);
        }

        public static double Deg2Rad(double degrees)
        {
            return Math.PI * degrees / 180.0;
        }

        public static double Rad2Deg(double radians)
        {
            return 180.0 * radians / Math.PI;
        }

        public static double ToBearing(double radians)
        {
            // convert radians to degrees (as bearing: 0...360)
            return (Rad2Deg(radians) + 360) % 360;
        }

        public static double CalculateAirDistanceInKm(this GeoCoordinate coord1, GeoCoordinate coord2)
        {
            const double QEarthRadius = 6378.1370D;
            const double D2R = (Math.PI / 180D);

            double dLng = (coord2.Longitude - coord1.Longitude) * D2R;
            double dLat = (coord2.Latitude - coord1.Latitude) * D2R;
            double a = Math.Pow(Math.Sin(dLat / 2D), 2D) +
                Math.Cos(coord1.Latitude * D2R) * Math.Cos(coord2.Latitude * D2R) *
                Math.Pow(Math.Sin(dLng / 2D), 2D);
            double c = 2D * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1D - a));

            return QEarthRadius * c;
        }

        public static double CalculateBearing(this GeoCoordinate coord1, GeoCoordinate coord2)
        {
            var dLon = Deg2Rad(coord2.Longitude - coord1.Longitude);
            var dPhi = Math.Log(
                Math.Tan(Deg2Rad(coord2.Latitude) / 2 + Math.PI / 4) /
                Math.Tan(Deg2Rad(coord1.Latitude) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return ToBearing(Math.Atan2(dLon, dPhi));
        }
    }
}
