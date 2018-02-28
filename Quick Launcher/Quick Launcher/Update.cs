using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;

namespace Quick_Launcher
{
    class Updater
    {
        static string version = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location).ProductVersion;
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
        private static extern bool InternetGetConnectedState(int Description, int ReservedValue);
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

        private static void extractRes()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + "\\meta")) Directory.CreateDirectory(Environment.CurrentDirectory + "\\meta");
            string str = Properties.Resources.updateScript;
            byte[] Save = Encoding.ASCII.GetBytes(str);
            FileStream fsObj = new FileStream(Environment.CurrentDirectory + "\\meta\\update.bat", FileMode.OpenOrCreate);
            fsObj.Write(Save, 0, Save.Length);
            fsObj.Flush();
            fsObj.Close();
        }

        public static void CheckUpdate()
        {/*
            if (!CheckNet()) new Exception("无法连接服务器！");
            if (version == getText("https://get.cstu.gq/metadata/xinyuan/quicklauncher/ver.txt").Trim())
            {
                MessageBox.Show("This program is outdated, confirm to update it.");
                HttpDownload(getText("https://get.cstu.gq/metadata/xinyuan/quicklauncher/update.txt"), Environment.CurrentDirectory + "\\Launcher.temp");
                while(File.Exists(Environment.CurrentDirectory + "\\temp\\Launcher.temp.temp"))
                {
                    Thread.Sleep(50);
                }
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/c @ECHO OFF&&TITLE Launcher Update Console&&meta\\update.bat";
                process.Start();
                ShutOff();
            }*/

            if (!CheckNet()) return;
            if (version != getText("http://get.cstu.gq/metadata/xinyuan/quicklauncher/ver.txt").Trim())
            {
                extractRes();
                MessageBox.Show("This launcher is outdated, confirm to update it.");
                HttpDownload(getText("http://get.cstu.gq/metadata/xinyuan/quicklauncher/update.txt"), Environment.CurrentDirectory + "\\Launcher.temp");
                checkver_fileexist:
                if (File.Exists(Environment.CurrentDirectory + "\\temp"))
                {
                    Thread.Sleep(50);
                    goto checkver_fileexist;
                }
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = "/c @ECHO OFF&&TITLE Launcher Update Console&&meta\\update.bat";
                p.Start();//启动程序
                ShutOff();
            }
        }

        private static void ShutOff()
        {
            Environment.Exit(0);
        }
    }
}
