using UnityEngine;
using System.Collections;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static string rootName = "MonoSingletonRoot";
    private static GameObject monoSingletionRoot = null;

    private static T instance = null;
    private static readonly object syslock = new object();

    public static T getInstance()
    {
        lock (syslock) {
            if (monoSingletionRoot == null)
            {
                monoSingletionRoot = GameObject.Find(rootName);
                if(monoSingletionRoot == null)
                {
                    monoSingletionRoot = new GameObject(rootName);
                    DontDestroyOnLoad(monoSingletionRoot);
                }
            }
            if (instance == null)
            {
                instance = monoSingletionRoot.AddComponent<T>();
            }
        }
        return instance;
    }

}

