using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using System.Threading;

public class MultiHttpDownLoad : MonoBehaviour
{
    string savePath = string.Empty;
    string FileName = "ClickEffect.apk";
    string resourceURL = @"http://www.dingxiaowei.cn/ClickEffect.apk";
    string saveFile = string.Empty;
    public int ThreadNum { get; set; }
    public bool[] ThreadStatus { get; set; }
    public string[] FileNames { get; set; }
    public int[] StartPos { get; set; }
    public int[] FileSize { get; set; }
    public string Url { get; set; }
    public bool IsMerge { get; set; }
    private int buffSize = 1024;
    DateTime beginTime;

    void Start()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        savePath = Application.streamingAssetsPath;
#elif UNITY_ANDROID
          savePath = Application.persistentDataPath;;
#endif
        saveFile = Path.Combine(savePath, FileName);

        DownDoad();
    }

    void Init(long fileSize)
    {
        if (ThreadNum == 0)
            ThreadNum = 5;

        ThreadStatus = new bool[ThreadNum];
        FileNames = new string[ThreadNum];
        StartPos = new int[ThreadNum];//下载字节起始点
        FileSize = new int[ThreadNum];//该进程文件大小
        int fileThread = (int)fileSize / ThreadNum;//单进程文件大小
        int fileThreade = fileThread + (int)fileSize % ThreadNum;//最后一个进程的资源大小
        for (int i = 0; i < ThreadNum; i++)
        {
            ThreadStatus[i] = false;
            FileNames[i] = i.ToString() + ".dat";
            if (i < ThreadNum - 1)
            {
                StartPos[i] = fileThread * i;
                FileSize[i] = fileThread;
            }
            else
            {
                StartPos[i] = fileThread * i;
                FileSize[i] = fileThreade;
            }
        }
    }

    void DownDoad()
    {
        UnityEngine.Debug.Log("开始下载 时间:" + System.DateTime.Now.ToString());
        beginTime = System.DateTime.Now;
        Url = resourceURL;
        long fileSizeAll = 0;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
        fileSizeAll = request.GetResponse().ContentLength;
        Init(fileSizeAll);

        System.Threading.Thread[] threads = new System.Threading.Thread[ThreadNum];
        HttpMultiThreadDownload[] httpDownloads = new HttpMultiThreadDownload[ThreadNum];
        for (int i = 0; i < ThreadNum; i++)
        {
            httpDownloads[i] = new HttpMultiThreadDownload(request, this, i);
            threads[i] = new System.Threading.Thread(new System.Threading.ThreadStart(httpDownloads[i].Receive));
            threads[i].Name = string.Format("线程{0}:", i);
            threads[i].Start();
        }
        StartCoroutine(MergeFile());
    }

    IEnumerator MergeFile()
    {
        while (true)
        {
            IsMerge = true;
            for (int i = 0; i < ThreadNum; i++)
            {
                if (ThreadStatus[i] == false)
                {
                    IsMerge = false;
                    yield return 0;
                    System.Threading.Thread.Sleep(100);
                    break;
                }
            }
            if (IsMerge)
                break;
        }

        int bufferSize = 512;
        string downFileNamePath = saveFile;
        byte[] bytes = new byte[bufferSize];
        FileStream fs = new FileStream(downFileNamePath, FileMode.Create);
        FileStream fsTemp = null;

        for (int i = 0; i < ThreadNum; i++)
        {
            fsTemp = new FileStream(FileNames[i], FileMode.Open);
            while (true)
            {
                yield return 0;
                buffSize = fsTemp.Read(bytes, 0, bufferSize);
                if (buffSize > 0)
                    fs.Write(bytes, 0, buffSize);
                else
                    break;
            }
            fsTemp.Close();
        }
        fs.Close();
        Debug.Log("接受完毕!!!结束时间:" + System.DateTime.Now.ToString());
        Debug.LogError("下载耗时:" + (System.DateTime.Now - beginTime).TotalSeconds.ToString());
        yield return null;
        DeleteCacheFiles();
    }

    private void DeleteCacheFiles()
    {
        for (int i = 0; i < ThreadNum; i++)
        {
            FileInfo info = new FileInfo(FileNames[i]);
            Debug.LogFormat("Delete File {0} OK!", FileNames[i]);
            info.Delete();
        }
    }
}

public class HttpMultiThreadDownload
{
    private int threadId;
    private string url;
    MultiHttpDownLoad downLoadObj;
    private const int buffSize = 1024;
    HttpWebRequest request;

    public HttpMultiThreadDownload(HttpWebRequest request, MultiHttpDownLoad downLoadObj, int threadId)
    {
        this.request = request;
        this.threadId = threadId;
        this.url = downLoadObj.Url;
        this.downLoadObj = downLoadObj;
    }

    public void Receive()
    {
        string fileName = downLoadObj.FileNames[threadId];
        var buffer = new byte[buffSize];
        int readSize = 0;
        FileStream fs = new FileStream(fileName, System.IO.FileMode.Create);
        Stream ns = null;

        try
        {
            request.AddRange(downLoadObj.StartPos[threadId], downLoadObj.StartPos[threadId] + downLoadObj.FileSize[threadId]);
            ns = request.GetResponse().GetResponseStream();
            readSize = ns.Read(buffer, 0, buffSize);
            showLog("线程[" + threadId.ToString() + "] 正在接收 " + readSize);
            while (readSize > 0)
            {
                fs.Write(buffer, 0, readSize);
                readSize = ns.Read(buffer, 0, buffSize);
                showLog("线程[" + threadId.ToString() + "] 正在接收 " + readSize);
            }
            fs.Close();
            ns.Close();
        }
        catch (Exception er)
        {
            Debug.LogError(er.Message);
            fs.Close();
        }
        showLog("线程[" + threadId.ToString() + "] 结束!");
        downLoadObj.ThreadStatus[threadId] = true;
    }

    private void showLog(string processing)
    {
        Debug.Log(processing);
    }
}