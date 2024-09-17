using System.Text;
using System.Collections.Generic;
using UnityEngine;
using DotNetEnv;

namespace UnityDotenv
{
    public class DotenvPreloader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoadRuntimeMethod()
        {
            IEnumerable<KeyValuePair<string, string>> envValuePaur = Env.Load(DotenvFile.FileFullPath);
#if UNITY_EDITOR
            if (PlayerPrefs.GetInt(Const.ShowLoadDebugDotenvKey, 1) == 1)
            {
                List<string> kvList = new List<string>();
                foreach (KeyValuePair<string, string> kvp in envValuePaur)
                {
                    kvList.Add(kvp.Key + "=" + kvp.Value);
                }
                StringBuilder logStr = new StringBuilder();
                logStr.Append("<color=#ffd700>Load Dotenv Data</color>");
                logStr.Append("\n\n");
                logStr.Append("<color=#ffd700>");
                logStr.Append(string.Join("\n", kvList));
                logStr.Append("</color>");
                Debug.Log(logStr.ToString());
            }
#endif
        }
    }
}
