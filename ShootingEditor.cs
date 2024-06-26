using QFramework;

namespace ShootGame
{
    public class ShootingEditor : Architecture<ShootingEditor>
    {
        protected override void Init()
        {
            RegisterSystem<IStateSystem>(new StateSystem());
            RegisterModel<IPlayerModel>(new PlayerModel());
            RegisterSystem<IGunSystem>(new GunSystem());
            RegisterSystem<ITimeSystem>(new TimeSystem());
            RegisterModel<IGunConfigModel>(new GunConfigModel());
            RegisterSystem<ReferencePoolSystem>(new ReferencePool());
            RegisterModel<IInventoryModel>(new InventoryModel());
        }
    }
}
