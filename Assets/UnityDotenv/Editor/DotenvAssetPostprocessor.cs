using UnityEngine;
using System.IO;
using UnityEditor;

namespace UnityDotenv
{
    public class DotenvAssetPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            DotenvAssetPostprocessor.CreateDotenvFile();
        }

        public static void CreateDotenvFile()
        {
            string dotenvFilePath = DotenvFile.FileFullPath;
            string dirPath = Path.GetDirectoryName(dotenvFilePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            if (!File.Exists(dotenvFilePath))
            {
                File.WriteAllText(DotenvFile.FileFullPath, "");
            }
        }
    }
}