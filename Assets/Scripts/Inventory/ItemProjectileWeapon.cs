using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemProjectileWeapon : ItemPickable
{
    void Start()
    {
        MakeSureRigidbodyExistsAndIsSetupCorrectly();

        itemType = ItemType.ProjectileWeapon;
    }
}
