using System;
using UnityEngine;

namespace ShootGame
{
    public class BleedBuff : BuffBase
    {
        private float bleedDamage;
        private float bleedDuration;
        private float bleedTickInterval;
        private int bleedStack;
        private int maxBleedStack;

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
            if(bleedTickInterval <= 0) { m_buffState = BuffState.ActiveReady; }
            //当血怒叠到5层的时候，发送事件
            if (maxBleedStack < 5) return;
            EventManager.Instance.SendEvent("NoxianMight", "");
        }
        public override void OnExit()
        {           
            ResetParams();
        }
        public override void OnRefresh()
        {
            throw new NotImplementedException();
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

        private void ResetParams()
        {
            bleedDamage = BuffParams.BleedDamage;
            bleedDuration = BuffParams.BleedDuration;
            bleedTickInterval = BuffParams.BleedTickInterval;
            maxBleedStack = BuffParams.MaxBleedStack;

            bleedStack = 0;
            m_buffState = BuffState.InActive;
        }
    }
}
