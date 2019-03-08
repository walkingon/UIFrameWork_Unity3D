using FSM;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleState : FSMstate
{

    public BattleState()
    {
        mStateId = SceneFSMstateID.Battle;
    }

    public override void OnEnterState(Dictionary<string, object> paramMap)
    {
        SceneManager.LoadScene("Battle");

        ViewManager.getInstance().OpenView("ViewBattle", true, null);
    }

    public override void OnLeaveState()
    {

    }

    public override void OnUpdateState()
    {

    }
}