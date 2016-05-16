using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryGridManager : MonoBehaviour {

    public Color Active_Color;
    public Color InActive_Color;

    void Start() {
        Initialize();
    }
    
    public void ChangeActiveInventorySlots(int change) {
        UI.root.playerInventory.ActiveInventorySlots = change;
        if (UI.root.playerInventory.ActiveInventorySlots > transform.childCount) {
            ActivateSlots(transform.childCount);
        } else {
            ActivateSlots(UI.root.playerInventory.ActiveInventorySlots);
        }
    }

    public void AddActiveInventorySlots(int change) {
        UI.root.playerInventory.ActiveInventorySlots += change;
        if (UI.root.playerInventory.ActiveInventorySlots > transform.childCount) {
            ActivateSlots(transform.childCount);
        } else {
            ActivateSlots(UI.root.playerInventory.ActiveInventorySlots);
        }
    }

    private void ActivateSlots(int ActiveInventorySlots) {
        Clear();
        for (int i = 0; i < ActiveInventorySlots; i++) {
            InventorySlotManager Slot = transform.GetChild(i).gameObject.GetComponent<InventorySlotManager>();
            Slot.Active = true;
            Slot.GetComponent<RawImage>().color = Active_Color;
        }
        CheckDropForInActive();
    }

    public void Initialize() {
        Clear();
        for (int i = 0; i < UI.root.playerInventory.ActiveInventorySlots; i++) {
            InventorySlotManager Slot = transform.GetChild(i).gameObject.GetComponent<InventorySlotManager>();
            Slot.Active = true;
            Slot.GetComponent<RawImage>().color  = Active_Color;
        }
        CheckDropForInActive();
    }

    private void Clear() {
        for (int i = 0; i < transform.childCount; i++) {
            InventorySlotManager Slot = transform.GetChild(i).gameObject.GetComponent<InventorySlotManager>();
            Slot.Active = false;
            Slot.GetComponent<RawImage>().color = InActive_Color;
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

}
