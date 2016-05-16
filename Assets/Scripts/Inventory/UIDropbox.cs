using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIDropbox : MonoBehaviour, IDropHandler {

    public void OnDrop(PointerEventData data) {

        GameObject dropped = data.pointerDrag;

        if (dropped.GetComponent<ItemSprite>() != null) {
            if (dropped.GetComponent<ItemSprite>().Reference.GetComponent<ItemBlock>() != null) {
                return;
            }
        }

        if (dropped.GetComponent<QuickSlotSprite>() != null) {
            DropQuickSlotSprite(dropped.GetComponent<QuickSlotSprite>());
        } else if (dropped.GetComponent<ItemSprite>() != null) {
            PlayerInventory inv = UI.root.playerInventory;
            if (dropped.GetComponent<ItemSprite>().initialParent.name.Contains("Storage")) {
                if (UI.root.StoragePanel.activeSelf && inv.saveStorage != null) {
                    inv.saveStorage.items[dropped.GetComponent<ItemSprite>().initialParent.
                        gameObject.GetComponent<InventorySlotManager>().id] = null;
                }
                DropItemSprite(dropped.GetComponent<ItemSprite>());
            } else {
                DropItemSprite(dropped.GetComponent<ItemSprite>());
            }
        }

    }

    public void DropQuickSlotSprite(QuickSlotSprite qSprite) {
        if (qSprite.gameObject.GetComponent<RawImage>().color.a != 0f) {
            qSprite.enableAllBlockRaycast();
            qSprite.Refresh();
            qSprite.initialParent.GetComponent<QuickSlotManager>().Clear();
        }
    }

    public void DropItemSprite(ItemSprite iSprite) {
        if (iSprite.leftClick) {
            UI.root.playerInventory.DropItem(iSprite.Reference.GetComponent<ItemPickable>());
            iSprite.enableAllBlockRaycast();
            Destroy(iSprite.gameObject, 0.01f);
        }
    }
	
}
