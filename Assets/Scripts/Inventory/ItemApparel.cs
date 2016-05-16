using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemApparel : ItemPickable
{
    void Start()
    {
        MakeSureRigidbodyExistsAndIsSetupCorrectly();

        itemType = ItemType.Apparel;
    }
}
