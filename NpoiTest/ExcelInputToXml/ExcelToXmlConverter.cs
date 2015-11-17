﻿using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NpoiTest.ExcelInputToXml
{
    class ExcelToXmlConverter
    {
        /// <summary>
        /// 坐标的默认偏移量
        /// </summary>
        private const double OFFSET = 0.00002;

        /// <summary>
        /// 保存从Excel中解析出来的数据
        /// </summary>
        private List<Data> dataList = new List<Data>();

        /// <summary>
        /// 输出路径
        /// </summary>
        private string filePath;


        /// <summary>
        /// 输入一个文件路径的构造方法
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        public ExcelToXmlConverter(string filePath)
        {
            this.filePath = filePath;

            //解析文件
            parseExcel();
            //检查空格
            checkspace();
        }

        /// <summary>
        /// 解析Excel文件 填充数据
        /// </summary>
        private void parseExcel()
        {
            XSSFWorkbook wk = null;
            using (FileStream fs = File.Open(filePath, FileMode.Open,
                FileAccess.Read, FileShare.ReadWrite))
            {
                //把xls文件读入workbook变量里，之后就可以关闭了
                wk = new XSSFWorkbook(fs);
                fs.Close();
            }
            //获取第一个表格
            ISheet sheet = wk.GetSheetAt(0);
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                Data data = new Data();
                data.Devicetype = row.GetCell(1).ToString();
                data.Kmmark = row.GetCell(2).ToString();
                data.Lateral = row.GetCell(3).ToString();
                data.Longitude = row.GetCell(4).ToString();
                data.Latitude = row.GetCell(5).ToString();
                dataList.Add(data);
            }
        }

        /// <summary>
        /// 检查空格
        /// </summary>
        /// <returns></returns>
        private void checkspace()
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].Longitude == "" || dataList[i].Latitude == "")
                {
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (!(dataList[k].Longitude == "" || dataList[k].Latitude == ""))
                        {
                            dataList[i].Longitude = (Convert.ToDouble(dataList[k].Longitude) + OFFSET).ToString();
                            dataList[i].Latitude = (Convert.ToDouble(dataList[k].Latitude) + OFFSET).ToString();
                            break;
                        }
                    }
                    if (dataList[i].Longitude == "" || dataList[i].Latitude == "")
                    {
                        int space = 1;
                        for (int k = i + 1; k < dataList.Count; k++)
                        {
                            if (!(dataList[k].Longitude == "" || dataList[k].Latitude == ""))
                            {
                                dataList[i].Longitude =
                                    (Convert.ToDouble(dataList[k].Longitude) - 0.00002 * space).ToString();
                                dataList[i].Latitude =
                                    (Convert.ToDouble(dataList[k].Latitude) - 0.00002 * space).ToString();
                                break;
                            }
                            else
                            {
                                space++;
                            }
                        }
                        if (dataList[i].Longitude == "" || dataList[i].Latitude == "")
                        {
                            Console.Out.Write("数据错误");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 将解析出的数据保存在xml文件中
        /// </summary>
        /// <param name="outputPath">输出路径</param>
        /// <param name="dataList">待输出的路径</param>
        public void ExportXmlFile(string outputPath)
        {
            XmlDocument save = new XmlDocument();
            XmlDeclaration decl = save.CreateXmlDeclaration("1.0", "utf-8", "");
            XmlElement Xmldata = save.CreateElement("data");
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].Devicetype != "")
                {
                    XmlElement devicetype = save.CreateElement("devicetype");
                    XmlElement kmmark = save.CreateElement("kmmark");
                    XmlElement lateral = save.CreateElement("lateral");
                    XmlElement longitude = save.CreateElement("longtitude");
                    XmlElement latitude = save.CreateElement("latitude");
                    devicetype.InnerText = dataList[i].Devicetype;
                    kmmark.InnerText = dataList[i].Kmmark;
                    lateral.InnerText = dataList[i].Lateral;
                    if (!(dataList[i].Longitude == "" || dataList[i].Latitude == ""))
                    {
                        longitude.InnerText = dataList[i].Longitude;
                        latitude.InnerText = dataList[i].Latitude;
                    }

                    devicetype.AppendChild(kmmark);
                    devicetype.AppendChild(lateral);
                    devicetype.AppendChild(longitude);
                    devicetype.AppendChild(latitude);

                    Xmldata.AppendChild(devicetype);
                }
            }
            save.AppendChild(decl);
            save.AppendChild(Xmldata);
            //输出文件
            //save.Save(outputPath);
            save.Save(outputPath);
        }
    }
}