using UnityEngine;
using DotNetEnv;
public class DotenvPreloader : MonoBehaviour
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void OnBeforeSceneLoadRuntimeMethod()
	{
		Env.Load();
	}
}