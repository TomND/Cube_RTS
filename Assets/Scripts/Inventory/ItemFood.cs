using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemFood : ItemPickable
{
    public int HungerGain;

	void Start()
	{
        MakeSureRigidbodyExistsAndIsSetupCorrectly();

        itemType = ItemType.Food;
	}
}
