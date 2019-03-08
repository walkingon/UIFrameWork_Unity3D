using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ViewController
{
    /// <summary>
    /// Base view controller.
    /// </summary>
    public abstract class BaseViewController
    {

        //  for child class and ViewManager
        public GameObject view = null;

        public abstract void OnOpen(object data);

        public virtual void OnClose() { }

        public virtual void OnHide() { }

        public virtual void OnShow() { }

        public virtual void OnPause() { }

        public virtual void OnResume() { }

        public virtual void OnReceiveMessage(object msg) { }


        // for ViewManager
        public string name = "";
        public bool isFullScreen = false;
        public bool isHided = false;
        public bool isPaused = false;
        public int instanceId = 0;

        public void Close()
        {
            OnClose();
            UnityEngine.Object.Destroy(view);
        }

        public void Hide()
        {
            view.SetActive(false);
            isHided = true;
            OnHide();
        }

        public void Show()
        {
            view.SetActive(true);
            isHided = false;
            OnShow();
        }

        public void Pause()
        {
            isPaused = true;
            OnPause();
        }

        public void Resume()
        {
            isPaused = false;
            OnResume();
        }

        public void ShowBlackBg(bool isShow)
        {

        }

        public void PlayOpenAnim(Delegate callback)
        {

        }

        public void PlayCloseAnim(Delegate callback)
        {

        }
    }
}


