using System.Collections.Generic;

namespace ShootGame
{
    public sealed class SystemManagerComponent : FrameworkComponent
    {
        private static readonly LinkedList<SystemBase> m_systemList = new LinkedList<SystemBase>();

        protected override void Awake()
        {
            base.Awake();
            Create<BleedBuffSystem>();
        }

        private void Create<T>() where T : SystemBase, new()
        {
            T System = ReferencePool.Acquire<T>();
            m_systemList.AddLast(System);
        }

        private void Update()
        {
            foreach (var buffSystem in m_systemList)
            {
                buffSystem.Tick();
            }
        }
    }
}
