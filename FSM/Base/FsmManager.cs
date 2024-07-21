using System.Collections.Generic;

namespace ShootGame
{
    public class FsmManager
    {
        private readonly Dictionary<TypeNamePair, FsmBase> m_Fsms;
        private readonly List<FsmBase> m_TempFsms;

        private static FsmManager m_Instance;
        private FsmManager()
        {
            m_Fsms = new Dictionary<TypeNamePair, FsmBase>();
            m_TempFsms = new List<FsmBase>();
        }
        public static FsmManager Instance
        {
            get
            {
                if (m_Instance == null) m_Instance = new();
                return m_Instance;
            }
        }

        public IFsm<T> CreateFsm<T>(string name, T owner, List<FsmState<T>> states) where T : class
        {
            TypeNamePair typeNamePair = new TypeNamePair(typeof(T), name);
            Fsm<T> fsm = Fsm<T>.Create(owner, states);
            m_Fsms.Add(typeNamePair, fsm);
            return fsm;
        }

        public void Update()
        {
            m_TempFsms.Clear();
            if (m_Fsms.Count <= 0)
            {
                return;
            }

            foreach (KeyValuePair<TypeNamePair, FsmBase> fsm in m_Fsms)
            {
                m_TempFsms.Add(fsm.Value);
            }

            foreach (FsmBase fsm in m_TempFsms)
            {               
                fsm.Update();
            }
        }
    }
}