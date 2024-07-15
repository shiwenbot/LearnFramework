using UnityEngine;

namespace ShootGame
{
    public class IdleState : FsmState<Player>
    {
        private static KeyCode[] MOVE_COMMANDS = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };
        public override void OnInit(IFsm<Player> fsm) { base.OnInit(fsm); }
        public override void OnEnter(IFsm<Player> fsm) 
        { 
            base.OnEnter(fsm);
            Debug.Log("In IdleState");
        }
        public override void OnUpdate(IFsm<Player> fsm)
        {
            base.OnUpdate(fsm);

            if (Input.GetKeyDown(KeyCode.M))
            {
                Debug.Log("Change State");
                ChangeState<MoveState>(fsm);
            }
        }
        public override void OnLeave(IFsm<Player> fsm) 
        {
            Debug.Log("Leave IdleState");
        }
        public override void OnDestroy(IFsm<Player> fsm) { }
    }
}