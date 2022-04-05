using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Armor_Chest", menuName = "Item/Armor/Chest")]
public class Armor_Chest : Armor
{
    public Armor_Chest()
    {
        //DEFAULT PROPERTY SETUP
        Slot = SlotType.CHEST;
    }
}
