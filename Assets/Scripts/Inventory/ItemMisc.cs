using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemMisc : ItemPickable
{
    void Start()
    {
        MakeSureRigidbodyExistsAndIsSetupCorrectly();

        itemType = ItemType.Misc;
    }
}
