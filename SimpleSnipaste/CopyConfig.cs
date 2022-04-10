using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSnipaste
{
    public class CopyConfig
    {
        public class CopiedFolder
        {
            public List<string> folders = new List<string>();

            public static CopiedFolder InitFolder()
            {
                CopiedFolder copiedFolder = new CopiedFolder();
                copiedFolder.folders.Add("D:\\log\\Tester");
                copiedFolder.folders.Add("D:\\log\\VisionService");
                return copiedFolder;
            }

            public new string ToString()
            {
                string text = string.Empty;
                foreach (string folder in folders)
                {
                    text = text + folder + "\0";
                }
                return text;
            }
        }

        public class CopiedFolderHour
        {
            public List<string> hours = new List<string>();

            public double Hour
            {
                get
                {
                    double result = 1.0;
                    if (hours.Count == 1 && !double.TryParse(hours[0], out result))
                    {
                        return 1.0;
                    }
                    return result;
                }
            }

            public new string ToString()
            {
                return Hour.ToString();
            }
        }

        public class CopiedFile
        {
            public List<string> files = new List<string>();

            public static CopiedFile InitFolder()
            {
                CopiedFile copiedFile = new CopiedFile();
                copiedFile.files.Add("D:\\log\\Tester");
                copiedFile.files.Add("D:\\log\\VisionService");
                return copiedFile;
            }

            public new string ToString()
            {
                string text = string.Empty;
                foreach (string file in files)
                {
                    text = text + file + "\0";
                }
                return text;
            }
        }

        public class IssueReport
        {
            public List<string> reports = new List<string>();

            public string Report
            {
                get
                {
                    string result = "D:\\IssueReport";
                    if (reports.Count == 1 && !string.IsNullOrEmpty(reports[0]))
                    {
                        return reports[0];
                    }
                    return result;
                }
            }

            public new string ToString()
            {
                return Report;
            }
        }

        public CopiedFolder Folder { get; set; } = new CopiedFolder();


        public CopiedFolderHour FolderHour { get; set; } = new CopiedFolderHour();


        public CopiedFile File { get; set; } = new CopiedFile();


        public IssueReport Report { get; set; } = new IssueReport();


        public void LoadConfig(string config = "CopyConfig.ini")
        {
            if (!System.IO.File.Exists(config))
            {
                CopyConfig copyConfig = CreateCopyExample();
                IniHelper.WritePrivateProfileSectionA("Folder", Encoding.UTF8.GetBytes(copyConfig.Folder.ToString()), config);
                IniHelper.WritePrivateProfileSectionA("FolderHour", Encoding.UTF8.GetBytes(copyConfig.FolderHour.ToString()), config);
                IniHelper.WritePrivateProfileSectionA("File", Encoding.UTF8.GetBytes(copyConfig.File.ToString()), config);
                IniHelper.WritePrivateProfileSectionA("Report", Encoding.UTF8.GetBytes(copyConfig.Report.ToString()), config);
            }
            IniHelper.ReadIniSectionKeyValues("Folder", out Folder.folders, config);
            IniHelper.ReadIniSectionKeyValues("FolderHour", out FolderHour.hours, config);
            IniHelper.ReadIniSectionKeyValues("File", out File.files, config);
            IniHelper.ReadIniSectionKeyValues("Report", out Report.reports, config);
        }

        private static CopyConfig CreateCopyExample()
        {
            CopyConfig copyConfig = new CopyConfig();
            copyConfig.Folder = CopiedFolder.InitFolder();
            copyConfig.File = CopiedFile.InitFolder();
            copyConfig.FolderHour = new CopiedFolderHour();
            copyConfig.Report = new IssueReport();
            return copyConfig;
        }
    }
}
