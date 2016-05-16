using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDrink : ItemPickable
{
    public int ThirstGain;

    void Start()
    {
        MakeSureRigidbodyExistsAndIsSetupCorrectly();

        itemType = ItemType.Drink;
    }
}
