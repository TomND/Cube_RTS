using UnityEngine;
using System.Collections;

public class ItemFunction : MonoBehaviour {

    public static void Bag(ItemSprite sprite) {
        ItemBag item = sprite.Reference.GetComponent<ItemBag>();
        int bagSpace = item.BagStorageLimit;
        UI.root.InventorySlotGrid.GetComponent<InventoryGridManager>().ChangeActiveInventorySlots(bagSpace);
    }

    public static void Apparel(ItemSprite sprite) {

    }

    public static void Ammunition(ItemSprite sprite) {

    }

    public static void Telescopes(ItemSprite sprite) {
        if (sprite.QSReference != null) {

        } else {
            sprite.TransferToQuickSlot();
        }
    }

    public static void ProjectileWeapon(ItemSprite sprite) {
        if (sprite.QSReference != null) {

        } else {
            sprite.TransferToQuickSlot();
        }
    }

    public static void MeleeWeapon(ItemSprite sprite) {
        if (sprite.QSReference != null) {

        } else {
            sprite.TransferToQuickSlot();
        }
    }
        
    public static void Medical(ItemSprite sprite) {
        ItemMedical item = sprite.Reference.GetComponent<ItemMedical>();
        int healthGain = item.HealthGain;
        UI.root.playerStats.healHealth(healthGain);
        sprite.QuantityChangeImplicit(-1);
    }
        

    public static void Misc(ItemSprite sprite) {
        // Test Purposes
    }

}

/*
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
*/