using QFramework;

namespace ShootGame
{
    public class ReloadCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var currentGun = this.GetSystem<IGunSystem>().CurrentGun;

            var gunConfigItem = this.GetModel<IGunConfigModel>().GetItemByName(currentGun.GunName.Value);
            //获取需要的子弹数量
            var needBulletCount = gunConfigItem.BulletMaxCount - currentGun.BulletLeft.Value;

            if(needBulletCount > 0)
            {
                //如果子弹库存
                if(currentGun.BulletOutGun.Value > 0)
                {                   
                    // 状态切换
                    currentGun.GunState.Value = GunState.Reload;

                    // 状态切回
                    this.GetSystem<ITimeSystem>().AddDelayTask(gunConfigItem.ReloadSeconds, () =>
                    {
                        //如果子弹库存足够填满弹夹
                        if (currentGun.BulletOutGun.Value > needBulletCount)
                        {
                            currentGun.BulletOutGun.Value -= needBulletCount;
                            currentGun.BulletLeft.Value += needBulletCount;
                        }
                        else
                        {
                            currentGun.BulletLeft.Value += currentGun.BulletOutGun.Value;
                            currentGun.BulletOutGun.Value = 0;
                        }
                        currentGun.GunState.Value = GunState.Idle;
                    });
                }
            }
        }
    }
}
