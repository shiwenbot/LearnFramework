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
        TData GetData<TData>(string name) where TData : Variable;
        Variable GetData(string name);
        void SetData<TData>(string name, TData data) where TData : Variable;
        void SetData(string name, Variable data);
    }
}