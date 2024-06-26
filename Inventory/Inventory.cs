using System.Collections.Generic;
using UnityEngine;

namespace ShootGame
{
    public class Inventory
    {
        public List<Item> itemList { get; private set; }

        public Inventory()
        {
            itemList = new List<Item>();
            while(itemList.Count < 8)
            {
                AddItem(new Item { itemType = Item.ItemType.Gun, name = "Gun", amount = 1 });
            }
            itemList[5].name = "GGGun";
        }

        public void AddItem(Item item)
        {        
            itemList.Add(item);
        }
    }
}