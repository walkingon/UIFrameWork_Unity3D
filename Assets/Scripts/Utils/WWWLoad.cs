using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class WWWLoad
{
    private WWW www = null;
    static System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
    /// <summary>
    /// 下载文件
    /// </summary>
    public IEnumerator DownFile(string url, string savePath, Action<WWW> process)
    {
        FileInfo file = new FileInfo(savePath);
        stopWatch.Start();
        UnityEngine.Debug.Log("Start:" + Time.realtimeSinceStartup);
        www = new WWW(url);
        while (!www.isDone)
        {
            yield return 0;
            if (process != null)
                process(www);
        }
        yield return www;
        if (www.isDone)
        {
            byte[] bytes = www.bytes;
            CreatFile(savePath, bytes);
        }
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="bytes"></param>
    public void CreatFile(string savePath, byte[] bytes)
    {
        FileStream fs = new FileStream(savePath, FileMode.Append);
        BinaryWriter bw = new BinaryWriter(fs);
        fs.Write(bytes, 0, bytes.Length);
        fs.Flush();     //流会缓冲，此行代码指示流不要缓冲数据，立即写入到文件。
        fs.Close();     //关闭流并释放所有资源，同时将缓冲区的没有写入的数据，写入然后再关闭。
        fs.Dispose();   //释放流
        www.Dispose();

        stopWatch.Stop();
        Debug.Log("下载完成,耗时:" + stopWatch.ElapsedMilliseconds);
        UnityEngine.Debug.Log("End:" + Time.realtimeSinceStartup);
    }

}