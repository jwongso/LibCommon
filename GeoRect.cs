using System;
using System.Linq;

namespace LibCommon
{
    public class GeoRect : GeoArea
    {
        private GeoCoordinate mTopLeft = new GeoCoordinate();
        private GeoCoordinate mBottomRight = new GeoCoordinate();

        public GeoRect()
        {
        }

        public GeoRect(GeoCoordinate topLeft, GeoCoordinate bottomRight)
        {
            mTopLeft = topLeft;
            mBottomRight = bottomRight;
        }

        public GeoRect(double west, double east, double south, double north)
        {
            mTopLeft.Latitude = north;
            mTopLeft.Longitude = west;
            mBottomRight.Latitude = south;
            mBottomRight.Longitude = east;
        }

        public GeoRect(GeoCoordinate center, double distanceInKm)
        {
            if (distanceInKm >= 0)
            {
                double radDist = distanceInKm / 6378.1370D;
                double minLat = center.Latitude.ToRad() - radDist;
                double maxLat = center.Latitude.ToRad() + radDist;

                double minLon, maxLon;
                if (minLat > GeoCoordinate.MinLatitude && maxLat < GeoCoordinate.MaxLatitude)
                {
                    double deltaLon = Math.Asin(Math.Sin(radDist) /
                    Math.Cos(center.Latitude.ToRad()));
                    minLon = center.Longitude.ToRad() - deltaLon;

                    if (minLon < GeoCoordinate.MinLongitude)
                    {
                        minLon += 2d * Math.PI;
                    }

                    maxLon = center.Longitude.ToRad() + deltaLon;
                    if (maxLon > GeoCoordinate.MaxLongitude)
                    {
                        maxLon -= 2d * Math.PI;
                    }
                }
                else
                {
                    // a pole is within the distance
                    minLat = Math.Max(minLat, GeoCoordinate.MinLatitude);
                    maxLat = Math.Min(maxLat, GeoCoordinate.MaxLatitude);
                    minLon = GeoCoordinate.MinLongitude;
                    maxLon = GeoCoordinate.MaxLongitude;
                }

                mTopLeft = new GeoCoordinate(minLat, minLon);
                mBottomRight = new GeoCoordinate(maxLat, maxLon);
            }
        }

        public GeoCoordinate TopLeft
        {
            get
            {
                return mTopLeft;
            }

            set
            {
                mTopLeft = value;
            }
        }

        public GeoCoordinate BottomRight
        {
            get
            {
                return mBottomRight;
            }

            set
            {
                mBottomRight = value;
            }
        }

        public override bool Contains(GeoCoordinate p)
        {
            if (p.Latitude <= mTopLeft.Latitude && 
                p.Latitude >= mBottomRight.Latitude)
            {
                double left = mTopLeft.Longitude;
                double right = mBottomRight.Longitude;

                if (left <= right)
                {
                    return p.Longitude >= left
                        && p.Longitude <= right;
                }

                // This rect covers area on the back of the globe (180 meridian)
                return p.Longitude >= left
                       || p.Longitude <= right;
            }

            return false;
        }

        public override GeoRect Bbox()
        {
            return this;
        }

        public override bool IsValid()
        {
            return mTopLeft.IsValid() && mBottomRight.IsValid() && 
                mTopLeft.Latitude >= mBottomRight.Latitude;
        }

        public GeoCoordinate Center()
        {
            double lat = (mBottomRight.Latitude + mTopLeft.Latitude) * 0.5;
            float alt = (mBottomRight.Altitude + mTopLeft.Altitude) * 0.5f;

            double longRight = mBottomRight.Longitude;
            double longLeft = mTopLeft.Longitude;

            double lon;

            if (longLeft > longRight)
            {
                // Wrap around -180
                double longRightAdjusted = longRight + 360.0;
                lon = (longRightAdjusted + longLeft) * 0.5;

                if (lon >= 180.0)
                {
                    lon -= 360.0;
                }
            }
            else
            {
                lon = (longRight + longLeft) * 0.5;
            }

            return new GeoCoordinate(lat, lon, alt);
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter cannot be cast to SSAJob return false:
            GeoRect p = obj as GeoRect;
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match 
            // (2 cm distance inbetween will be considered as the same)
            return base.Equals(obj) &&
                this.mTopLeft == p.mTopLeft &&
                this.mBottomRight == p.mBottomRight;
        }

        public bool Equals(GeoRect p)
        {
            // Return true if the fields match:
            return base.Equals(p) &&
                this.mTopLeft == p.mTopLeft &&
                this.mBottomRight == p.mBottomRight;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (int)(Math.Abs(mTopLeft.Latitude) + Math.Abs(mTopLeft.Longitude));
        }

        public static bool operator ==(GeoRect a, GeoRect b)
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
            return a.mTopLeft == b.mTopLeft &&
                a.mBottomRight == b.mBottomRight;
        }

        public static bool operator !=(GeoRect a, GeoRect b)
        {
            return !(a == b);
        }
    }
}
