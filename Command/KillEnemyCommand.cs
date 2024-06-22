using QFramework;
using UnityEngine;

namespace ShootGame
{
    public class KillEnemyCommand : AbstractCommand
    {
        /// <summary>
        /// 1. ��ɱ������
        /// 2. �и��ʲ����ӵ�
        /// </summary>
        protected override void OnExecute()
        {
            this.GetSystem<IStateSystem>().killCount.Value++;
            if(Random.Range(0, 100) >= 20)
            {
                this.GetSystem<IGunSystem>().CurrentGun.AddBullet();
            }
            
        }
    }
}
