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

        private static string GetText(string getmotd_url)
        {
            WebClient client = new WebClient();
            byte[] buffer = client.DownloadData(getmotd_url);
            return Encoding.ASCII.GetString(buffer);
        }

        private static void ExtractRes()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + "\\meta")) Directory.CreateDirectory(Environment.CurrentDirectory + "\\meta");
            string script = Properties.Resources.updateScript;
            byte[] bytes = Encoding.ASCII.GetBytes(script);
            FileStream stream = new FileStream(Environment.CurrentDirectory + "\\meta\\update.bat", FileMode.OpenOrCreate);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Close();
        }

        public static void CheckUpdate()
        {
            
            if (!CheckNet()) return;
            if (version != GetText("http://get.cstu.gq/metadata/xinyuan/quicklauncher/ver.txt").Trim())
            {
                ExtractRes();
                MessageBox.Show("This launcher is outdated, confirm to update it.");
#if DEBUG == false
                HttpDownload(GetText("http://get.cstu.gq/metadata/xinyuan/quicklauncher/update.txt"), Environment.CurrentDirectory + "\\Launcher.temp");
                checkver_fileexist:
                if (File.Exists(Environment.CurrentDirectory + "\\temp"))
                {
                    Thread.Sleep(50);
                    goto checkver_fileexist;
                }
#endif
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = "/c @ECHO OFF&&TITLE Launcher Update Console&&meta\\update.bat";
                p.Start();
#if DEBUG == false
                ShutOff();
#endif
            }
        }

        private static void ShutOff()
        {
            Environment.Exit(0);
        }
    }
}
