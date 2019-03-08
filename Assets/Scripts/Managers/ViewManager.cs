using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ViewController;

/// <summary>
/// Scene fsm.
/// </summary>
public class SceneFSM : FSM.FSM{}


/// <summary>
/// View manager.
/// </summary>
public class ViewManager : MonoSingleton<ViewManager>
{
    private Canvas UIRoot = null;
    private GameObject EventSystem = null;

    /// <summary>
    /// 场景状态机.
    /// </summary>
    private SceneFSM sceneFSM = null;

    private List<BaseViewController> viewList = null;
    private GameObject tipRoot = null;
    private GameObject loadingAnimGO = null;

    public void Init()
    {
        viewList = new List<BaseViewController>();

        InitUIRoot();
        InitSceneFSM();
        InitLoadingAnimGO();
    }

    private void InitUIRoot()
    {
        UIRoot = GameObject.Find("UIRoot").GetComponent<Canvas>();
        EventSystem = GameObject.Find("EventSystem");

        DontDestroyOnLoad(UIRoot.gameObject);
        DontDestroyOnLoad(EventSystem);
    }

    private void InitSceneFSM()
    {
        sceneFSM = UIRoot.gameObject.AddComponent<SceneFSM>();
        sceneFSM.RegisterState(new LoadingState());
        sceneFSM.RegisterState(new FlashState());
        sceneFSM.RegisterState(new LoginState());
        sceneFSM.RegisterState(new HomeState());
        sceneFSM.RegisterState(new BattleState());
        //sceneFSM.LogStateMap();

        //切换到初始状态
        SwitchScene(SceneFSMstateID.Loading, null);
    }

    /// <summary>
    /// Switchs the scene.
    /// </summary>
    /// <param name="id">Identifier.</param>
    /// <param name="enterParams">Enter parameters.</param>
    public void SwitchScene(SceneFSMstateID id, Dictionary<string, object> enterParams)
    {
        sceneFSM.SwitchState(id, enterParams);
    }

    private void InitLoadingAnimGO()
    {

    }

    /// <summary>
    /// Turns the off screen click.
    /// </summary>
    /// <param name="showLoading">If set to <c>true</c> show loading.</param>
    public void TurnOffScreenClick(bool showLoading)
    {
        TurnOffScreenClick(showLoading, "");
    }

    /// <summary>
    /// Turns the off screen click.
    /// </summary>
    /// <param name="showLoading">If set to <c>true</c> show loading.</param>
    /// <param name="tip">Tip.</param>
    public void TurnOffScreenClick(bool showLoading, string tip)
    {

    }

    /// <summary>
    /// Turns the on screen click.
    /// </summary>
    public void TurnOnScreenClick()
    {

    }

    /// <summary>
    /// Refreshs the turn off screen click tip.
    /// </summary>
    /// <param name="tip">Tip.</param>
    public void RefreshTurnOffScreenClickTip(string tip)
    {

    }

    /// <summary>
    /// Opens the view.
    /// </summary>
    /// <param name="viewControlerName">View controler name.</param>
    /// <param name="isFullScreen">If set to <c>true</c> is full screen.</param>
    /// <param name="customData">Custom data.</param>
    public void OpenView(string viewControlerName, bool isFullScreen, object customData)
    {
        TurnOffScreenClick(true);

        Type ViewType = Type.GetType("ViewController." + viewControlerName);
        if(ViewType == null)
        {
            Debug.LogError(string.Format("打开界面 {0} 失败.", viewControlerName));
            return;
        }       
        BaseViewController view = Activator.CreateInstance(ViewType) as BaseViewController;
        view.name = viewControlerName;
        view.isFullScreen = isFullScreen;

        //历史界面暂停，如果打开的界面是全屏，历史界面进入后台
        foreach (BaseViewController oldView in viewList)
        {
            if (!oldView.isPaused)
            {
                oldView.Pause();
                Debug.Log(oldView.name + " ______PAUSED.");
            }
            if (isFullScreen)
            {
                if (!oldView.isHided)
                {
                    oldView.Hide();
                    Debug.Log(oldView.name + " ______HIDED.");
                }
            }
        }

        if (isFullScreen)
        {
            view.ShowBlackBg(true); 
        }
        else
        {
            view.ShowBlackBg(false);
        }

        view.OnOpen(customData);
        Tools.normalizedParent(view.view.transform, UIRoot.transform);

        Debug.Log(view.name + " ______OPENED.");

        TurnOnScreenClick();

        view.instanceId = viewList.Count + 1;
        viewList.Add(view);

    }

    /// <summary>
    /// Closes the view.
    /// </summary>
    /// <param name="viewControlerName">View controler name.</param>
    public void CloseView(string viewControlerName)
    {
        for(int i = viewList.Count - 1; i >= 0; i--)
        {
            BaseViewController mView = viewList[i];
            if(mView.name.Equals(viewControlerName))
            {
                mView.Close();
                Debug.Log(mView.name + " ______CLOSED.");
                viewList.Remove(mView);
                break;
            }
        }

        //前一个界面继续
        for (int i = viewList.Count - 1; i >= 0; i--)
        {
            BaseViewController mView = viewList[i];
            if(mView.isPaused)
            {
                mView.Resume();
                Debug.Log(mView.name + " ______RESUMED.");
                break;
            }
        }

        //往前查第一个全屏界面之上的历史界面回到前台
        for (int i = viewList.Count - 1; i >= 0; i--)
        {
            BaseViewController mView = viewList[i];
            if (mView.isHided)
            {
                mView.Show();
                Debug.Log(mView.name + " ______SHOWDED.");
            }
            if (mView.isFullScreen)
            {
                break;
            }
        }
    }

    /// <summary>
    /// Sends the view message.
    /// </summary>
    /// <returns><c>true</c>, if view message was sent, <c>false</c> otherwise.</returns>
    /// <param name="view">View.</param>
    /// <param name="msg">Message.</param>
    public bool SendViewMessage(BaseViewController view, object msg)
    {
        foreach (BaseViewController mview in viewList)
        {
            if(mview.Equals(view))
            {
                mview.OnReceiveMessage(msg);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Shows the tip.
    /// </summary>
    /// <param name="str">String.</param>
    public void ShowTip(string str)
    {
        ShowTip(str, 2);
    }

    /// <summary>
    /// Shows the tip.
    /// </summary>
    /// <param name="str">String.</param>
    /// <param name="duration">Duration.</param>
    public void ShowTip(string str, int duration)
    {
        ShowTip(str, duration, Color.white);
    }

    /// <summary>
    /// Shows the tip.
    /// </summary>
    /// <param name="str">String.</param>
    /// <param name="duration">Duration.</param>
    /// <param name="color">Color.</param>
    public void ShowTip(string str, int duration, Color color)
    {

    }
}
