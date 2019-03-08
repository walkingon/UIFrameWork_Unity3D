using UnityEngine;
using UnityEngine.UI;

namespace ViewController
{
    public class ViewHome : BaseViewController
    {

        public override void OnOpen(object data)
        {
            view = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Views/Home/ViewHome"));

            init();
        }

        void init()
        {
            initBtnClick();
        }

        void initBtnClick()
        {
            Button btnBack, btnTest, btnNext;
            btnBack = view.transform.Find("left/btnBack").GetComponent<Button>();
            btnTest = view.transform.Find("mid/btnTest").GetComponent<Button>();
            btnNext = view.transform.Find("right/btnNext").GetComponent<Button>();

            btnBack.onClick.AddListener(OnBackClick);
            btnTest.onClick.AddListener(() => OnTestClick("hhh"));
            btnNext.onClick.AddListener(() => OnNextClick(100));
        }

        void OnBackClick()
        {
            Debug.Log("点击返回");
        }

        void OnTestClick(string msg)
        {
            Debug.Log("点击测试" + msg);
        }

        void OnNextClick(int no)
        {
            Debug.Log("点击下一页" + no);
            ViewManager.getInstance().CloseView("ViewHome");
            ViewManager.getInstance().SwitchScene(SceneFSMstateID.Battle, null);
        }
    }
}

