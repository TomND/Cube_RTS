using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class StorageGridManager : MonoBehaviour {

    public Color Active_Color;
    public Color InActive_Color;

    public void ChangeActiveInventorySlots(int change, ItemPickable[] item, StorageViewable storage) {
        ActivateSlots(change, item, storage);
    }

    private void ActivateSlots(int capacity, ItemPickable[] item, StorageViewable storage) {
        Clear();
        for (int i = 0; i < capacity; i++) {
            InventorySlotManager Slot = transform.GetChild(i).gameObject.GetComponent<InventorySlotManager>();
            Slot.Active = true;
            Slot.GetComponent<RawImage>().color = Active_Color;
            if (item[i] != null) {
                GameObject itemObject = new GameObject();
                ItemSprite itemSprite = itemObject.AddComponent<ItemSprite>();
                RawImage image = itemObject.AddComponent<RawImage>();
                itemSprite.Reference = item[i].gameObject;
                itemSprite.Quantity = item[i].Quantity;
                image.texture = item[i].ItemIcon;
                image.SetNativeSize();
                itemObject.name = item[i].name;
                itemObject.transform.SetParent(Slot.transform, false);
                try {
                    item[i].gameObject.GetComponent<Collider>().enabled = false;
                    item[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                catch (Exception e) {
                    item[i].gameObject.SetActive(false);
                }
            }
        }
        CheckDropForInActive();
    }

    public void SaveSlots(StorageViewable storage) {
        for (int i = 0; i < storage.items.Length; i++) {
            if (UI.root.StorageSlotGrid.transform.GetChild(i).transform.childCount > 0) {
                storage.items[i] = UI.root.StorageSlotGrid.transform.GetChild(i).GetChild(0).gameObject.
                    GetComponent<ItemSprite>().Reference.GetComponent<ItemPickable>();
            }
        }
    }

    private void Clear() {
        for (int i = 0; i < transform.childCount; i++) {
            InventorySlotManager Slot = transform.GetChild(i).gameObject.GetComponent<InventorySlotManager>();
            Slot.Active = false;
            Slot.GetComponent<RawImage>().color = InActive_Color;
            if (Slot.transform.childCount > 0) {
                Destroy(Slot.transform.GetChild(0).gameObject);
            }
        }
    }

    private void CheckDropForInActive() {
        for (int i = 0; i < transform.childCount; i++) {
            InventorySlotManager Slot = transform.GetChild(i).gameObject.GetComponent<InventorySlotManager>();
            if (!Slot.Active && Slot.transform.childCount > 0) {
                Slot.DropItems();
            }
        }
    }

    /*public void AddActiveInventorySlots(int change) { NOT MEANT FOR STORAGE
    UI.root.playerInventory.ActiveInventorySlots += change;
    if (UI.root.playerInventory.ActiveInventorySlots > transform.childCount) {
        ActivateSlots(transform.childCount);
    } else {
        ActivateSlots(UI.root.playerInventory.ActiveInventorySlots);
    }
}*/

}
