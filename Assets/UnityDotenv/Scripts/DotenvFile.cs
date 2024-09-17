using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace UnityDotenv
{
    public class DotenvFile
    {

        public static string FileFullPath
        {
            get
            {
                return Path.Combine(Application.streamingAssetsPath, FilePath);
            }
        }

        public static string FilePath
        {
            get
            {
                return ".env";
            }
        }

        public static void WriteDotenvFile(Dictionary<string, string> keyValueDic)
        {
            List<string> lines = new List<string>();
            foreach (var keyValue in keyValueDic)
            {
                if (string.IsNullOrEmpty(keyValue.Key)) continue;
                string line = string.Join("=", new string[] { keyValue.Key, keyValue .Value});
                lines.Add(line);
            }
            File.WriteAllText(FileFullPath, string.Join("\n", lines.ToArray()));
        }
    }
}
