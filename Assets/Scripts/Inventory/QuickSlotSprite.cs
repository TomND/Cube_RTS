using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class QuickSlotSprite : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    [HideInInspector]
    public Transform initialParent;
    [HideInInspector]
    public Transform finalParent;
    
    public GameObject ItemName;
    public GameObject ItemQuantity;

    private bool dragged = false;

    public void OnBeginDrag(PointerEventData eventData) { // BEGIN DRAG 
        if (gameObject.GetComponent<RawImage>().color.a != 0f) {
            initialParent = transform.parent;
            transform.SetParent(UI.root.transform);
            ItemName.GetComponent<Text>().text = "";
            ItemQuantity.GetComponent<Text>().text = "";
            disableAllBlockRaycast();
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData) { // PROCESS OF DRAG
        if (gameObject.GetComponent<RawImage>().color.a != 0f) {
            transform.position = Input.mousePosition;
            dragged = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData) { // END DRAG
        if (dragged) {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.SetParent(initialParent);
            enableAllBlockRaycast();
            finalParent = transform.parent;
            Refresh();
        }
        dragged = false;
    }

    public bool disableAllBlockRaycast() {
        if (gameObject.GetComponent<RawImage>().color.a != 0f) {
            for (int i = 0; i < UI.root.QuickSlotGrid.transform.childCount; i++) {
                if (i != initialParent.GetComponent<QuickSlotManager>().id) {
                    UI.root.QuickSlotGrid.transform.GetChild(i).GetChild(0).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
            }
        }
        return true;
    }

    public bool enableAllBlockRaycast() {
        if (gameObject.GetComponent<RawImage>().color.a != 0f) {
            for (int i = 0; i < UI.root.QuickSlotGrid.transform.childCount; i++) {
                if (i != initialParent.GetComponent<QuickSlotManager>().id) {
                    UI.root.QuickSlotGrid.transform.GetChild(i).GetChild(0).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }
        }
        return true;
    }

    public void Refresh() {
        try {
            GameObject reference = transform.parent.GetComponent<QuickSlotManager>().Reference.Reference;
            if (reference.GetComponent<ItemBlock>() != null) {
                ItemName.GetComponent<Text>().text = reference.GetComponent<ItemBlock>().BlockName;
            } else {
                ItemName.GetComponent<Text>().text = reference.name;
            }
            ItemQuantity.GetComponent<Text>().text = transform.parent.GetComponent<QuickSlotManager>().Reference.Quantity.ToString();
            transform.localPosition = new Vector3(0, 0, 0);
            gameObject.GetComponent<RawImage>().SetNativeSize();
            gameObject.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(80, 80);
        }
        catch (Exception) {
            transform.localPosition = new Vector3(0, 0, 0);
            gameObject.GetComponent<RawImage>().SetNativeSize();
            gameObject.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(80, 80);
        }
    }
	
}
