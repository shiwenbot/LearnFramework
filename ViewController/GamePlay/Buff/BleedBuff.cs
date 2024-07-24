using System;
using UnityEngine;

namespace ShootGame
{
    public class BleedBuff : BuffBase
    {
        private float m_bleedDamage;
        private float bleedDuration;
        private float bleedTickInterval;
        private int m_bleedStack;
        private int maxBleedStack;
        private bool m_isCaster;

        public int BleedStack
        {
            get { return m_bleedStack; }
            private set { m_bleedStack = value;}
        }

        public float BleedDamage
        {
            get { return m_bleedDamage; }
            private set { m_bleedDamage = value; }
        }

        #region 生命周期函数
        public override void OnInit()
        {
            ResetParams();
        }
        public override void OnEnter()
        {
            m_buffState = BuffState.Active;         
        }

        public override void OnUpdate(float deltatime)
        {
            //记录buff持续时间
            bleedDuration -= deltatime;
            bleedTickInterval -= deltatime;
            if ( bleedDuration <= 0 ) { OnExit(); }
            if(bleedTickInterval <= 0)
            {
                m_buffState = BuffState.ActiveReady;
                bleedTickInterval = BuffParams.BleedTickInterval;
            }
            if (m_bleedStack < 5) return;
            //当血怒叠到5层的时候，发送事件给BuffComponent
            EventManager.Instance.SendEvent<BuffBase, bool>("NoxianMightInit", new NoxianMightBuff(), true);
        }
        public override void OnExit()
        {           
            ResetParams();
            EventManager.Instance.SendEvent<int>("OnBleedBuffExit", 0);
        }
        public override void OnRefresh()
        {
            bleedDuration = BuffParams.BleedDuration;
            m_bleedStack = Mathf.Min(5, ++m_bleedStack);
            m_bleedDamage++;
        }
        public override void OnDestroy()
        {
            throw new NotImplementedException();
        }
        #endregion

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        protected override void ResetParams()
        {
            m_bleedDamage = BuffParams.BleedDamage;
            bleedDuration = BuffParams.BleedDuration;
            bleedTickInterval = BuffParams.BleedTickInterval;
            maxBleedStack = BuffParams.MaxBleedStack;

            m_bleedStack = 1;
            m_buffState = BuffState.InActive;
        }

        public override void Initialize(bool isCaster)
        {
            m_isCaster = isCaster;
        }
    }
}
