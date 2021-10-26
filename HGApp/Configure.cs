using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ConfigurationTool
{
    public class Configure
    {
        private static string _filePath = Application.StartupPath + "\\" + "io.ini";

        [DllImport("Kernel32.dll")]
        private static extern ulong GetPrivateProfileString(string strAppName, string strKeyName, string strDefault,
            StringBuilder sbReturnString, int nSize, string strFileName);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern uint GetPrivateProfileStringA(string section, string key, string def, Byte[] retVal, int size, string filePath);
        [DllImport("Kernel32.dll")]
        private static extern bool WritePrivateProfileString(string strAppName, string strKeyName, string strString,
            string strFileName);

        [DllImport("Kernel32.dll")]
        private static extern int GetPrivateProfileInt(string strAppName, string strKeyName, int nDefault, string strFileName);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSectionNames(byte[] buffer, int iLen, string lpFileName);
        private Configure(string filePath)
        {
            filePath = _filePath;
        }

        public static string FilePath
        {
            set
            {
                _filePath = value;
            }
        }

        /// <summary>
        /// 读取string类型数据
        /// </summary>
        /// <param name="strAppName"></param>
        /// <param name="strKeyName"></param>
        /// <param name="strDefault"></param>
        /// <returns></returns>
        public static string ReadConfig(string strAppName, string strKeyName, string strDefault)
        {
            return ReadConfig(strAppName, strKeyName, strDefault, _filePath );
        }

        public static double ReadConfig(string strAppName, string strKeyName, double dDefault)
        {
            string result = ReadConfig(strAppName, strKeyName, dDefault.ToString(), _filePath );
            return StringToDouble(result, dDefault);
        }

        public static float ReadConfig(string strAppName, string strKeyName, float dDefault)
        {
            string result = ReadConfig(strAppName, strKeyName, dDefault.ToString(), _filePath );
            return StringToFloat(result, dDefault);
        }

        private static string ReadConfig(string strAppName, string strKeyName, string strDefault, string strFilepath)
        {
            StringBuilder strReturn = new StringBuilder(255);
            GetPrivateProfileString(strAppName, strKeyName, strDefault, strReturn, 255, strFilepath);
            return strReturn.ToString();
        }

        /// <summary>
        /// 读取int类型数据
        /// </summary>
        /// <param name="strAppName"></param>
        /// <param name="strKeyName"></param>
        /// <param name="ndefault"></param>
        /// <returns></returns>
        public static int ReadConfig(string strAppName, string strKeyName, int ndefault)
        {
            return ReadConfig(strAppName, strKeyName, ndefault, _filePath );
        }

        public static bool ReadConfig(string strAppName, string strKeyName, bool bdefault)
        {
            string result = ReadConfig(strAppName, strKeyName, bdefault.ToString(), _filePath );
            return StringToBool(result, bdefault);
        }

        private static int ReadConfig(string strAppName, string strKeyName, int ndefault, string strFilepath)
        {
            int result = GetPrivateProfileInt(strAppName, strKeyName, ndefault, strFilepath);
            return result;
        }

        /// <summary>
        /// 写入string类型数据
        /// </summary>
        /// <param name="strAppName"></param>
        /// <param name="strKeyName"></param>
        /// <param name="strString"></param>
        /// <returns></returns>
        public static bool WriteConfig(string strAppName, string strKeyName, string strString)
        {
            return WritePrivateProfileString(strAppName, strKeyName, strString, _filePath );
        }

        private static bool WriteConfig(string strAppName, string strKeyName, string strString, string strFilepath)
        {
            return WritePrivateProfileString(strAppName, strKeyName, strString, strFilepath);
        }

        /// <summary>
        /// 写入int类型数据
        /// </summary>
        /// <param name="strAppName"></param>
        /// <param name="strKeyName"></param>
        /// <param name="strString"></param>
        /// <returns></returns>
        public static bool WriteConfig(string strAppName, string strKeyName, int strString)
        {
            return WritePrivateProfileString(strAppName, strKeyName, strString.ToString(), _filePath );
        }

        private static bool WriteConfig(string strAppName, string strKeyName, int strString, string strFilepath)
        {
            return WritePrivateProfileString(strAppName, strKeyName, strString.ToString(), strFilepath);
        }

        private static double StringToDouble(string strValue, double Fail)
        {
            double result = Fail;
            if (strValue == null || strValue == "")
            {
                return result;
            }
            if (!Double.TryParse(strValue, out result))
            {
                result = Fail;
            }
            return result;
        }

        private static float StringToFloat(string strValue, float Fail)
        {
            float result = Fail;
            if (strValue == null || strValue == "")
            {
                return result;
            }
            if (!float.TryParse(strValue, out result))
            {
                result = Fail;
            }
            return result;
        }

        private static int StringToInt(string strValue, int Fail)
        {
            int result = Fail;
            if (strValue == null || strValue == "")
            {
                return result;
            }
            if (!Int32.TryParse(strValue, out result))
            {
                result = Fail;
            }
            return result;
        }

        private static bool StringToBool(string strValue, bool Fail)
        {
            bool result = Fail;
            if (strValue == null || strValue == "")
            {
                return result;
            }
            if (!bool.TryParse(strValue, out result))
            {
                result = Fail;
            }
            return result;
        }

        public static void DelectSection(string SectionName)
        {
            WritePrivateProfileString(SectionName, null, null, _filePath);
        }
        public static void DeleteKey(string strSection, string strKey)
        {
            WritePrivateProfileString(strSection, strKey, null, _filePath);
        }
        //读取某个section下所有的key和value
        public static Dictionary<string, string> ReadKeys(string SectionName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Byte[] buf = new Byte[65536];
            uint len = GetPrivateProfileStringA(SectionName, null, null, buf, buf.Length, _filePath);
            int j = 0;
            for (int i = 0; i < len; i++)
            {
                if (buf[i] == 0)
                {
                    string strName = Encoding.Default.GetString(buf, j, i - j);
                    result.Add(strName, ReadConfig(SectionName, strName, strName));
                    j = i + 1;
                }
            }
            return result;
        }
        //读取所有的section
        public static List<string> ReadSections()
        {
            byte[] buffer = new byte[65535];
            int rel = GetPrivateProfileSectionNames(buffer, buffer.GetUpperBound(0), _filePath);
            return Conver2ArrayList(rel, buffer);
        }

        private static List<string> Conver2ArrayList(int rel, byte[] buffer)
        {
            List<string> arrayList = new List<string>();
            if (rel > 0)
            {
                int iCnt, iPos;
                string tmp;
                iCnt = 0;
                iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (buffer[iCnt] == 0x00)
                    {
                        tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                            arrayList.Add(tmp);
                    }
                }
            }
            return arrayList;
        }
    }
}
