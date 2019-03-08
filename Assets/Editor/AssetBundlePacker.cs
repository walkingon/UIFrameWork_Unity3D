using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

class AssetBundlePacker
{
    [MenuItem("Custom Tools/Asset Bundle Packer/Android/Split")]
    static void BuildAndroidSplit()
    {
        AssetBundlePacker.CreateAssetBundle(false, BuildTarget.Android);
    }

    [MenuItem("Custom Tools/Asset Bundle Packer/Android/One")]
    static void BuildAndroidOne()
    {
        AssetBundlePacker.CreateAssetBundle(true, BuildTarget.Android);
    }

    [MenuItem("Custom Tools/Asset Bundle Packer/iOS/Split")]
    static void BuildiOSSplit()
    {
        AssetBundlePacker.CreateAssetBundle(false, BuildTarget.iOS);
    }

    [MenuItem("Custom Tools/Asset Bundle Packer/iOS/One")]
    static void BuildiOSOne()
    {
        AssetBundlePacker.CreateAssetBundle(true, BuildTarget.iOS);
    }

    [MenuItem("Custom Tools/Asset Bundle Packer/Mac/Split")]
    static void BuildMacSplit()
    {
        AssetBundlePacker.CreateAssetBundle(false, BuildTarget.StandaloneOSX);
    }

    [MenuItem("Custom Tools/Asset Bundle Packer/Mac/One")]
    static void BuildMacOne()
    {
        AssetBundlePacker.CreateAssetBundle(true, BuildTarget.StandaloneOSX);
    }

    /// <summary>
    /// Creates the asset bundle.
    /// </summary>
    /// <param name="isAll">If set to <c>true</c> is all.</param>
    /// <param name="target">Target.</param>
    static void CreateAssetBundle(bool isAll, BuildTarget target)
    {

        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/StreamingAssets/");
        AssetDatabase.CreateFolder("Assets", "StreamingAssets");

        if (isAll)
        {
            AssetBundlePacker.CreateAssetBunldesALL(target);
        }
        else
        {
            AssetBundlePacker.CreateAssetBunldesMain(target);
        }
    }

    /// <summary>
    /// 分开打包，会生成两个Assetbundle。
    /// </summary>
    static void CreateAssetBunldesMain(BuildTarget target)
    {
        //获取在Project视图中选择的所有游戏对象  
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        //遍历所有的游戏对象  
        foreach (Object obj in SelectedAsset)
        {
            string sourcePath = AssetDatabase.GetAssetPath(obj);
            //本地测试：建议最后将Assetbundle放在StreamingAssets文件夹下，如果没有就创建一个，因为移动平台下只能读取这个路径  
            //StreamingAssets是只读路径，不能写入  
            //服务器下载：就不需要放在这里，服务器上客户端用www类进行下载。  
            string targetPath = Application.dataPath + "/StreamingAssets/" + obj.name + ".assetbundle";
            if (BuildPipeline.BuildAssetBundle(obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies, target))
            {
                Debug.Log(obj.name + "资源打包成功");
            }
            else
            {
                Debug.Log(obj.name + "资源打包失败");
            }
        }
        //刷新编辑器  
        AssetDatabase.Refresh();

    }

    /// <summary>
    /// 将所有对象打包在一个Assetbundle中。
    /// </summary>
    static void CreateAssetBunldesALL(BuildTarget target)
    {

        Caching.ClearCache();

        string Path = Application.dataPath + "/StreamingAssets/ALL.assetbundle";

        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        foreach (Object obj in SelectedAsset)
        {
            Debug.Log("Create AssetBunldes name :" + obj);
        }

        //这里注意第二个参数就行  
        if (BuildPipeline.BuildAssetBundle(null, SelectedAsset, Path, BuildAssetBundleOptions.CollectDependencies, target))
        {
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.Log("资源打包失败");
        }
    }

}
