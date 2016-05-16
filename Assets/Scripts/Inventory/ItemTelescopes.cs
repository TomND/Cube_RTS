using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemTelescopes : ItemPickable
{
    void Start()
    {
        MakeSureRigidbodyExistsAndIsSetupCorrectly();

        itemType = ItemType.Telescopes;
    }
}
