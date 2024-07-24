namespace ShootGame
{
    public sealed class FsmComponent : FrameworkComponent
    {
        private FsmManager m_FsmManager = null;
        protected override void Awake()
        {
            base.Awake();
            m_FsmManager = FsmManager.Instance;
        }

        protected override void Update()
        {
            base.Update();
            if (m_FsmManager != null)
            {               
                m_FsmManager.Update();
            }
        }
    }
}