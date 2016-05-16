using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class QuickSlotManager : MonoBehaviour, IDropHandler {

    public ItemSprite Reference;
    public GameObject QuickSlotSprite;
    public Text itemName;
    public Text itemQty;

    [HideInInspector]
    public int id;

    void Start() {
        QuickSlotSprite = transform.GetChild(0).gameObject;
        string name = transform.name;
        string number = name.Substring(name.Length - 1, 1);
        int.TryParse(number, out this.id);
        id -= 1;
    }

    public void OnDrop(PointerEventData data) {
        GameObject droppedItem = data.pointerDrag;
        if (droppedItem.GetComponent<ItemSprite>() != null) {
            ItemSprite sprite = droppedItem.GetComponent<ItemSprite>();
            GameObject item = droppedItem.GetComponent<ItemSprite>().Reference;
            TransferProcess(sprite);
        } else if (droppedItem.GetComponent<QuickSlotSprite>() != null) {
           // QuickSlotSprite.transform.SetParent(transform);
            if (droppedItem.GetComponent<RawImage>().color.a != 0f) {
                transform.GetChild(0).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
                droppedItem.GetComponent<QuickSlotSprite>().initialParent.
                GetComponent<QuickSlotManager>().Reference.TransferToQuickSlot(id);
                QuickSlotSprite.GetComponent<QuickSlotSprite>().Refresh();
            }
        }
    }

    private void TransferProcess(ItemSprite sprite) {
        sprite.TransferToQuickSlot(id);
        QuickSlotSprite.GetComponent<QuickSlotSprite>().Refresh();
    }

    public void Clear() {
        RawImage img = QuickSlotSprite.GetComponent<RawImage>();
        Color c = new Color(255f, 255f, 255f, 0f);
        img.color = c;
        img.texture = null;
        itemName.text = "";
        itemQty.text = "";
        Reference.QSReference = null;
        Reference = null;
    }

    public void UpdateSlotInfo() {
        try {
            itemQty.text = Reference.Quantity.ToString();
            itemName.text = Reference.Reference.name;
        }
        catch (Exception) {
            Clear();
        }
    }
	


}
