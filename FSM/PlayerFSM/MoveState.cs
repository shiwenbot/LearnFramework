using ShootGame;
using UnityEngine;

namespace ShootGame
{
    public class MoveState : FsmState<Player>
    {
        public override void OnInit(IFsm<Player> fsm) { base.OnInit(fsm); }
        public override void OnEnter(IFsm<Player> fsm)
        {
            base.OnEnter(fsm);
            Debug.Log("In MoveState");
        }
        public override void OnUpdate(IFsm<Player> fsm)
        {
            base.OnUpdate(fsm);
        }
        public override void OnLeave(IFsm<Player> fsm)
        {
            Debug.Log("Leave IdleState");
        }
        public override void OnDestroy(IFsm<Player> fsm) { }
    }
}