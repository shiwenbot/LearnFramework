using QFramework;
using UnityEngine;

namespace ShootGame
{
    public class KillEnemyCommand : AbstractCommand
    {
        /// <summary>
        /// 1. 击杀数增加
        /// 2. 有概率补给子弹
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
