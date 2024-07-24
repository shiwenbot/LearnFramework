using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootGame
{
    public class OnTriggerAddItem : MonoBehaviour
    {
        public Item thisItem;
        public Inventory playerInventory;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                AddItem();
                Destroy(gameObject);
            }
        }

        private void AddItem()
        {
            if(!playerInventory.items.Contains(thisItem))
            {
                playerInventory.items.Add(thisItem);
            }
            else
            {
                thisItem.itemCount++;
            }
        }
    }
}
