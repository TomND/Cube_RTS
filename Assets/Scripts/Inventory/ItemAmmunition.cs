using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemAmmunition : ItemPickable
{
    public int AmmoCount;
    public ItemPickable[] WeaponCompatabilities;
    [HideInInspector]
    public bool Invested = false;

    void Start()
    {
        MakeSureRigidbodyExistsAndIsSetupCorrectly();
        itemType = ItemType.Ammunition;
    }

    public void RemoveBullet(int x) {
        this.AmmoCount -= x;
        if (this.AmmoCount <= 0) {
            Destroy(gameObject);
        }
    }

}
