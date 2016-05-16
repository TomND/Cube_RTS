using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemBuildable : ItemPickable
{
    
    public bool isOffice = false;
    public bool factionOwned = false;

    void Start()
    {
        MakeSureRigidbodyExistsAndIsSetupCorrectly();

        itemType = ItemType.Buildable;
    }
}
