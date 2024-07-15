using System;

namespace ShootGame
{
    public interface IFsm<T> where T : class
    {
        T Owner { get; }
        FsmState<T> CurrentState {  get; }
        void Start<TState>() where TState : FsmState<T>;
        void Start(Type stateType);
        FsmState<T> GetState(Type stateType);
        
    }
}