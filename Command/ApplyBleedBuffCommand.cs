using QFramework;
using UnityEngine;

namespace ShootGame
{
    public class ApplyBleedBuffCommand : AbstractCommand
    {
        private Entity m_caster;
        private Entity m_target;

        public ApplyBleedBuffCommand(Entity caster)
        {
            m_caster = caster;
            //m_target = target;
        }

        protected override void OnExecute()
        {
            //获取到的这个array存了所有对象的BuffComponent
            var buffArray = GameEntry.GetComponent<EcsComponentManager>().GetArray<BuffComponent>();
            //从array中分别获取到caster和target的BuffComponent，然后更新buff状态
            buffArray.GetComponent(m_caster).CommitBuff<BleedBuff>();
        }
    }
}
