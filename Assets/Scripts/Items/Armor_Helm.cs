using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Armor_Helm", menuName = "Item/Armor/Helm")]
public class Armor_Helm : Armor
{
    public Armor_Helm()
    {
        //DEFAULT PROPERTY SETUP
        Slot = SlotType.HELM;
    }
}
