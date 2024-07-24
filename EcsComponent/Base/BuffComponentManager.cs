using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShootGame
{
    public class BuffComponentManager : FrameworkComponent
    {
        private List<BuffComponent> m_buffComponents;
        protected override void Awake()
        {
            base.Awake();
            EventManager.Instance.RegisterEvent<BuffComponent>("Initialize buffComponent", AddBuffComponent);
            m_buffComponents = new List<BuffComponent>();
        }

        private void AddBuffComponent(BuffComponent component)
        {
            m_buffComponents.Add(component);
        }

        protected override void Update()
        {
            float deltaTime = Time.deltaTime;
            foreach (var component in m_buffComponents)
            {
                component.Tick(deltaTime);
            }
        }

        public void AddBuff(Entity entity)
        {

        }     
    }
}
