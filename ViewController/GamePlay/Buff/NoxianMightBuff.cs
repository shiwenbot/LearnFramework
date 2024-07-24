using System;
using UnityEngine;

namespace ShootGame
{
    public class NoxianMightBuff : BuffBase
    {
        bool m_isCaster;

        float m_mightDuration;
        float m_attackValueBonus;
        public NoxianMightBuff()
        {           
            m_buffState = BuffState.InActive;
            m_mightDuration = BuffParams.MightDuration;
            m_attackValueBonus = BuffParams.AttackValueBonus;
        }

        public override void Initialize(bool isCaster)
        {
            m_isCaster = isCaster;
        }

        public override void OnInit()
        {
            
        }
        public override void OnEnter()
        {
            m_buffState = BuffState.Active;
            EventManager.Instance.SendEvent<Color>("OnNoxianMightChanged", Color.red);
        }
        public override void OnUpdate(float deltatime)
        {
            m_mightDuration -= deltatime;
            if(m_mightDuration < 0) { OnExit(); }
        }

        public override void OnExit()
        {
            m_buffState = BuffState.InActive;
            EventManager.Instance.SendEvent<Color>("OnNoxianMightChanged", Color.white);
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
            
        }

        protected override void ResetParams()
        {
            m_buffState = BuffState.InActive;
            m_mightDuration = BuffParams.MightDuration;
            m_attackValueBonus = BuffParams.AttackValueBonus;
        }
    }
}
