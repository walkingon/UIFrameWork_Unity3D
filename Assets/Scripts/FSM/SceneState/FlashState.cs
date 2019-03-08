using FSM;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlashState : FSMstate
{

    public FlashState()
    {
        mStateId = SceneFSMstateID.Flash;
    }

    public override void OnEnterState(Dictionary<string, object> paramMap)
    {
        SceneManager.LoadScene("Flash");

        ViewManager.getInstance().OpenView("ViewFlash", true, null);
    }

    public override void OnLeaveState()
    {

    }

    public override void OnUpdateState()
    {

    }
}