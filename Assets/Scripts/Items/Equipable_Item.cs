using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipable_Item : Item
{
    public SlotType Slot { get; set; }

    public abstract void Equip();
}
