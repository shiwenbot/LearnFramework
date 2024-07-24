using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    public string ItemName;
    public Sprite itemImage;
    public int itemCount;
    [TextArea] public string itemInfo;

    public bool equipable; 
}
