using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string Name { get; set; }
    public Texture2D Icon { get; set; }
    public GameObject WorldObject { get; set; }
    public RarityType Rarity { get; set; }

    //TODO determine where best to put this (not all items are elemental items (for example, food and potions)
    public enum ElementType
    {
        NOT_SET,
        NATURE,
        WATER,
        FIRE
    }

    public enum RarityType
    {
        COMMON,
        UNCOMMON,
        RARE,
        EPIC,
        LEGENDARY
    }

    public enum SlotType
    {
        HELM,
        CHEST,
        LEGS,
        WEAPON
    }
}
