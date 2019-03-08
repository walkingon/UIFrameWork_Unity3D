using System;
using UnityEngine;
using System.Collections.Generic;

namespace FSM
{
    /// <summary>
    /// 状态
    /// </summary>
    public abstract class FSMstate
    {
        protected Enum mStateId;

        /// <summary>
        /// Gets the state identifier.
        /// </summary>
        /// <returns>The state identifier.</returns>
        public Enum GetStateId()
        {
            return mStateId;
        }

        /// <summary>
        /// Ons the state of the enter.
        /// </summary>
        /// <param name="paramMap">Parameter map.</param>
        public abstract void OnEnterState(Dictionary<string, object> paramMap);

        /// <summary>
        /// Ons the state of the leave.
        /// </summary>
        public abstract void OnLeaveState();

        /// <summary>
        /// Ons the state of the update.
        /// </summary>
        public abstract void OnUpdateState();
    }


    /// <summary>
    /// 状态机
    /// </summary>
    public abstract class FSM : MonoBehaviour
    {
        private FSMstate mState = null;
        private Dictionary<Enum, FSMstate> stateMap = new Dictionary<Enum, FSMstate>();

        /// <summary>
        /// Gets the state of the current.
        /// </summary>
        /// <returns>The current state.</returns>
        public FSMstate GetCurrentState()
        {
            return mState;
        }

        /// <summary>
        /// Registers the state.
        /// </summary>
        /// <param name="state">State.</param>
        public void RegisterState(FSMstate state)
        {
            if(state == null)
            {
                Debug.LogError("FSM 注册失败，state为null");
                return;
            }
            stateMap[state.GetStateId()] = state;
        }

        /// <summary>
        /// Switchs the state.
        /// </summary>
        /// <returns><c>true</c>, if state was switched, <c>false</c> otherwise.</returns>
        /// <param name="stateId">State identifier.</param>
        /// <param name="enterParamMap">Enter parameter map, can be null</param>
        public bool SwitchState(Enum stateId, Dictionary<string, object> enterParamMap)
        {
            if (!stateMap.ContainsKey(stateId))
            {
                Debug.LogError("FSM 切换失败，未注册的状态 : " + stateId);
                return false;
            }
            if (mState != null)
            {
                if(mState.GetStateId() == stateId)
                {
                    Debug.Log("FSM 状态无需改变");
                    return true;
                }
                mState.OnLeaveState();
            }
            mState = stateMap[stateId];
            mState.OnEnterState(enterParamMap);

            return true;
        }

        private void Update()
        {
            if(mState != null)
            {
                mState.OnUpdateState();
            }
        }

        /// <summary>
        /// Logs the state map.
        /// </summary>
        public void LogStateMap()
        {
            Debug.Log(GetType().Name + "状态列表:");
            foreach(Enum k in stateMap.Keys)
            {
                Debug.Log(string.Format(" {0} -  {1}", k, stateMap[k]));
            }
        }
    }
}