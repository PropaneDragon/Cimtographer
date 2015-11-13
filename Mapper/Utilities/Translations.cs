using Mapper.OSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mapper.Utilities
{
    static class Translations
    {
        private static Vector2 centreLatLon_;
        private static Vector2 scale_;
        private const double WGS84_a = 6378137.0; // Major semiaxis [m]
        private const double WGS84_b = 6356752.3; // Minor semiaxis [m]
        public const int GameSizeMetres = 18000;
        public const int GameSizeGameCoordinates = 1920 * 9;

        public static void SetUpBounds(OSMBounds bounds, double scale)
        {
            centreLatLon_ = new Vector2((float)(bounds.minlon + bounds.maxlon) / 2f, (float)(bounds.minlat + bounds.maxlat) / 2f);

            double lat = Deg2rad(centreLatLon_.y);
            double radius = WGS84EarthRadius(lat);
            double pradius = radius * Math.Cos(lat);

            double scaleX = scale * GameSizeGameCoordinates / Rad2deg(GameSizeMetres / pradius);
            double scaleY = scale * GameSizeGameCoordinates / Rad2deg(GameSizeMetres / radius);

            scale_ = new Vector2((float)scaleX, (float)scaleY);
        }

        public static void VectorToLonLat(Vector3 position, out decimal lon, out decimal lat)
        {
            lon = (decimal)((position.x / scale_.x) + centreLatLon_.x);
            lat = (decimal)((position.z / scale_.y) + centreLatLon_.y);
        }

        private static double WGS84EarthRadius(double lat)
        {
            var An = WGS84_a * WGS84_a * Math.Cos(lat);
            var Bn = WGS84_b * WGS84_b * Math.Sin(lat);
            var Ad = WGS84_a * Math.Cos(lat);
            var Bd = WGS84_b * Math.Sin(lat);
            return Math.Sqrt((An * An + Bn * Bn) / (Ad * Ad + Bd * Bd));
        }

        private static double Deg2rad(double degrees)
        {
            return Math.PI * degrees / 180.0;
        }

        private static double Rad2deg(double radians)
        {
            return 180.0 * radians / Math.PI;
        }
    }
}
