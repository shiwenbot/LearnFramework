namespace ShootGame
{
    public class GameState : FsmState<ProcedureManager>
    {
        IFsm<ProcedureManager> m_fsm;

        public override void OnInit(IFsm<ProcedureManager> fsm)
        {
            base.OnInit(fsm);
            m_fsm = fsm;
        }

        public override void OnEnter(IFsm<ProcedureManager> fsm)
        {
            base.OnEnter(fsm);
            GameSceneManager.Instance.LoadScene();
        } 
    }
}