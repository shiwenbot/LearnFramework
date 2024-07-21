using System;

namespace ShootGame
{
    public class NoxianMightBuff : BuffBase
    {
        bool m_isCaster;

        float m_mightDuration;
        float m_attackValueBonus;
        public NoxianMightBuff(bool isCaster)
        {
            m_isCaster = isCaster;
            m_buffState = BuffState.InActive;
            m_mightDuration = BuffParams.MightDuration;
            m_attackValueBonus = BuffParams.AttackValueBonus;
        }

        public override void OnInit()
        {
            
        }
        public override void OnEnter()
        {
            EventManager.Instance.RegisterEvent<string>("NoxianMight", OnMaxStack);
        }
        public override void OnUpdate(float deltatime)
        {
            throw new NotImplementedException();
        }

        public override void OnExit()
        {
            throw new NotImplementedException();
        }

        private void OnMaxStack(string eventName)
        {
            //把状态设置为active即可，会在system处理增加攻击力/在头上挂血怒标志等逻辑
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override void OnDestroy()
        {
            throw new NotImplementedException();
        }

        //TODO: 平A可以续血怒时间
        public override void OnRefresh()
        {
            throw new NotImplementedException();
        }


    }
}
