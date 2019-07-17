using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

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

            //1920*1080的图片大多数大于200KB，低于200KB的属于API返回错误分辨率的图片。
            //此API不是Bing官方API，而是第三方API，出错概率大约20%左右
            //当前分辨率壁纸文件的大致最小大小
            int ImgSmallestBytes = 200 * 1024;
            
            //初始API --- 随机获取图片
            string RawAPIAddress = "https://bing.ioliu.cn/v1/rand";
            //通过API获得图片的地址
            string ImgAddress = RawAPIAddress + 
                "?w=" + ScreenWidth.ToString() + "&h=" + ScreenHeight.ToString();

            //创建存储图片的文件夹
            System.Console.WriteLine("正在进入壁纸下载目录......");
            try
            {
                Directory.CreateDirectory(DownloadPath);
            }
            catch//这里偷了点懒，直接捕获所有异常
            {
                System.Console.WriteLine("无法进入下载目录，可能需要以管理员权限运行");
                System.Console.WriteLine("\n按任意键退出程序");
                System.Console.ReadKey();
                return;
            }
            //图片路径  图片文件名称实例：2019-07-16 16-31-22-875.jpg
            string ImgPath = DownloadPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fff") + ".jpg";

            DownLoadImg:

            //下载图片
            System.Console.WriteLine("正在下载图片......");
            try
            {
                new WebClient().DownloadFile(ImgAddress, ImgPath);
            }
            catch (WebException)
            {
                System.Console.WriteLine("网络连接错误！请在网络良好的环境下重试！");
                System.Console.WriteLine("\n按任意键退出程序");
                System.Console.ReadKey();
                return;
            }
            

            if(new FileInfo(ImgPath).Length < ImgSmallestBytes)
            {
                System.Console.WriteLine("Error:壁纸WebAPI出错，分发的图片URL不正确！");
                System.Console.WriteLine("Solution:尝试重新请求并下载......");
                goto DownLoadImg;
            }
            //Main(null);
            
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