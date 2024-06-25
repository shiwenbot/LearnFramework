using UnityEngine;

namespace ShootGame
{
    public class Item
    {
        public enum ItemType
        {
            Gun,
            HealthPotion,
            ManaPotion,
            Coin
        }

        public ItemType itemType;
        public string name;
        public int amount;
    }
}
