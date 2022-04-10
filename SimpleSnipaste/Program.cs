using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;

namespace SimpleSnipaste
{
    internal class Program
    {
        private static CopyConfig copyConfig = new CopyConfig();

        private static string CutDirectory = "D:\\ScreenShot";

        private static void WriteIssueDetatil(string dir)
        {
            Console.WriteLine("请输入现象描述:");
            string value = Console.ReadLine();
            using (var streamWriter = new StreamWriter(dir + "\\IssueDetail.txt"))
            {
                streamWriter.WriteLine(value);
            }
        }

        private static void SaveScreen(string fileName)
        {
            CaptureScreenshot.HideConsole();
            Bitmap screen = CaptureScreenshot.GetScreen();
            screen.Save(fileName, ImageFormat.Png);
        }

        private static void CopyFile(string dstFolder)
        {
            foreach (string file in copyConfig.File.files)
            {
                if (!File.Exists(file))
                {
                    continue;
                }
                string directoryName = Path.GetDirectoryName(file);
                string[] array = directoryName.Split(':');
                if (array.Length == 2)
                {
                    string path = dstFolder + "\\" + array[1];
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = Path.GetFileName(file);
                    string destFileName = dstFolder + "\\" + array[1] + "\\" + fileName;
                    File.Copy(file, destFileName, overwrite: true);
                }
            }
        }

        private static void CopyFolder(string dstFolder)
        {
            foreach (string folder in copyConfig.Folder.folders)
            {
                if (!Directory.Exists(folder))
                {
                    continue;
                }
                string[] files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
                string[] array = files;
                foreach (string text in array)
                {
                    DateTime lastWriteTime = File.GetLastWriteTime(text);
                    double num = (DateTime.Now - lastWriteTime).TotalSeconds / 3600.0;
                    if (num > copyConfig.FolderHour.Hour)
                    {
                        continue;
                    }
                    string directoryName = Path.GetDirectoryName(text);
                    string[] array2 = directoryName.Split(':');
                    if (array2.Length == 2)
                    {
                        string path = dstFolder + "\\" + array2[1];
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = Path.GetFileName(text);
                        string destFileName = dstFolder + "\\" + array2[1] + "\\" + fileName;
                        File.Copy(text, destFileName, overwrite: true);
                    }
                }
            }
        }

        private static void SnipScreen()
        {
            string text = DateTime.Now.ToString("yyyyMMddHHmmsss");
            CaptureScreenshot.HideConsole();
            Bitmap screen = CaptureScreenshot.GetScreen();
            if (!Directory.Exists(CutDirectory))
            {
                Directory.CreateDirectory(CutDirectory);
            }
            screen.Save(CutDirectory + "\\" + text + ".png", ImageFormat.Png);
        }

        private static void SaveReport()
        {
            string text = DateTime.Now.ToString("yyyyMMddHHmmsss");
            string text2 = copyConfig.Report.Report + "\\" + text;
            if (!Directory.Exists(text2))
            {
                Directory.CreateDirectory(text2);
            }
            WriteIssueDetatil(text2);
            SaveScreen(text2 + "\\" + text + ".png");
            CopyFolder(text2);
            CopyFile(text2);
            ZipFile.CreateFromDirectory(text2, text2 + ".zip");
        }

        private static void Main(string[] args)
        {
            copyConfig.LoadConfig(Directory.GetCurrentDirectory() + "\\CopyConfig.ini");
            //SnipScreen();
            SaveReport();
        }
    }
}