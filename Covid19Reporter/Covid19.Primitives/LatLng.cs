using System;
using System.Globalization;
using System.Linq;

namespace Covid19.Primitives
{
    public class LatLng
    {
        public LatLng(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }
        public double Longitude { get; }

        public static bool TryParse(string str, IFormatProvider formatProvider, out LatLng position)
        {
            try
            {
                string[] segs = str.Split(',').Select(x => x.Trim()).ToArray();
                double lat = double.Parse(segs[0], NumberStyles.Float, formatProvider);
                double lng = double.Parse(segs[1], NumberStyles.Float, formatProvider);
                position = new LatLng(lat, lng);
                return true;
            }
            catch
            {
                position = null;
                return false;
            }
        }
    }
}
