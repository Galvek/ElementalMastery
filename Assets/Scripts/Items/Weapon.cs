using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipable_Item
{
    public int MinDamage { get; set; }
    public int MaxDamage { get; set; }
    public float AttackSpeed { get; set; }
    public float AttackRange { get; set; }
    public ElementType Element { get; set; }

    public Weapon()
    {
        //DEFAULT PROPERTY SETUP
        Slot = SlotType.WEAPON;
    }

    public override void Equip()
    {
        throw new System.NotImplementedException();
    }
}
