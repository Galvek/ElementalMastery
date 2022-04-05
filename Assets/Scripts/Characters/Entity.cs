using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : ScriptableObject
{
    public string Name { get; set; }
    public int Level { get; set; }

    public float MaxHealth { get; set; }
    public float Health { get; set; }
}
