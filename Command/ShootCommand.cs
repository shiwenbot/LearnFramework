using QFramework;

namespace ShootGame
{
    public class ShootCommand : AbstractCommand
    {
        // 这里采用static，防止每次开枪都new一个Command对象
        public static readonly ShootCommand Instance = new ShootCommand();

        //开枪后子弹数量减少
        protected override void OnExecute()
        {
            var GunSystem = this.GetSystem<IGunSystem>();
            GunSystem.CurrentGun.BulletLeft.Value--; //子弹数量减少
            GunSystem.CurrentGun.GunState.Value = GunState.Shooting; //枪支状态改为射击中
            var gunConfigItem = this.GetModel<IGunConfigModel>()
                .GetItemByName(GunSystem.CurrentGun.GunName.Value);

            var timeSystem = this.GetSystem<ITimeSystem>();

            //射击后，一段时间后枪支状态改为Idle
            timeSystem.AddDelayTask(1 / gunConfigItem.Frequency, () => 
            { 
                GunSystem.CurrentGun.GunState.Value = GunState.Idle;
                //如果子弹数量为0，且弹夹中还有子弹，则自动装弹
                if (GunSystem.CurrentGun.BulletLeft.Value == 0 &&
                        GunSystem.CurrentGun.BulletOutGun.Value > 0)
                {
                    this.SendCommand<ReloadCommand>();
                }
            });
        }
    }
}