using QFramework;

namespace ShootGame
{
    public class SwitchGunCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetSystem<IGunSystem>()
                .SwitchGun();
        }
    }
}
