using QFramework;

namespace ShootGame
{
    public class PickGunCommand : AbstractCommand
    {
        private readonly string mName;
        private readonly int mBulletInGun;
        private readonly int mBulletOutGun;

        public PickGunCommand(string name, int bulletInGun, int bulletOutGun)
        {
            mName = name;
            mBulletInGun = bulletInGun;
            mBulletOutGun = bulletOutGun;
        }

        protected override void OnExecute()
        {
            this.GetSystem<IGunSystem>()
                .PickGun(mName, mBulletInGun, mBulletOutGun);
            //this.GetSystem<IInventorySystem>().AddItem(new Item { itemType = Item.ItemType.Gun, name = "Gun", amount = 1 });
            //this.GetSystem<I_Inventory>().print();
        }
    }
}
