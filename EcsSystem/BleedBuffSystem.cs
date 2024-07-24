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
        ComponentArray<HealthBarComponent> healthBarComponentArray;
        public BleedBuffSystem()
        {
            buffComponentArray = GameEntry.GetComponent<EcsComponentManager>().GetArray<BuffComponent>();
            healthBarComponentArray = GameEntry.GetComponent<EcsComponentManager>().GetArray<HealthBarComponent>();
            EventManager.Instance.RegisterEvent<Entity>("AddEntity", AddEntity);
        }
        public override void Tick()
        {
            foreach (var entity in m_entities)
            {
                BleedBuff buff = buffComponentArray.GetComponent(entity).GetBuff<BleedBuff>();
                if (buff == null || buff.m_buffState != BuffState.ActiveReady) continue;
                //根据buff更新UI
                healthBarComponentArray.GetComponent(entity).UpdateHealthBar(buff.BleedDamage, buff.BleedStack);
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
