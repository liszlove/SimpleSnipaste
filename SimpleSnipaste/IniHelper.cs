using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleSnipaste
{
    public class IniHelper
    {
        public class ExampleConfig
        {
            public class _HIK_Camera
            {
                public string CameraName = "hikvision camera";

                public string CameraSn = "2020SN12041330";

                public double CameraGain = 11.25;

                public double CameraExposure = 500.0;

                public int ImageWidth = 1280;

                public int ImageHeight = 1024;

                public bool HardwareTrigger = true;

                public bool ImageRotate = false;
            }

            public class _DaHeng_Camera
            {
                public string CameraName = "daheng camera";

                public string CameraSn = "2020SN12041333";

                public double CameraGain = 3.75;

                public double CameraExposure = 1500.0;

                public int ImageWidth = 640;

                public int ImageHeight = 480;

                public bool HardwareTrigger = true;

                public bool ImageRotate = true;
            }

            public class _Basler_Camera
            {
                public string CameraName = "basler camera";

                public string CameraSn = "2020SN12041334";

                public double CameraGain = 16.0;

                public double CameraExposure = 80.0;

                public int ImageWidth = 2480;

                public int ImageHeight = 2048;

                public bool HardwareTrigger = false;

                public bool ImageRotate = false;
            }

            public _HIK_Camera HIK_Camera = new _HIK_Camera();

            public _DaHeng_Camera DaHeng_Camera = new _DaHeng_Camera();

            public _Basler_Camera Basler_Camera = new _Basler_Camera();
        }

        public static string ConfigIniFileName = ".\\Config.ini";

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern int GetPrivateProfileSectionNamesA(byte[] szBuffer, int nSize, string lpFileName);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern int GetPrivateProfileSectionA(string lpAppName, byte[] szBuffer, int nSize, string lpFileName);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int WritePrivateProfileSectionA(string lpAppName, byte[] szBuffer, string lpFileName);

        public static bool WriteIni(string SectionName, string KeyName, string KeyValue, string FileName)
        {
            return WritePrivateProfileString(SectionName, KeyName, KeyValue, FileName);
        }

        public static int ReadIni(string SectionName, string KeyName, string DefaultKeyValue, ref string ReturnedValue, string FileName)
        {
            string fullPath = Path.GetFullPath(FileName);
            if (!File.Exists(fullPath))
            {
                return -1;
            }
            int nSize = 2048;
            StringBuilder stringBuilder = new StringBuilder();
            int privateProfileString = GetPrivateProfileString(SectionName, KeyName, DefaultKeyValue, stringBuilder, nSize, fullPath);
            if (stringBuilder.ToString().Length == 0)
            {
                stringBuilder.Append(DefaultKeyValue);
            }
            ReturnedValue = stringBuilder.ToString();
            return privateProfileString;
        }

        public static void WriteIni<T>(string SectionName, string KeyName, T KeyValue, string FileName)
        {
            Type type = KeyValue.GetType();
            string text = KeyValue.ToString();
            if (type.Equals(typeof(short)) || type.Equals(typeof(int)) || type.Equals(typeof(long)) || type.Equals(typeof(ushort)) || type.Equals(typeof(uint)) || type.Equals(typeof(ulong)))
            {
                WritePrivateProfileString(SectionName, KeyName, text, FileName);
            }
            if (type.Equals(typeof(bool)))
            {
                WritePrivateProfileString(SectionName, KeyName, (text.Equals("true", StringComparison.OrdinalIgnoreCase) ? 1 : 0).ToString(), FileName);
            }
            if (type.Equals(typeof(double)))
            {
                WritePrivateProfileString(SectionName, KeyName, Convert.ToDouble(text).ToString("F3"), FileName);
            }
            if (type.Equals(typeof(string)))
            {
                WritePrivateProfileString(SectionName, KeyName, text, FileName);
            }
        }

        public static T ReadIni<T>(string SectionName, string KeyName, T DefaultKeyValue, string FileName)
        {
            string fullPath = Path.GetFullPath(FileName);
            if (!File.Exists(fullPath))
            {
                return default(T);
            }
            Type typeFromHandle = typeof(T);
            string empty = string.Empty;
            empty = ((!typeFromHandle.Equals(typeof(bool))) ? DefaultKeyValue.ToString() : ((DefaultKeyValue.ToString() == "False") ? "0" : "1"));
            string ReturnedValue = string.Empty;
            ReadIni(SectionName, KeyName, empty, ref ReturnedValue, fullPath);
            if (typeFromHandle.Equals(typeof(short)))
            {
                return (T)(object)Convert.ToInt16(ReturnedValue);
            }
            if (typeFromHandle.Equals(typeof(int)))
            {
                return (T)(object)Convert.ToInt32(ReturnedValue);
            }
            if (typeFromHandle.Equals(typeof(long)))
            {
                return (T)(object)Convert.ToInt64(ReturnedValue);
            }
            if (typeFromHandle.Equals(typeof(ushort)))
            {
                return (T)(object)Convert.ToUInt16(ReturnedValue);
            }
            if (typeFromHandle.Equals(typeof(uint)))
            {
                return (T)(object)Convert.ToUInt32(ReturnedValue);
            }
            if (typeFromHandle.Equals(typeof(ulong)))
            {
                return (T)(object)Convert.ToUInt64(ReturnedValue);
            }
            if (typeFromHandle.Equals(typeof(bool)))
            {
                return (T)(object)((Convert.ToInt32(ReturnedValue) == 1) ? true : false);
            }
            if (typeFromHandle.Equals(typeof(double)))
            {
                return (T)(object)Convert.ToDouble(ReturnedValue);
            }
            if (typeFromHandle.Equals(typeof(string)))
            {
                return (T)(object)ReturnedValue;
            }
            if (typeFromHandle.Equals(typeof(byte)))
            {
                return (T)(object)Convert.ToByte(ReturnedValue, 10);
            }
            return default(T);
        }

        public static T1 ReadIni<T1, T2>(string SectionName, string KeyName, string DefaultKeyValue, string FileName)
        {
            string fullPath = Path.GetFullPath(FileName);
            if (!File.Exists(fullPath))
            {
                return (T1)(object)default(T2);
            }
            string ReturnedValue = string.Empty;
            ReadIni(SectionName, KeyName, DefaultKeyValue, ref ReturnedValue, fullPath);
            Type typeFromHandle = typeof(T1);
            Type typeFromHandle2 = typeof(T2);
            if (typeFromHandle2.Equals(typeof(short)))
            {
                return (T1)(object)Convert.ToInt16(ReturnedValue);
            }
            if (typeFromHandle2.Equals(typeof(int)))
            {
                return (T1)(object)Convert.ToInt32(ReturnedValue);
            }
            if (typeFromHandle2.Equals(typeof(long)))
            {
                return (T1)(object)Convert.ToInt64(ReturnedValue);
            }
            if (typeFromHandle2.Equals(typeof(ushort)))
            {
                return (T1)(object)Convert.ToUInt16(ReturnedValue);
            }
            if (typeFromHandle2.Equals(typeof(uint)))
            {
                return (T1)(object)Convert.ToUInt32(ReturnedValue);
            }
            if (typeFromHandle2.Equals(typeof(ulong)))
            {
                return (T1)(object)Convert.ToUInt64(ReturnedValue);
            }
            if (typeFromHandle2.Equals(typeof(bool)))
            {
                return (T1)(object)((Convert.ToInt32(ReturnedValue) == 1) ? true : false);
            }
            if (typeFromHandle2.Equals(typeof(double)))
            {
                return (T1)(object)Convert.ToDouble(ReturnedValue);
            }
            if (typeFromHandle2.Equals(typeof(string)))
            {
                return (T1)(object)ReturnedValue;
            }
            return (T1)(object)default(T2);
        }

        public static int ReadIniSections(out List<string> sections, string fileName)
        {
            sections = new List<string>();
            string fullPath = Path.GetFullPath(fileName);
            if (!File.Exists(fullPath))
            {
                return -1;
            }
            int num = 102400;
            byte[] array = new byte[num];
            int privateProfileSectionNamesA = GetPrivateProfileSectionNamesA(array, num, fullPath);
            string @string = Encoding.Default.GetString(array);
            string[] array2 = @string.Split(default(char));
            for (int i = 0; i < array2.Length; i++)
            {
                if (array2[i].Length != 0)
                {
                    sections.Add(array2[i]);
                }
            }
            return 0;
        }

        public static int ReadIniSectionKeyValues(string section, out Dictionary<string, string> keyValues, string fileName)
        {
            keyValues = new Dictionary<string, string>();
            string fullPath = Path.GetFullPath(fileName);
            if (!File.Exists(fullPath))
            {
                return -1;
            }
            int num = 102400;
            byte[] array = new byte[num];
            int privateProfileSectionA = GetPrivateProfileSectionA(section, array, num, fullPath);
            string @string = Encoding.Default.GetString(array);
            string[] array2 = @string.Split(default(char));
            for (int i = 0; i < array2.Length; i++)
            {
                if (!string.IsNullOrEmpty(array2[i]))
                {
                    string[] array3 = array2[i].Split('=');
                    if (array3.Length == 2)
                    {
                        keyValues[array3[0]] = array3[1];
                    }
                }
            }
            return 0;
        }

        public static int ReadIniSectionKeyValues(string section, out List<string> values, string fileName)
        {
            values = new List<string>();
            string fullPath = Path.GetFullPath(fileName);
            if (!File.Exists(fullPath))
            {
                return -1;
            }
            int num = 102400;
            byte[] array = new byte[num];
            int privateProfileSectionA = GetPrivateProfileSectionA(section, array, num, fullPath);
            string @string = Encoding.UTF8.GetString(array);
            string[] array2 = @string.Split(default(char));
            for (int i = 0; i < array2.Length; i++)
            {
                if (!string.IsNullOrEmpty(array2[i]))
                {
                    values.Add(array2[i]);
                }
            }
            return 0;
        }

        public static T IniDeserialize<T>(string iniFileName)
        {
            try
            {
                Type typeFromHandle = typeof(T);
                object obj = Activator.CreateInstance(typeFromHandle);
                List<string> sections = new List<string>();
                ReadIniSections(out sections, iniFileName);
                foreach (string item in sections)
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    ReadIniSectionKeyValues(item, out keyValues, iniFileName);
                    FieldInfo field = typeFromHandle.GetField(item);
                    object obj2 = Activator.CreateInstance(field.FieldType);
                    foreach (KeyValuePair<string, string> item2 in keyValues)
                    {
                        FieldInfo field2 = field.FieldType.GetField(item2.Key);
                        if (field2.FieldType.Equals(typeof(short)))
                        {
                            field2.SetValue(obj2, Convert.ToInt16(item2.Value));
                        }
                        else if (field2.FieldType.Equals(typeof(int)))
                        {
                            if (item2.Value.Contains("0x") || item2.Value.Contains("0X"))
                            {
                                field2.SetValue(obj2, Convert.ToInt32(item2.Value, 16));
                            }
                            else
                            {
                                field2.SetValue(obj2, Convert.ToInt32(item2.Value));
                            }
                        }
                        else if (field2.FieldType.Equals(typeof(long)))
                        {
                            field2.SetValue(obj2, Convert.ToInt64(item2.Value));
                        }
                        else if (field2.FieldType.Equals(typeof(ushort)))
                        {
                            field2.SetValue(obj2, Convert.ToUInt16(item2.Value));
                        }
                        else if (field2.FieldType.Equals(typeof(uint)))
                        {
                            field2.SetValue(obj2, Convert.ToUInt32(item2.Value));
                        }
                        else if (field2.FieldType.Equals(typeof(ulong)))
                        {
                            field2.SetValue(obj2, Convert.ToUInt64(item2.Value));
                        }
                        else if (field2.FieldType.Equals(typeof(bool)))
                        {
                            field2.SetValue(obj2, Convert.ToBoolean(item2.Value));
                        }
                        else if (field2.FieldType.Equals(typeof(double)))
                        {
                            field2.SetValue(obj2, Convert.ToDouble(item2.Value));
                        }
                        else if (field2.FieldType.Equals(typeof(string)))
                        {
                            string text = item2.Value;
                            if (text.StartsWith("\""))
                            {
                                text = text.Remove(0, 1);
                            }
                            if (text.EndsWith("\""))
                            {
                                text = text.Remove(text.Length - 1, 1);
                            }
                            field2.SetValue(obj2, text);
                        }
                        else if (field2.FieldType.Equals(typeof(byte)))
                        {
                            field2.SetValue(obj2, Convert.ToByte(item2.Value, 10));
                        }
                        else
                        {
                            field2.SetValue(obj2, item2.Value);
                        }
                    }
                    field.SetValue(obj, obj2);
                }
                return (T)obj;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static void IniSerialize<T>(T tObject, string iniFileName)
        {
            try
            {
                string fullPath = Path.GetFullPath(iniFileName);
                Type typeFromHandle = typeof(T);
                FieldInfo[] fields = typeFromHandle.GetFields();
                FieldInfo[] array = fields;
                foreach (FieldInfo fieldInfo in array)
                {
                    string name = fieldInfo.Name;
                    FieldInfo field = typeFromHandle.GetField(name);
                    object value = field.GetValue(tObject);
                    FieldInfo[] fields2 = field.FieldType.GetFields();
                    FieldInfo[] array2 = fields2;
                    foreach (FieldInfo fieldInfo2 in array2)
                    {
                        WritePrivateProfileString(name, fieldInfo2.Name, fieldInfo2.GetValue(value).ToString(), fullPath);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public static void CreateExampleIni(string iniFileName = ".\\ExampleConfig.ini")
        {
            ExampleConfig tObject = new ExampleConfig();
            IniSerialize(tObject, iniFileName);
        }
    }
}