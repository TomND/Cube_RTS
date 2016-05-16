using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemBag : ItemPickable
{
    public int BagStorageLimit;
    public int StorageLimit;
    public ItemPickable[] StorageObjects;

	void Start ()
	{
        MakeSureRigidbodyExistsAndIsSetupCorrectly();

        itemType = ItemType.Bag;
	}
}
