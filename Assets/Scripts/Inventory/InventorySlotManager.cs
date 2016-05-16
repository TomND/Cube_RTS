using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class InventorySlotManager : MonoBehaviour, IDropHandler {

    public bool Active = false;
    public bool GearSlot = false;
    public bool Quickslot = false;
    [HideInInspector]
    public int id;

    void Start() {
        string name = transform.name;
        string number = name.Substring(name.Length - 1, 1);
        int.TryParse(number, out this.id);
        id -= 1;
    }

    public void OnDrop(PointerEventData data) {
        GameObject droppedObject = data.pointerDrag;
        ItemSprite droppedSprite = droppedObject.GetComponent<ItemSprite>();

        if (transform.childCount > 0 && Active && 
            droppedSprite.Reference.GetComponent<ItemPickable>().Stackable) { // checks if multiple/stackable

            ItemSprite currentSprite = transform.GetChild(0).gameObject.GetComponent<ItemSprite>();
            bool stackableCurrent = currentSprite.Reference.GetComponent<ItemPickable>().Stackable;

            if (stackableCurrent) {
                int availableToTransfer = currentSprite.Reference.GetComponent<ItemPickable>().MaxStack - currentSprite.Quantity;
                if (droppedSprite.Reference.name == currentSprite.Reference.name) {
                    bool limitOfStack = currentSprite.QuantityChange(droppedSprite.Quantity, droppedSprite.Reference.GetComponent<ItemPickable>());
                    if (limitOfStack) {
                        droppedSprite.enableAllBlockRaycast();
                        currentSprite.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
                        currentSprite.QuantityChangeImplicit(droppedSprite.Quantity);
                        currentSprite.UpdateQuickSlotInfo();
                        droppedSprite.ClearQuickSlotInfo();
                        droppedSprite.willDestroy();
                        Destroy(droppedSprite.Reference.gameObject);
                        Destroy(droppedObject, 0.25f);
                    } else {
                        if (droppedSprite.Quantity > 0 && availableToTransfer > 0) {
                            currentSprite.UpdateQuickSlotInfo();
                            droppedSprite.UpdateQuickSlotInfo();
                            droppedSprite.QuantityChangeImplicit(-availableToTransfer);
                            currentSprite.QuantityChangeImplicit(availableToTransfer);
                        }
                    }
                }
            } else {
                if (transform.childCount == 0 && Active) {
                    droppedObject.transform.SetParent(transform);
                    droppedObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }

        }else{
            if (transform.childCount == 0 && Active) {
                droppedObject.transform.SetParent(transform);
                droppedObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
    }

    public void DropItems() {
        if (transform.childCount > 0) {
            ItemSprite sprite = transform.GetChild(0).gameObject.GetComponent<ItemSprite>();
            UI.root.playerInventory.DropItem(sprite.Reference.GetComponent<ItemPickable>());
            if (sprite.QSReference != null) {
                sprite.QSReference.GetComponent<QuickSlotManager>().Clear();
            }
            Destroy(transform.GetChild(0).gameObject);
        }
    }

  }

