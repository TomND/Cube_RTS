using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class ItemSprite : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler {

    public int Quantity = 0;
    public GameObject Reference;
    public GameObject ItemName;
    public GameObject ItemQuantity;

    public GameObject QSReference;

    private EventTrigger eventTrigger;
    private CanvasGroup raycastControl;

    void Start() {
        Initialize();
    }

    bool doubleClick = false;
    bool rightClickSplit = false;
    public void OnPointerUp(PointerEventData data) { // -2 = Right-Click   -1 = Left-Click
        if (data.pointerId == -1) { // left click (checks for double)
            if (doubleClick) {
                useItem();
                doubleClick = false;
            } else {
                StartCoroutine(doubleClickCatch());
            }
        }else if (data.pointerId == -2) { // right click
            if (Quantity > 1 && !UI.root.playerInventory.isInventoryFull()) {
                // Split
                if (!rightClickSplit) {
                    ClearQuickSlotInfo();
                    int newQuantity = Quantity / 2;
                    QuantityChangeImplicit(-newQuantity);
                    GameObject item = Instantiate(Reference.gameObject) as GameObject;
                    if (Reference.gameObject.GetComponent<ItemBlock>() != null) {
                        item.name = Reference.gameObject.GetComponent<ItemBlock>().BlockName;
                        item.transform.SetParent(GameObject.Find("Map").transform);
                    } else {
                        item.name = Reference.name;
                    }
                    item.GetComponent<ItemPickable>().Quantity = newQuantity;
                    try {
                        item.gameObject.GetComponent<Collider>().enabled = false;
                        item.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    }
                    catch (Exception) {
                        item.gameObject.SetActive(false);
                    }
                    UI.root.playerInventory.MoveItemToInventory(item.GetComponent<ItemPickable>(), newQuantity, true);
                    StartCoroutine(rightClick());
                }
            } else if (UI.root.playerInventory.isInventoryFull()) {
                UI.root.playerInventory.Show_InventoryFullDialogBox();
            }
        }
    }

    IEnumerator rightClick() {
        rightClickSplit = true;
        yield return new WaitForSeconds(0.25f);
        rightClickSplit = false;
    }

    IEnumerator doubleClickCatch() {
        doubleClick = true;
        yield return new WaitForSeconds(0.25f);
        doubleClick = false;
    }

    [HideInInspector]
    public Transform initialParent;
    [HideInInspector]
    public Transform finalParent;

    public bool leftClick = false;
    public void OnBeginDrag(PointerEventData eventData) { // BEGIN DRAG
        if (eventData.pointerId == -1) {
            leftClick = true;
            initialParent = transform.parent;
            transform.SetParent(UI.root.transform);
            ItemName.GetComponent<Text>().text = "";
            ItemQuantity.GetComponent<Text>().text = "";
            disableAllBlockRaycast();
            raycastControl.blocksRaycasts = false;
        } else {
            leftClick = false;
        }
    }

    public void OnDrag(PointerEventData eventData) { // PROCESS OF DRAG
        if (eventData.pointerId == -1) {
            leftClick = true;
            transform.position = Input.mousePosition;
        } else {
            leftClick = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData) { // END DRAG
        if (eventData.pointerId == -1) {
            leftClick = true;
            enableAllBlockRaycast();
            raycastControl.blocksRaycasts = true;
            gameObject.GetComponent<RawImage>().SetNativeSize();
            gameObject.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(80, 80);
            finalParent = transform.parent;
            if (finalParent.gameObject.GetComponent<InventorySlotManager>() == null) {
                transform.SetParent(initialParent);
            } else if (initialParent.name.Contains("Storage")) {
                PlayerInventory inv = UI.root.playerInventory;
                if (UI.root.StoragePanel.activeSelf && inv.saveStorage != null) {
                    inv.saveStorage.items[initialParent.gameObject.
                        GetComponent<InventorySlotManager>().id] = null;
                }
            }
            Refresh();
        } else {
            leftClick = false;
        }
    }

    public bool disableAllBlockRaycast() {
        for (int i = 0; i < UI.root.playerInventory.ActiveInventorySlots; i++) {
            if (UI.root.InventorySlotGrid.transform.GetChild(i).childCount > 0) {
                UI.root.InventorySlotGrid.transform.GetChild(i).GetChild(0).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
        for (int i = 0; i < UI.root.QuickSlotGrid.transform.childCount; i++) {
            try {
                UI.root.QuickSlotGrid.transform.GetChild(i).GetChild(0).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            } catch (Exception e) { print(i); }
        }
        return true;
    }

    public bool enableAllBlockRaycast() {
        for (int i = 0; i < UI.root.playerInventory.ActiveInventorySlots; i++) {
            if (UI.root.InventorySlotGrid.transform.GetChild(i).childCount > 0) {
                UI.root.InventorySlotGrid.transform.GetChild(i).GetChild(0).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
        for (int i = 0; i < UI.root.QuickSlotGrid.transform.childCount; i++) {
            UI.root.QuickSlotGrid.transform.GetChild(i).GetChild(0).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        return true;
    }

    public bool QuantityChange(int change, ItemPickable item) {
        if ((Quantity + change) > item.MaxStack) {
            return false;
        } else {
            return true;
        }
    }

    public void QuantityChangeImplicit(int change) {
        Quantity += change;
        CheckQuantity();
        Reference.GetComponent<ItemPickable>().Quantity = this.Quantity;
        Refresh();
    }

    public void QuantityChangeImplicitEqualsTo(int change) {
        Quantity = change;
        CheckQuantity();
        Reference.GetComponent<ItemPickable>().Quantity = this.Quantity;
        Refresh();
    }

    private void CheckQuantity() {
        if (Quantity == 0) {
            Destroy(Reference.gameObject);
            Destroy(gameObject);
        }
    }

    bool willDestroyVar = false;
    public void willDestroy() {
        willDestroyVar = true;
    }

    public void Refresh() {
        try {
            if (!willDestroyVar) {
                if (Reference.gameObject.GetComponent<ItemBlock>() != null) {
                    ItemName.GetComponent<Text>().text = Reference.gameObject.GetComponent<ItemBlock>().BlockName;
                } else {
                    ItemName.GetComponent<Text>().text = Reference.name;
                }
                ItemQuantity.GetComponent<Text>().text = Quantity.ToString();
                transform.localPosition = new Vector3(0, 0, 0);
                GetComponent<RawImage>().SetNativeSize();
                gameObject.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(80, 80);
                UpdateQuickSlotInfo();
            }
        }
        catch (Exception) { }
    }

    private void Initialize() {
        ItemName = Instantiate(Resources.Load("ItemName")) as GameObject;
        ItemQuantity = Instantiate(Resources.Load("ItemQuantity")) as GameObject;

        Vector3 namePos = ItemName.transform.localPosition;
        Vector3 qtyPos = ItemQuantity.transform.localPosition;

        ItemName.transform.SetParent(transform);
        ItemQuantity.transform.SetParent(transform);

        ItemName.transform.localPosition = namePos;
        ItemQuantity.transform.localPosition = qtyPos;

        try {
            if (Reference.gameObject.GetComponent<ItemBlock>() != null) {
                ItemName.GetComponent<Text>().text = Reference.gameObject.GetComponent<ItemBlock>().BlockName;
            } else {
                ItemName.GetComponent<Text>().text = Reference.name;
            }
            ItemQuantity.GetComponent<Text>().text = Quantity.ToString();
        }
        catch (Exception) {
            Destroy(gameObject);
        }

        eventTrigger = gameObject.AddComponent<EventTrigger>();
        raycastControl = gameObject.AddComponent<CanvasGroup>();
        raycastControl.blocksRaycasts = true;
        initialParent = transform.parent;
        GetComponent<RawImage>().SetNativeSize();
        gameObject.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(80, 80);
        Refresh();
    }

    public void TransferToQuickSlot() {
        int x = findAvailableQuickSlot();
        if (x >= 0) {
            ClearQuickSlotInfo();
            GameObject qSlot = UI.root.QuickSlotGrid.transform.GetChild(x).gameObject;
            QSReference = qSlot;
            QuickSlotManager qSlotManager = qSlot.GetComponent<QuickSlotManager>();
            RawImage img = qSlot.transform.GetChild(0).gameObject.GetComponent<RawImage>();
            img.texture = Reference.GetComponent<ItemPickable>().ItemIcon;
            img.color = Color.white;
            qSlotManager.Reference = this;
            qSlotManager.UpdateSlotInfo();
        }
    }

    public void TransferToQuickSlot(int position) {
        if (position >= 0) {
            ClearQuickSlotInfo();
            GameObject qSlot = UI.root.QuickSlotGrid.transform.GetChild(position).gameObject;
            QSReference = qSlot;
            QuickSlotManager qSlotManager = qSlot.GetComponent<QuickSlotManager>();
            RawImage img = qSlot.transform.GetChild(0).gameObject.GetComponent<RawImage>();
            img.texture = Reference.GetComponent<ItemPickable>().ItemIcon;
            img.color = Color.white;
            qSlotManager.Reference = this;
            qSlotManager.UpdateSlotInfo();
        }
    }

    public void ClearQuickSlotInfo() {
        if (QSReference != null) {
            QSReference.GetComponent<QuickSlotManager>().Clear();
        }
    }

    public void UpdateQuickSlotInfo() {
        if (QSReference != null) {
            QSReference.GetComponent<QuickSlotManager>().UpdateSlotInfo();
        }
    }

    public int findAvailableQuickSlot() {
        Transform[] qSlots = returnQuickSlotArray();
        for (int i = 0; i < qSlots.Length; i++) {
            if (qSlots[i].gameObject.GetComponent<QuickSlotManager>().Reference == null) {
                return i;
            }
        }
        return -1;
    }

    public Transform[] returnQuickSlotArray() {
        Transform[] t = new Transform[UI.root.QuickSlotGrid.transform.childCount];
        for (int i = 0; i < t.Length; i++) {
            t[i] = UI.root.QuickSlotGrid.transform.GetChild(i).transform;
        }
        return t;
    }

    void OnDestroy() {
        UpdateQuickSlotInfo();
        ClearQuickSlotInfo();
    }

    public void useItem() {
        ItemType type = Reference.GetComponent<ItemPickable>().itemType;
        if (type == ItemType.Bag) {
            ItemFunction.Bag(this);
        } else if (type == ItemType.Telescopes) {
            ItemFunction.Telescopes(this);
        } else if (type == ItemType.Apparel) {
            ItemFunction.Apparel(this);
        } else if (type == ItemType.ProjectileWeapon) {
            ItemFunction.ProjectileWeapon(this);
        } else if (type == ItemType.MeleeWeapon) {
            ItemFunction.MeleeWeapon(this);
        } else if (type == ItemType.Medical) {
            ItemFunction.Medical(this);
        } else if (type == ItemType.Ammunition) {
            ItemFunction.Ammunition(this);
        } else if (type == ItemType.Misc) {
            ItemFunction.Misc(this);
        }
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