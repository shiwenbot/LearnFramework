using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShootGame
{
    public class Fsm<T> : FsmBase, IFsm<T>, IReference where T : class
    {
        private T m_Owner;
        private readonly Dictionary<Type, FsmState<T>> m_States;
        private FsmState<T> m_CurrentState;

        public T Owner { get { return m_Owner; } }
        public bool IsRunning {  get { return m_CurrentState != null; } }

        public Fsm()
        {
            m_Owner = null;
            m_States = new Dictionary<Type, FsmState<T>>();
        }

        public static Fsm<T> Create(T owner, List<FsmState<T>> states)
        {
            Fsm<T> fsm = ReferencePool.Acquire<Fsm<T>>();           
            fsm.m_Owner = owner;
            foreach (FsmState<T> state in states)
            {
                Type stateType = state.GetType();
                fsm.m_States.Add(stateType, state);
                state.OnInit(fsm);
            }
            return fsm;
        }
        public void Start<TState>() where TState : FsmState<T>
        {
            FsmState<T> state = GetState<TState>();
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                throw new Exception("FSM is running, can not start again.");
            }
            FsmState<T> state = GetState(stateType);
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        internal override void Update()
        {
            if (m_CurrentState == null)
            {
                return;
            }
            m_CurrentState.OnUpdate(this);
        }
        public FsmState<T> CurrentState {  get { return m_CurrentState; } }

        public TState GetState<TState>() where TState : FsmState<T>
        {
            FsmState<T> state = null;
            if (m_States.TryGetValue(typeof(TState), out state))
            {
                return (TState)state;
            }

            return null;
        }
        public FsmState<T> GetState(Type stateType)
        {
            FsmState<T> state = null;
            if (m_States.TryGetValue(stateType, out state))
            {
                Debug.Log("Get State Successful");
                return state;
            }
            Debug.Log("Fail to get State");
            return null;
        }

        public void Clear()
        {
            //Need to release
        }

        internal void ChangeState<TState>() where TState : FsmState<T>
        {
            ChangeState(typeof(TState));
        }

        internal void ChangeState(Type stateType)
        {
            FsmState<T> state = GetState(stateType);
            m_CurrentState.OnLeave(this);
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }
    }
}