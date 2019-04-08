using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Common
{
    /// <summary>
    /// 百度坐标转换
    /// </summary>
    public class BDLocaltion
    {
        /*
        我提供的是JAVA代码，简单注释一下：
        pi: 圆周率。
        a: 卫星椭球坐标投影到平面地图坐标系的投影因子。
        ee: 椭球的偏心率。
        x_pi: 圆周率转换量。
         */
        /// <summary>
        /// 圆周率
        /// </summary>
        static double pi = 3.14159265358979324;
        /// <summary>
        /// 卫星椭球坐标投影到平面地图坐标系的投影因子
        /// </summary>
        static double a = 6378245.0;
        /// <summary>
        /// 椭球的偏心率
        /// </summary>
        static double ee = 0.00669342162296594323;
        /// <summary>
        /// 圆周率转换量
        /// </summary>
        static double x_pi = pi * 3000.0 / 180.0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static double[] wgs2bd(double lat, double lon)
        {
            double[] wgs2gcjval = wgs2gcj(lat, lon);
            double[] gcj2bdval = gcj2bd(wgs2gcjval[0], wgs2gcjval[1]);
            return gcj2bdval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static double[] gcj2bd(double lat, double lon)
        {
            double x = lon, y = lat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
            double bd_lon = z * Math.Cos(theta) + 0.0065;
            double bd_lat = z * Math.Sin(theta) + 0.006;
            return new double[] { bd_lat, bd_lon };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static double[] bd2gcj(double lat, double lon)
        {
            double x = lon - 0.0065, y = lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            double gg_lon = z * Math.Cos(theta);
            double gg_lat = z * Math.Sin(theta);
            return new double[] { gg_lat, gg_lon };
        }

        private static double[] wgs2gcj(double lat, double lon)
        {
            double dLat = transformLat(lon - 105.0, lat - 35.0);
            double dLon = transformLon(lon - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            double mgLat = lat + dLat;
            double mgLon = lon + dLon;
            double[] loc = { mgLat, mgLon };
            return loc;
        }

        private static double transformLat(double lat, double lon)
        {
            double ret = -100.0 + 2.0 * lat + 3.0 * lon + 0.2 * lon * lon + 0.1 * lat * lon + 0.2 * Math.Sqrt(Math.Abs(lat));
            ret += (20.0 * Math.Sin(6.0 * lat * pi) + 20.0 * Math.Sin(2.0 * lat * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(lon * pi) + 40.0 * Math.Sin(lon / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(lon / 12.0 * pi) + 320 * Math.Sin(lon * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        private static double transformLon(double lat, double lon)
        {
            double ret = 300.0 + lat + 2.0 * lon + 0.1 * lat * lat + 0.1 * lat * lon + 0.1 * Math.Sqrt(Math.Abs(lat));
            ret += (20.0 * Math.Sin(6.0 * lat * pi) + 20.0 * Math.Sin(2.0 * lat * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(lat * pi) + 40.0 * Math.Sin(lat / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(lat / 12.0 * pi) + 300.0 * Math.Sin(lat / 30.0 * pi)) * 2.0 / 3.0;
            return ret;
        }
        /*
        static double pi = 3.14159265358979324;
        static double a = 6378245.0;
        static double ee = 0.00669342162296594323;
        public final static double x_pi = 3.14159265358979324 * 3000.0 / 180.0;

        public static double[] wgs2bd(double lat, double lon) {
               double[] wgs2gcj = wgs2gcj(lat, lon);
               double[] gcj2bd = gcj2bd(wgs2gcj[0], wgs2gcj[1]);
               return gcj2bd;
        }

        public static double[] gcj2bd(double lat, double lon) {
               double x = lon, y = lat;
               double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
               double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
               double bd_lon = z * Math.Cos(theta) + 0.0065;
               double bd_lat = z * Math.Sin(theta) + 0.006;
               return new double[] { bd_lat, bd_lon };
        }

        public static double[] bd2gcj(double lat, double lon) {
               double x = lon - 0.0065, y = lat - 0.006;
               double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
               double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
               double gg_lon = z * Math.Cos(theta);
               double gg_lat = z * Math.Sin(theta);
               return new double[] { gg_lat, gg_lon };
        }

        public static double[] wgs2gcj(double lat, double lon) {
               double dLat = transformLat(lon - 105.0, lat - 35.0);
               double dLon = transformLon(lon - 105.0, lat - 35.0);
               double radLat = lat / 180.0 * pi;
               double magic = Math.Sin(radLat);
               magic = 1 - ee * magic * magic;
               double sqrtMagic = Math.Sqrt(magic);
               dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
               dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
               double mgLat = lat + dLat;
               double mgLon = lon + dLon;
               double[] loc = { mgLat, mgLon };
               return loc;
        }

        private static double transformLat(double lat, double lon) {
               double ret = -100.0 + 2.0 * lat + 3.0 * lon + 0.2 * lon * lon + 0.1 * lat * lon + 0.2 * Math.Sqrt(Math.abs(lat));
               ret += (20.0 * Math.Sin(6.0 * lat * pi) + 20.0 * Math.Sin(2.0 * lat * pi)) * 2.0 / 3.0;
               ret += (20.0 * Math.Sin(lon * pi) + 40.0 * Math.Sin(lon / 3.0 * pi)) * 2.0 / 3.0;
               ret += (160.0 * Math.Sin(lon / 12.0 * pi) + 320 * Math.Sin(lon * pi  / 30.0)) * 2.0 / 3.0;
               return ret;
        }

        private static double transformLon(double lat, double lon) {
               double ret = 300.0 + lat + 2.0 * lon + 0.1 * lat * lat + 0.1 * lat * lon + 0.1 * Math.Sqrt(Math.abs(lat));
               ret += (20.0 * Math.Sin(6.0 * lat * pi) + 20.0 * Math.Sin(2.0 * lat * pi)) * 2.0 / 3.0;
               ret += (20.0 * Math.Sin(lat * pi) + 40.0 * Math.Sin(lat / 3.0 * pi)) * 2.0 / 3.0;
               ret += (150.0 * Math.Sin(lat / 12.0 * pi) + 300.0 * Math.Sin(lat / 30.0 * pi)) * 2.0 / 3.0;
               return ret;
        }


        
        我提供的是JAVA代码，简单注释一下：
        pi: 圆周率。
        a: 卫星椭球坐标投影到平面地图坐标系的投影因子。
        ee: 椭球的偏心率。
        x_pi: 圆周率转换量。
        transformLat(lat, lon): 转换方法，比较复杂，不必深究了。输入：横纵坐标，输出：转换后的横坐标。
        transformLon(lat, lon): 转换方法，同样复杂，自行脑补吧。输入：横纵坐标，输出：转换后的纵坐标。
        wgs2gcj(lat, lon): WGS坐标转换为GCJ坐标。
        gcj2bd(lat, lon): GCJ坐标转换为百度坐标。

                */
    }
}
