using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemMedical : ItemPickable
{
    public int HealthGain;

    void Start()
    {
        MakeSureRigidbodyExistsAndIsSetupCorrectly();

        itemType = ItemType.Medical;
    }
}
