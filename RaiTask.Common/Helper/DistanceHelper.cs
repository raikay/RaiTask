﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RaiTask.Infrastructure.Helper
{

    public class DistanceHelper
    {
        /// <summary>
        /// 地球半径(单位：米)
        /// </summary>
        public const double EarthRadius = 6378137.0;
        /// <summary>
        /// 
        /// </summary>
        private const double PI = 3.141592653589793;
        private static double getRad(double lat)
        {
            return lat * PI / 180.0;
        }
        /// <summary>
        /// 计算地球两点距离(单位:米)
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lng1"></param>
        /// <param name="lat2"></param>
        /// <param name="lng2"></param>
        /// <returns></returns>
        public static double GetGreatCircleDistance(double lat1, double lng1, double lat2, double lng2)
        {
            var radLat1 = getRad(lat1);
            var radLat2 = getRad(lat2);
            var a = radLat1 - radLat2;
            var b = getRad(lng1) - getRad(lng2);

            var s = 2 * Math.Asin(Math.Sqrt((Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))));
            return Math.Round(s * EarthRadius * 10000) / 10000.0;
        }
        /// <summary>
        /// 计算两点位置的距离，返回两点的距离，单位：公里或千米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="lat1">第一点纬度</param>
        /// <param name="lng1">第一点经度</param>
        /// <param name="lat2">第二点纬度</param>
        /// <param name="lng2">第二点经度</param>
        /// <returns>返回两点的距离，单位：公里或千米</returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            //地球半径，单位米
            double EARTH_RADIUS = 6378137;
            double radLat1 = getRad(lat1);
            double radLng1 = getRad(lng1);
            double radLat2 = getRad(lat2);
            double radLng2 = getRad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result / 1000;
        }
        /// <summary>
        /// 根据一个给定经纬度的点和距离，进行附近地点查询
        /// </summary>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="distance">距离（单位：公里或千米）</param>
        /// <returns>返回一个范围的4个点，最小纬度和纬度，最大经度和纬度</returns>
        public static PositionModel FindNeighPosition(double longitude, double latitude, double distance)
        {
            //先计算查询点的经纬度范围  
            double r = 6378.137;//地球半径千米  
            double dis = distance;//千米距离    
            double dlng = 2 * Math.Asin(Math.Sin(dis / (2 * r)) / Math.Cos(latitude * Math.PI / 180));
            dlng = dlng * 180 / Math.PI;//角度转为弧度  
            double dlat = dis / r;
            dlat = dlat * 180 / Math.PI;
            double minlat = latitude - dlat;
            double maxlat = latitude + dlat;
            double minlng = longitude - dlng;
            double maxlng = longitude + dlng;
            return new PositionModel
            {
                MinLat = minlat,
                MaxLat = maxlat,
                MinLng = minlng,
                MaxLng = maxlng
            };
        }

    }
    public class PositionModel
    {
        public double MinLat { get; set; }
        public double MaxLat { get; set; }
        public double MinLng { get; set; }
        public double MaxLng { get; set; }
    }


}
