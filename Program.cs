using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace ChangeWallpapersFromWeb
{
    class Program
    {
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

        static void Main(string[] args)
        {
            //设置分辨率
            int ScreenWidth = 1920;
            int ScreenHeight = 1080;
            //设置存储位置（绝对路径）
            string DownloadPath = @"E:\BingImg";
            
            //初始API --- 随机获取图片
            string RawAPIAddress = "https://bing.ioliu.cn/v1/rand";
            //通过API获得图片的地址
            string ImgAddress = RawAPIAddress + 
                "?w=" + ScreenWidth.ToString() + "&h=" + ScreenHeight.ToString();

            //创建存储图片的文件夹
            System.Console.WriteLine("正在进入壁纸下载目录......");
            Directory.CreateDirectory(DownloadPath);
            //图片路径  图片文件名称实例：2019-07-16 16-31-22-875.jpg
            string ImgPath = DownloadPath + "//" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fff") + ".jpg";

            //下载图片
            System.Console.WriteLine("正在下载图片......");
            new WebClient().DownloadFile(ImgAddress, ImgPath);
            
            //return;
            System.Console.WriteLine("正在设置壁纸......");
            if (!SystemParametersInfo(20, 0, ImgPath, 1))
            {
                System.Console.WriteLine("设置壁纸失败！请重试！");
                System.Console.WriteLine("\n按任意键退出程序");
                System.Console.ReadKey();
            }
            else
                System.Console.WriteLine("设置壁纸成功！即将自动关闭程序......");

        }
    }
}