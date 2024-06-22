using QFramework;

namespace ShootGame
{
    public class ReloadCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var currentGun = this.GetSystem<IGunSystem>().CurrentGun;

            var gunConfigItem = this.GetModel<IGunConfigModel>().GetItemByName(currentGun.GunName.Value);
            //��ȡ��Ҫ���ӵ�����
            var needBulletCount = gunConfigItem.BulletMaxCount - currentGun.BulletLeft.Value;

            if(needBulletCount > 0)
            {
                //����ӵ����
                if(currentGun.BulletOutGun.Value > 0)
                {                   
                    // ״̬�л�
                    currentGun.GunState.Value = GunState.Reload;

                    // ״̬�л�
                    this.GetSystem<ITimeSystem>().AddDelayTask(gunConfigItem.ReloadSeconds, () =>
                    {
                        //����ӵ�����㹻��������
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
