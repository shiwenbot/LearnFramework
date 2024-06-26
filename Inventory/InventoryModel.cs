using QFramework;
using UnityEngine;

namespace ShootGame
{
    public interface IInventoryModel : IModel
    {
        public Inventory GetInventory();
        int GetLeftIndex();
        int GetRightIndex();
    }

    public class InventoryModel : AbstractModel, IInventoryModel
    {
        private Inventory inventory;
        private int leftIndex = 0, rightIndex = 0; //list的index要比container的child小1，因为child包含了一个预制体（在初始化完成后会被设置成inactive）        

        public void AddItem(Item item)
        {           
            inventory.AddItem(item);
        }

        public Inventory GetInventory()
        {
            return inventory;
        }

        public int GetLeftIndex()
        {
            return leftIndex;
        }

        public int GetRightIndex()
        {
            return rightIndex;
        }

        protected override void OnInit()
        {           
            inventory = new Inventory();           
        }
    }
}