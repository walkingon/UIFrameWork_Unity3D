using FSM;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingState : FSMstate
{

    public LoadingState()
    {
        mStateId = SceneFSMstateID.Loading;
    }

    public override void OnEnterState(Dictionary<string, object> paramMap)
    {
        SceneManager.LoadScene("Loading");

        ViewManager.getInstance().OpenView("ViewLoading", true, null);
    }

    public override void OnLeaveState()
    {

    }

    public override void OnUpdateState()
    {

    }
}