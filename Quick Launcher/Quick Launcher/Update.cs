﻿using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace Quick_Launcher
{
    class Updater
    {
        static string version = "1.0.0.0";
        /// <summary>
        /// http下载文件
        /// </summary>
        private static bool HttpDownload(string url, string path)
        {
            string tempPath = Path.GetDirectoryName(path) + @"\temp";
            Directory.CreateDirectory(tempPath);  //创建临时文件目录
            string tempFile = tempPath + @"\" + Path.GetFileName(path) + ".temp";
            if (File.Exists(tempFile)) //伪FileMode.Truncate
            {
                File.Delete(tempFile);
            }
            try
            {
                using (FileStream tempWritter = new FileStream(tempFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    tempWritter.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                responseStream.Close();
                    File.Move(tempFile, path);
                return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool CheckFile(string URL)
        {
            HttpWebRequest request = null;
            HttpWebResponse resquest = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(URL);
                resquest = (HttpWebResponse)request.GetResponse();
                return resquest.ContentLength != 0;
            }
            catch (Exception)
            {
                //文件不存在
                return false;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (resquest != null)
                {
                    resquest.Close();
                }
            }
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(int Description, int ReservedValue);
        private static bool CheckNet()
        {
            return InternetGetConnectedState(0, 0);
        }

        private static string getText(string getmotd_url)
        {
            WebClient client = new WebClient();
            byte[] buffer = client.DownloadData(getmotd_url);
            return System.Text.Encoding.ASCII.GetString(buffer);
        }

        public static void CheckUpdate()
        {
            if (!CheckNet()) new Exception("无法连接服务器！");
            if (version != getText("http://get.cstu.gq/metadata/xinyuan/quicklauncher/ver.txt").Trim())
            {
                MessageBox.Show("This programm is outdated, confirm to update it.");
                HttpDownload(getText("http://get.cstu.gq/metadata/xinyuan/quicklauncher/update.txt"), Environment.CurrentDirectory + "\\Launcher.temp");
                while(File.Exists(Environment.CurrentDirectory + "\\temp"))
                {
                    Thread.Sleep(50);
                }
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/c @ECHO OFF&&TITLE Launcher Update Console&&meta\\update.bat";
                process.Start();
                ShutOff();
            }
        }

        private static void ShutOff()
        {
            Environment.Exit(0);
        }
    }
}