using System;
using System.Linq;
using System.Runtime.Serialization;

namespace LibCommon
{
    [DataContract]
    public class GeoCoordinate
    {
        public const double MinLatitude = -90.0;
        public const double MaxLatitude = 90.0;
        public const double MinLongitude = -180.0;
        public const double MaxLongitude = 180.0;
        const double AngleEps = 1.7966e-7;
        const double AltitudeEps = 1e-2;

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }

        [DataMember]
        public float Altitude { get; set; }

        public GeoCoordinate(GeoCoordinate other)
        {
            Latitude = other.Latitude;
            Longitude = other.Longitude;
            Altitude = other.Altitude;
        }

        public GeoCoordinate(double latitude, double longitude, float altitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }

        public GeoCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = float.NaN;
        }

        public GeoCoordinate()
        {
            Latitude = double.NaN;
            Longitude = double.NaN;
            Altitude = float.NaN;
        }

        public bool IsValid()
        {
            if (!double.IsNaN(Latitude) && !double.IsNaN(Longitude))
            {
                if ((Latitude >= MinLatitude && Latitude <= MaxLatitude) &&
                    (Longitude >= MinLongitude && Longitude <= MaxLongitude))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter cannot be cast to SSAJob return false:
            GeoCoordinate p = obj as GeoCoordinate;
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match 
            // (2 cm distance inbetween will be considered as the same)
            return base.Equals(obj) &&
                Math.Abs(this.Latitude - p.Latitude) < AngleEps &&
                Math.Abs(this.Longitude - p.Longitude) < AngleEps &&
                Math.Abs(this.Altitude - p.Altitude) < AltitudeEps;
        }

        public bool Equals(GeoCoordinate p)
        {
            // Return true if the fields match:
            return base.Equals(p) &&
                Math.Abs(this.Latitude - p.Latitude) < AngleEps &&
                Math.Abs(this.Longitude - p.Longitude) < AngleEps &&
                Math.Abs(this.Altitude - p.Altitude) < AltitudeEps;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (int)(Math.Abs(Latitude) + Math.Abs(Longitude));
        }

        public static bool operator ==(GeoCoordinate a, GeoCoordinate b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match 
            // (2 cm distance inbetween will be considered as the same)
            return Math.Abs(a.Latitude - b.Latitude) < AngleEps &&
                Math.Abs(a.Longitude - b.Longitude) < AngleEps &&
                Math.Abs(a.Altitude - b.Altitude) < AltitudeEps;
        }

        public static bool operator !=(GeoCoordinate a, GeoCoordinate b)
        {
            return !(a == b);
        }
    }
}
