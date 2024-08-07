using System.Collections.Generic;
using UnityEngine;

namespace ShootGame
{
    public class ProcedureManager : MonoBehaviour
    {
        protected List<FsmState<ProcedureManager>> stateList;
        private IFsm<ProcedureManager> fsm;

        private void Start()
        {
            stateList = new List<FsmState<ProcedureManager>> { new LaunchState(), new HotFixState(), new GameState() };
            fsm = FsmManager.Instance.CreateFsm("ProcedureManager", this, stateList);
            //fsm.Start<LaunchState>();
            fsm.Start<LaunchState>();
        }
    }
}