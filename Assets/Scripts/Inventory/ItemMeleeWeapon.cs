using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ItemMeleeWeapon : ItemPickable
{
    void Start()
    {
        MakeSureRigidbodyExistsAndIsSetupCorrectly();

        itemType = ItemType.MeleeWeapon;
    }
}
