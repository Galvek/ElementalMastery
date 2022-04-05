using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Armor_Legs", menuName = "Item/Armor/Legs")]
public class Armor_Legs : Armor
{
    public Armor_Legs()
    {
        //DEFAULT PROPERTY SETUP
        Slot = SlotType.LEGS;
    }
}
