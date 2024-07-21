using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShootGame
{
    /// <summary>
    /// 仿诺手流血效果，涉及到扣血，血滴子UI效果
    /// 1. 根据流血层数扣血
    /// 2. 根据流血层数决定是否展示UI
    /// </summary>
    public class BleedBuffSystem : BuffSystemBase
    {
        ComponentArray<BuffComponent> buffComponentArray;
        public BleedBuffSystem()
        {
            buffComponentArray = GameEntry.GetComponent<EcsComponentManager>().GetArray<BuffComponent>();
            EventManager.Instance.RegisterEvent<Entity>("AddEntity", AddEntity);
        }
        public override void Tick()
        {
            foreach (var entity in m_entities)
            {
                BleedBuff buff = buffComponentArray.GetComponent(entity).GetBuff<BleedBuff>();
                if (buff == null || buff.m_buffState != BuffState.ActiveReady) continue;
                Debug.Log("成功了");
                buff.m_buffState = BuffState.Active;
            }
        }

        public void AddEntity(Entity entity)
        {
            m_entities.AddLast(entity);
        }
        public override void Clear()
        {
            
        }
    }
}
