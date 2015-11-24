﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiTest.Model.Database
{
    /// <summary>
    /// 单个Marker需要获取的数据
    /// 也就是excel中一行需要有的数据
    /// </summary>
    internal class DbBean
    {
        //工程名
        public string PrjName { get; set; }
        //经度
        public string Longitude { get; set; }
        //纬度
        public string Latitude { get; set; }
        //设备类型
        public string DeviceType { get; set; }
        //公里标
        public string KilometerMark { get; set; }
        //侧向
        public string SideDirection { get; set; }
        //照片路径
        public string PhotoPathName { get; set; }

        public DbBean()
        {
            PrjName = "hahaha";
            Longitude = "a'regm";
            Latitude = "mlerkgnr";
        }

        /// <summary>
        /// 传入三个关键数据的构造方法
        /// </summary>
        /// <param name="prjName"></param>
        /// <param name="kilometerMark"></param>
        /// <param name="sideDirection"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="deviceType"></param>
        /// <param name="photoPathName"></param>
        public DbBean(string prjName, string kilometerMark, string sideDirection, double longitude, double latitude, string deviceType, string photoPathName)
        {
            PrjName = prjName;
            Longitude = longitude + "";
            Latitude = latitude + "";
            DeviceType = deviceType;
            KilometerMark = kilometerMark;
            SideDirection = sideDirection;
            PhotoPathName = photoPathName;
        }
    }
}