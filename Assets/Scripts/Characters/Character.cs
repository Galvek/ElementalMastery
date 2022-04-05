using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    public float MaxEnergy { get; set; }
    public float Energy { get; set; }

    public float Stamina { get; set; }
    public float Strength { get; set; }
    public float Intellegence { get; set; }
    public float Dexterity { get; set; }

    public float NatureResistance { get; set; }
    public float WaterResistance { get; set; }
    public float FireResistance { get; set; }
}
