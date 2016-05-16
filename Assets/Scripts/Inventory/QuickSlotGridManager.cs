using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuickSlotGridManager : MonoBehaviour {

    Color initColor;

    void Start() {
        initColor = transform.GetChild(0).gameObject.GetComponent<RawImage>().color;
    }

    public int selectedSlot = 0;
    public void changeSlot(int slot) {
        DisableInitial();
        selectedSlot = slot;
        EnableNew(slot);
        CheckWield();
    }

    public void CheckWield() {
        // Hold something infront

        ItemSprite sprite = transform.GetChild(selectedSlot).gameObject.GetComponent<QuickSlotManager>().Reference;
        if (sprite == null || sprite.Reference == null) { /*Debug.LogError("Sprite or SpriteReference is null");*/ return; }
        ItemPickable item = sprite.Reference.GetComponent<ItemPickable>();
        switch(item.itemType)
        {
            case ItemType.Ammunition:
                ItemPullOutFunction.Ammunition(sprite);
                break;
            case ItemType.Apparel:
                ItemPullOutFunction.Apparel(sprite);
                break;
            case ItemType.Bag:
                ItemPullOutFunction.Bag(sprite);
                break;
            case ItemType.Medical:
                ItemPullOutFunction.Medical(sprite);
                break;
            case ItemType.MeleeWeapon:
                ItemPullOutFunction.MeleeWeapon(sprite);
                break;
            case ItemType.Misc:
                ItemPullOutFunction.Misc(sprite);
                break;
            case ItemType.ProjectileWeapon:
                ItemPullOutFunction.ProjectileWeapon(sprite);
                break;
            case ItemType.Telescopes:
                ItemPullOutFunction.Telescopes(sprite);
                break;
        }
        if (transform.GetChild(selectedSlot).gameObject.GetComponent<QuickSlotManager>().Reference != null) {
            GameObject holdObject = transform.GetChild(selectedSlot).gameObject.GetComponent<QuickSlotManager>().Reference.Reference;
            holdObject.SetActive(true);

            if (holdObject.GetComponent<Collider>() != null && holdObject.GetComponent<MeshRenderer>() != null) {
                holdObject.GetComponent<Collider>().enabled = true;
                holdObject.GetComponent<MeshRenderer>().enabled = true;
            }
                
            if (holdObject.GetComponent<ItemPickable>().heldPreset != null) {
                holdObject.transform.localPosition = holdObject.GetComponent<ItemPickable>().heldPreset.localPosition;
                holdObject.transform.localEulerAngles = holdObject.GetComponent<ItemPickable>().heldPreset.localEulerAngles;
                //holdObject.transform.localScale = holdObject.GetComponent<ItemPickable>().heldPreset.localScale;
            } else {
                holdObject.transform.localPosition = new Vector3(0, 0, 0);
                holdObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                //holdObject.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            if (holdObject.GetComponent<ItemPickable>().itemType == ItemType.ProjectileWeapon) {
                //UI.root.playerInventory.gameObject.GetComponent<AnimationController>().UpdateHeld(holdObject, "hasGun");
                print("as hun");
            } else if (holdObject.GetComponent<ItemPickable>().itemType == ItemType.MeleeWeapon) {
                //UI.root.playerInventory.gameObject.GetComponent<AnimationController>().UpdateHeld(holdObject, "hasMelee");
            } else {
                //UI.root.playerInventory.gameObject.GetComponent<AnimationController>().UpdateHeld(holdObject, "hasEmptyRight");
            }
            Destroy(holdObject.GetComponent<Rigidbody>());
        }
    }

    private void DisableInitial() {
        GameObject QuickSlot = transform.GetChild(selectedSlot).gameObject;
        RawImage img = QuickSlot.GetComponent<RawImage>();
        img.color = initColor;
        QuickSlotManager manager = QuickSlot.GetComponent<QuickSlotManager>();
        if (manager.Reference != null)
        {
            ItemSprite sprite = manager.Reference;
            GameObject item = sprite.Reference.gameObject;
            ItemPickable pickable = item.GetComponent<ItemPickable>();
            switch (pickable.itemType)
            {
                case ItemType.Ammunition:
                    ItemPutAwayFunction.Ammunition(sprite);
                    break;
                case ItemType.Apparel:
                    ItemPutAwayFunction.Apparel(sprite);
                    break;
                case ItemType.Bag:
                    ItemPutAwayFunction.Bag(sprite);
                    break;
                case ItemType.Medical:
                    ItemPutAwayFunction.Medical(sprite);
                    break;
                case ItemType.MeleeWeapon:
                    ItemPutAwayFunction.MeleeWeapon(sprite);
                    break;
                case ItemType.Misc:
                    ItemPutAwayFunction.Misc(sprite);
                    break;
                case ItemType.ProjectileWeapon:
                    ItemPutAwayFunction.ProjectileWeapon(sprite);
                    //item.GetComponent<WeaponHandler>().enabledInQS = false;
                    break;
                case ItemType.Telescopes:
                    ItemPutAwayFunction.Telescopes(sprite);
                    break;
            }
        }
    }

    private void EnableNew(int slot) {
        GameObject QuickSlot = transform.GetChild(slot).gameObject;
        RawImage newImg = QuickSlot.GetComponent<RawImage>();
        newImg.color = Color.black;
        QuickSlotManager manager = QuickSlot.GetComponent<QuickSlotManager>();
        if (manager.Reference != null) {
            GameObject item = manager.Reference.Reference.gameObject;
            ItemPickable pickable = item.GetComponent<ItemPickable>();
            if (pickable.itemType == ItemType.ProjectileWeapon) {
                //item.GetComponent<WeaponHandler>().enabledInQS = true;
            }
        }
    }

}
