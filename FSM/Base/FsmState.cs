namespace ShootGame
{
    public abstract class FsmState<T> where T : class
    {
        public FsmState() { }
        public virtual void OnInit(IFsm<T> fsm) { }
        public virtual void OnEnter(IFsm<T> fsm) { }
        public virtual void OnUpdate(IFsm<T> fsm) { }
        public virtual void OnLeave(IFsm<T> fsm) { }
        public virtual void OnDestroy(IFsm<T> fsm) { }
        public void ChangeState<TState>(IFsm<T> fsm) where TState : FsmState<T>
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            fsmImplement.ChangeState<TState>();
        }
    }
}