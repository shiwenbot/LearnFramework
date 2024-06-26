using QFramework;
namespace ShootGame
{
    public class CurrentInventoryQuery : IBelongToArchitecture, ICanGetModel
    {
        public Inventory Do()
        {
            var inventoryModel = this.GetModel<IInventoryModel>();
            return inventoryModel.GetInventory();
        }
        public IArchitecture GetArchitecture()
        {
            return ShootingEditor.Interface;
        }
    }
}