using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Food will go into the food slot on the action bar.
/// Will have a use function override
/// </summary>
[CreateAssetMenu(fileName = "New_Food", menuName = "Item/Consumable/Food")]
public class Food : Consumable
{
    public Food()
    {

    }

    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}
