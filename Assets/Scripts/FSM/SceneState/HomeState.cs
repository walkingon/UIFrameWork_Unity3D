using FSM;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeState : FSMstate
{

    public HomeState()
    {
        mStateId = SceneFSMstateID.Home;
    }

    public override void OnEnterState(Dictionary<string, object> paramMap)
    {
        SceneManager.LoadScene("Home");

        ViewManager.getInstance().OpenView("ViewHome", true, null);
    }

    public override void OnLeaveState()
    {

    }

    public override void OnUpdateState()
    {

    }
}