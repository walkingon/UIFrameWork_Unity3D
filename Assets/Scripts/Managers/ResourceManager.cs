using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ResourceManager : MonoSingleton<ResourceManager>
{

    public delegate void downloadAssetBundleCallback(AssetBundle ab);

    string assetBundleUrl = AppConst.DownloadServerUrl + AppConst.AssetBundlePath + "ALL.assetbundle";

    public void Init()
    { 

    }

    public IEnumerator DownloadAssetBundle(string url, downloadAssetBundleCallback cbk)
    {
        Debug.Log("下载AssetBundle: " + url);
        var uwr = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return uwr.SendWebRequest();

        // Get an asset from the bundle.
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
        yield return bundle;

        Debug.Log("下载完成.");
        cbk(bundle);
    }

    void StartDownloadAssetBundle()
    {
        var cbk = new ResourceManager.downloadAssetBundleCallback(OnDownloadSuccess);
        StartCoroutine(DownloadAssetBundle(this.assetBundleUrl, cbk));
    }

    void OnDownloadSuccess(AssetBundle ab)
    {
        string[] assetNames = ab.GetAllAssetNames();
        for (int i = 0; i < assetNames.Length; i++)
        {
            string name = assetNames[i];
            Debug.Log(name);
        }
    }
}
