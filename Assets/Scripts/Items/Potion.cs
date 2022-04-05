using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Potion will go into the potion slot on the action bar.
/// Will have a use function override
/// </summary>
[CreateAssetMenu(fileName = "New_Potion", menuName = "Item/Consumable/Potion")]
public class Potion : Consumable
{
    public Potion()
    {

    }

    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}
