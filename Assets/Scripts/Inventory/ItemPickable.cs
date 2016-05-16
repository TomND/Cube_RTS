using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public abstract class ItemPickable : MonoBehaviour {
    [HideInInspector]
    public ItemType itemType;
    public Texture2D ItemIcon;
    public bool Stackable;
    public int Quantity = 1;
    public bool ignoreRaycast = false;

    [Multiline]
    public string Description;
    public Transform ItemBody;
    public int MaxStack;
    
    
    public ActionType actionType;

    public enum ActionType
    {
        Nothing,
        Eat,
        Drink,
        Bandage

    }


    public Transform heldPreset;

    protected void MakeSureRigidbodyExistsAndIsSetupCorrectly()
    {
        if (gameObject.GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>().drag = 3f;
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().drag = 3f;
        }
    }
}

public enum ItemType {

    Block,
    Bag,
    Apparel,
    Ammunition,
    Telescopes,
    ProjectileWeapon,
    MeleeWeapon,
    Food,
    Drink,
    Medical,
    Buildable,
    Misc

}