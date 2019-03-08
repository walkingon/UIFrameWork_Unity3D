using FSM;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginState : FSMstate
{

    public LoginState()
    {
        mStateId = SceneFSMstateID.Login;
    }

    public override void OnEnterState(Dictionary<string, object> paramMap)
    {
        SceneManager.LoadScene("Login");

        ViewManager.getInstance().OpenView("ViewLogin", true, null);
    }

    public override void OnLeaveState()
    {

    }

    public override void OnUpdateState()
    {

    }
}