using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PlayerInventory : MonoBehaviour
{

    public float distance = 10f;
    public int ActiveInventorySlots = 15;

    public static bool inUI = false;

    GameObject Door;
    GameObject PointingAtObject;
    bool openDoor = false;
    bool closeDoor = false;
    public int doorSpeed = 90;

    bool LookingAt;
    bool PointingAtPickable;
    bool PointingAtStorage;
    bool PointingAtDoor;
    RaycastHit Hit;

    void Start()
    {
        UI.root.InventorySlotGrid.GetComponent<InventoryGridManager>().Initialize();
        CheckGUI(true);
    }

    void Update()
    {

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        LookingAt = Physics.Raycast(ray, out Hit, distance);

        if (LookingAt)
        {
            if (ItemDebugMonitor(Hit.transform.gameObject) && !PlayerInventory.inUI)
            {
                PointingAtPickable = true;
                PointingAtStorage = false;
                PointingAtDoor = false;
                PointingAtObject = Hit.transform.gameObject;
                CheckItemPickUp();
                /*if (PointingAtObject.GetComponent<WeaponHandler>() == null)
                {
                    DisplayNotificationBox(PointingAtObject.GetComponent<ItemPickable>().name,
                    PointingAtObject.GetComponent<ItemPickable>().Quantity.ToString(),
                    PointingAtObject.GetComponent<ItemPickable>().ItemIcon);
                }*/
            }
            else if (Hit.transform.gameObject.tag == "Door")
            {
                PointingAtDoor = true;
                DoorFunction();
            }
            else if (Hit.transform.gameObject.GetComponent<StorageViewable>() != null)
            {
                PointingAtStorage = true;
                PointingAtPickable = false;
                PointingAtDoor = false;
                PointingAtObject = Hit.transform.gameObject;
                CheckStorageViewable();
            }
            else
            {
                UI.root.NotificationBox.SetActive(false);
                PointingAtDoor = false;
                PointingAtStorage = false;
                PointingAtPickable = false;
            }
        }
        else
        {
            try
            {
                UI.root.NotificationBox.SetActive(false);
            }
            catch (Exception) { }
            PointingAtDoor = false;
            PointingAtStorage = false;
            PointingAtPickable = false;
        }

        //CheckHold();

        if (openDoor)
        {
            OpenDoor();
        }
        else if (closeDoor)
        {
            CloseDoor();
        }

        InteractGUI();
        
        if(Input.GetMouseButtonDown(0) && UI.root.QuickSlotGrid.GetComponent<QuickSlotGridManager>().selectedSlot != -1)
        {
            ItemSprite sprite = UI.root.QuickSlotGrid.transform.GetChild(UI.root.QuickSlotGrid.GetComponent<QuickSlotGridManager>().selectedSlot).GetComponent<QuickSlotManager>().Reference;
            if(sprite != null && sprite.Reference != null)
            {
                switch(sprite.Reference.GetComponent<ItemPickable>().itemType)
                {
                    case ItemType.Ammunition:
                        ItemFunction.Ammunition(sprite);
                        break;
                    case ItemType.Apparel:
                        ItemFunction.Apparel(sprite);
                        break;
                    case ItemType.Bag:
                        ItemFunction.Bag(sprite);
                        break;
                    case ItemType.Medical:
                        ItemFunction.Medical(sprite);
                        break;
                    case ItemType.MeleeWeapon:
                        ItemFunction.MeleeWeapon(sprite);
                        break;
                    case ItemType.Misc:
                        ItemFunction.Misc(sprite);
                        break;
                    case ItemType.ProjectileWeapon:
                        ItemFunction.ProjectileWeapon(sprite);
                        break;
                    case ItemType.Telescopes:
                        ItemFunction.Telescopes(sprite);
                        break;
                }
            }
        }
    }

    public void DisplayNotificationBox(string itemName, string itemQuantity, Texture2D imgItem)
    {
        UI.root.NotificationBox.SetActive(true);
        UI.root.notification_txtItemName.text = itemName;
        UI.root.notification_txtItemQuantity.text = itemQuantity;
        UI.root.notification_imgItem.texture = imgItem;
    }

    public void DisplayGeneralNotificationBox(string Message, float lengthOfTime)
    {
        UI.root.generalNotification_txtMessage.text = Message;
        StartCoroutine(DelayShow(lengthOfTime));
    }

    IEnumerator DelayShow(float time)
    {
        UI.root.GeneralNotificationBox.SetActive(true);
        yield return new WaitForSeconds(time);
        UI.root.GeneralNotificationBox.SetActive(false);
    }

    public void CheckItemPickUp()
    {
        if (PointingAtPickable)
        {
            if (Input.GetKeyDown(Keymap.Interact))
            {
                ItemPickable item = PointingAtObject.GetComponent<ItemPickable>();
                MoveItemToInventory(item, item.Quantity, false);
            }
        }
    }

    [HideInInspector]
    public StorageViewable saveStorage;
    public void CheckStorageViewable()
    {
        if (PointingAtStorage)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                CheckGUI(false);
                UI.root.StoragePanel.SetActive(true);
                UI.root.InventoryPanel.transform.SetParent(UI.root.Storage_InventoryPos);
                UI.root.InventoryPanel.transform.localPosition = new Vector3(0f, 0f, 0f);
                StorageViewable storage = Hit.transform.gameObject.GetComponent<StorageViewable>();
                UI.root.StorageNameText.text = storage.Name;
                saveStorage = storage;
                StorageGridManager manager = UI.root.StorageSlotGrid.GetComponent<StorageGridManager>();
                int capacity = storage.items.Length;
                if (capacity > 40)
                {
                    capacity = 40;
                }
                manager.ChangeActiveInventorySlots(capacity, storage.items, storage);
            }
        }
    }

    public void InteractGUI()
    {
        if (Input.GetKeyDown(Keymap.AccessGUI))
        {
            CheckGUI(UI.root.MasterPanel.activeSelf);
        }
        CheckQuickSlotInput();
    }


    public void ExecuteCraft(ItemPickable i, int itemQty)
    {
        StartCoroutine(DelayOperation(i, itemQty));
    }

    IEnumerator DelayOperation(ItemPickable i, int itemQty)
    {
        yield return new WaitForEndOfFrame();
        //print("item being transferred " + i.name);
        UI.root.playerInventory.RefreshHeld();
        UI.root.playerInventory.MoveItemToInventory(i, itemQty, false);
    }

    public void CheckGUI(bool visible)
    {
        if (visible)
        {
            initInventory();
            UI.root.MasterPanel.SetActive(false);
            //animController.mouseControl = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            UI.root.MasterPanel.SetActive(true);
            //animController.mouseControl = false;
            CheckReferences();
            Cursor.lockState = CursorLockMode.None;
            //CraftingManager.Reinit();
            //UI.root.CraftingListGrid.GetComponent<CraftingManager>().GenerateCraftingInfoList();
        }
        PlayerInventory.inUI = UI.root.MasterPanel.activeSelf;
        FirstPersonController.LockCursor(!PlayerInventory.inUI);
    }

    public void CheckReferences() {
         for (int i = 0; i < UI.root.InventorySlotGrid.transform.childCount; i++) {
            GameObject Slot = UI.root.InventorySlotGrid.transform.GetChild(i).gameObject;
            if (Slot.transform.childCount != 0) {
                ItemSprite sprite = Slot.transform.GetChild(0).GetComponent<ItemSprite>();
                if (sprite != null) {
                    if (sprite.Reference == null) {
                        Destroy(sprite.gameObject);
                    }
                }
            }
        }
    }

    public void initInventory()
    {
        if (UI.root.StoragePanel.activeSelf && saveStorage != null)
        {
            UI.root.StorageSlotGrid.GetComponent<StorageGridManager>().SaveSlots(saveStorage);
        }
        UI.root.InventoryPanel.transform.SetParent(UI.root.MasterPanel.transform);
        UI.root.InventoryPanel.transform.localPosition = new Vector3(UI.root.InventoryPanel.transform.localPosition.x,
            0f, UI.root.InventoryPanel.transform.localPosition.y);
        UI.root.StoragePanel.SetActive(false);
        UI.root.StructurePanel.SetActive(false);
        saveStorage = null;
    }

    bool FullInventory = false;
    public void MoveItemToInventory(ItemPickable itemPickable, int quantity, bool ignoreCommonItems)
    {
        //print(" Moving... " + held);
        if (held < ActiveInventorySlots)
        {
            FullInventory = false;
            for (int i = 0; i < UI.root.InventorySlotGrid.transform.childCount; i++)
            {
                InventorySlotManager Slot = UI.root.InventorySlotGrid.transform.GetChild(i).GetComponent<InventorySlotManager>();
                //print("here " + i + " child count: " + Slot.transform.childCount);
                bool limitOfStack = false;
                try
                {
                    limitOfStack = Slot.transform.GetChild(0).GetComponent<ItemSprite>().QuantityChange(quantity, itemPickable); // true --> can change + quantity
                }
                catch (Exception) { }
                if (Slot.Active && Slot.transform.childCount == 0 && quantity == 1)
                { // Checking if Single and not filled
                    Single_AddToSlot(itemPickable, Slot);
                    //print("Here");
                    break;
                }
                if (Slot.Active && (Slot.transform.childCount == 0) && itemPickable.Stackable && !ignoreCommonItems && quantity > 1)
                {
                    MultipleInit_AddToSlot(itemPickable, Slot, quantity);
                    break;
                }
          
                if (Slot.transform.childCount >= 1)
                {
                    if (Slot.Active && (Slot.transform.GetChild(0).GetComponent<ItemSprite>().Reference.name == itemPickable.name) && itemPickable.Stackable && limitOfStack && !ignoreCommonItems)
                    { // Checking if same item and stackable
                        Multiple_AddToSlot(itemPickable, Slot, quantity);
                        break;
                    }
                }
                if (Slot.Active && (Slot.transform.childCount == 0) && itemPickable.Stackable && limitOfStack && ignoreCommonItems)
                { // Checking if same item and stackable
                    Multiple_AddToSlot(itemPickable, Slot, quantity);
                    break;
                }
            }
        }
        else
        {
            FullInventory = true;
            Show_InventoryFullDialogBox();
        }
    }

    public bool isInventoryFull()
    {
        RefreshHeld();
        if (held < ActiveInventorySlots)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool isInventoryFull(int aboutToAdd, int limit)
    {
        RefreshHeld();
        float slotsAdded = Mathf.Ceil((float)aboutToAdd / (float)limit);
        if (held + (int)slotsAdded <= ActiveInventorySlots)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool isInventoryFull(int ToRemove, int aboutToAdd, int limit)
    {
        RefreshHeld();
        float slotsAdded = Mathf.Ceil((float)aboutToAdd / (float)limit);
        if ((held - ToRemove) + (int)slotsAdded <= ActiveInventorySlots)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Show_InventoryFullDialogBox()
    {
        DisplayGeneralNotificationBox("Inventory is full.", 3f);
    }

    private void Multiple_AddToSlot(ItemPickable item, InventorySlotManager Slot, int quantity)
    {
        Slot.transform.GetChild(0).GetComponent<ItemSprite>().QuantityChangeImplicit(quantity);
        Destroy(item.gameObject);
    }

    private void MultipleInit_AddToSlot(ItemPickable itemPickable, InventorySlotManager Slot, int quantity)
    {
        GameObject item = new GameObject();
        ItemSprite itemSprite = item.AddComponent<ItemSprite>();
        RawImage image = item.AddComponent<RawImage>();
        itemSprite.Reference = itemPickable.gameObject;
        itemSprite.Quantity = quantity;
        image.texture = itemPickable.ItemIcon;
        image.SetNativeSize();
        image.rectTransform.sizeDelta = new Vector2(80, 80);
        if (Hit.transform.gameObject.GetComponent<ItemBlock>() != null) {
            item.name = Hit.transform.gameObject.GetComponent<ItemBlock>().BlockName;
        } else {
            item.name = itemPickable.gameObject.name;
        }
        item.transform.SetParent(Slot.transform, false);
        try
        {
            itemPickable.gameObject.GetComponent<Collider>().enabled = false;
            itemPickable.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        catch (Exception)
        {
            itemPickable.gameObject.SetActive(false);
        }
        Refresh();
    }

    /*  private void Debug() {
          print("Slot Index " + i);
          print("Slot Active " + Slot.Active);
          print("Slot Child Count " + Slot.transform.childCount);
          print("Item Pickable " + itemPickable.Stackable);
          print("Limit of Stack " + limitOfStack);
          print("IgnoreCommonItems " + ignoreCommonItems);
          print("Quantity " + quantity);
      }*/

    private void Single_AddToSlot(ItemPickable itemPickable, InventorySlotManager Slot)
    {
        GameObject item = new GameObject();
        ItemSprite itemSprite = item.AddComponent<ItemSprite>();
        RawImage image = item.AddComponent<RawImage>();
        itemSprite.Reference = itemPickable.gameObject;
        itemSprite.Quantity = 1;
        image.texture = itemPickable.ItemIcon;
        image.SetNativeSize();
        image.rectTransform.sizeDelta = new Vector2(80, 80);
        if (itemPickable.transform.gameObject.GetComponent<ItemBlock>() != null) {
            item.name = itemPickable.transform.gameObject.GetComponent<ItemBlock>().BlockName;
        } else {
            item.name = itemPickable.gameObject.name;
        }
        item.transform.SetParent(Slot.transform, false);
        try
        {
            itemPickable.gameObject.GetComponent<Collider>().enabled = false;
            itemPickable.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        catch (Exception)
        {
            itemPickable.gameObject.SetActive(false);
        }
        Refresh();
    }

    public void DropItem(ItemPickable itemPickable)
    {
        if (itemPickable.transform.GetComponent<ItemBlock>() != null) {
            return;
        }
        itemPickable.gameObject.GetComponent<Rigidbody>().drag = 3f;
        itemPickable.transform.position = gameObject.transform.position + gameObject.transform.forward * 1.5f + new Vector3(0, 1.5f, 0);
        try
        {
            itemPickable.gameObject.GetComponent<Collider>().enabled = true;
            itemPickable.gameObject.GetComponent<MeshRenderer>().enabled = true;
            itemPickable.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            PointingAtPickable = false;
        }
        catch (Exception)
        {
            itemPickable.gameObject.SetActive(true);
            PointingAtPickable = false;
            itemPickable.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        Refresh();
    }

    public int held = 0;
    private bool Refreshed = false;
    void Refresh()
    {
        int held = 0;
        StartCoroutine(Refresher());
        for (int i = 0; i < UI.root.InventorySlotGrid.transform.childCount; i++)
        {
            if (UI.root.InventorySlotGrid.transform.GetChild(i).childCount >= 1)
            {
                held++;
            }
        }
        this.held = held;
        UI.root.numberOfItems.text = held.ToString() + "/" + ActiveInventorySlots.ToString();
    }

    public void RefreshHeld()
    {
        int held = 0;
        StartCoroutine(Refresher());
        for (int i = 0; i < UI.root.InventorySlotGrid.transform.childCount; i++)
        {
            if (UI.root.InventorySlotGrid.transform.GetChild(i).childCount >= 1)
            {
                held++;
            }
        }
        this.held = held;
        UI.root.numberOfItems.text = held.ToString() + "/" + ActiveInventorySlots.ToString();
    }

    IEnumerator Refresher()
    {
        Refreshed = true;
        yield return new WaitForSeconds(0.1f);
        Refreshed = false;
    }

    GameObject save;
    void OnGUI()
    {

        if (Hit.transform != null) {
            if (CheckIfBlock(Hit.transform.gameObject)) {
                return;
            }
        }

        if (PointingAtDoor && !PlayerInventory.inUI)
        {
            Crosshair.SetCrosshair(false);
            GUI.DrawTexture(new Rect((Screen.width / 2) - (UI.root.HandGrabIcon.width / 8), (Screen.height / 2) - (UI.root.HandGrabIcon.height / 8), UI.root.HandGrabIcon.width / 4, UI.root.HandGrabIcon.height / 4), UI.root.HandGrabIcon);
        }
        else
        {
            Crosshair.SetCrosshair(true);
        }
        if ((PointingAtPickable || PointingAtStorage) && !PlayerInventory.inUI && !Refreshed && Hit.transform != null)
        {
            Crosshair.SetCrosshair(false);
            GUI.DrawTexture(new Rect((Screen.width / 2) - (UI.root.HandGrabIcon.width / 8), (Screen.height / 2) - (UI.root.HandGrabIcon.height / 8), UI.root.HandGrabIcon.width / 4, UI.root.HandGrabIcon.height / 4), UI.root.HandGrabIcon);
            if (save == null)
            {
                if (PointingAtPickable)
                {
                    if (Hit.transform.GetComponent<ItemBlock>() == null) {
                        TryOutlineForPickable();
                    }
                }
                else if (PointingAtStorage)
                {
                    //StorageViewable pointingAt = Hit.transform.gameObject.GetComponent<StorageViewable>();
                    TryOutlineForStorage();
                }
            }
        }
        else
        {
            if (save != null)
            {
                if (save.GetComponent<ItemPickable>() != null)
                {
                    CheckRemoveOutline("pickable");
                }
                else if (save.GetComponent<StorageViewable>() != null)
                {
                    CheckRemoveOutline("storage");
                }
            }
            Crosshair.SetCrosshair(true);
        }
    }

    private bool TryOutlineForPickable()
    {
        try
        {
            Transform item;
            if (Hit.transform.gameObject.GetComponent<ItemPickable>().ItemBody != null)
            {
                item = Hit.transform.gameObject.GetComponent<ItemPickable>().ItemBody;
            }
            else
            {
                item = Hit.transform;
            }
            if (Hit.transform.gameObject != null && item.FindChild("Viewing") == null)
            {
                save = item.gameObject;
                GameObject dupe = Instantiate(item.gameObject) as GameObject;
                if (!Hit.transform.name.Contains("Block")) {
                    Hit.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                } else {
                    dupe.GetComponent<BoxCollider>().isTrigger = true;
                }
                try
                {
                    Destroy(dupe.GetComponent<Rigidbody>());
                }
                catch (Exception) { }
                Destroy(dupe.GetComponent<ItemPickable>());
                dupe.transform.SetParent(item.transform);
                dupe.transform.localPosition = new Vector3(0, 0, 0);
                dupe.transform.localEulerAngles = new Vector3(0, 0, 0);
               /* if (save.transform.parent != null)
                {
                    dupe.transform.localScale = new Vector3(Hit.transform.localScale.x * Hit.transform.parent.localScale.x,
                        Hit.transform.localScale.y * Hit.transform.parent.localScale.y,
                        Hit.transform.localScale.z * Hit.transform.parent.localScale.z);
                }
                else
                {
                    dupe.transform.localScale = new Vector3(Hit.transform.localScale.x, Hit.transform.localScale.y, Hit.transform.localScale.z);
                }*/
                dupe.transform.localScale = new Vector3(1f, 1f, 1f);
                dupe.name = "Viewing";
                dupe.GetComponent<Renderer>().material = UI.root.BlankMat;
                dupe.GetComponent<Renderer>().material.shader = UI.root.ObjectOutlineShader;
                dupe.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.green);
                dupe.GetComponent<Renderer>().material.SetFloat("_Outline", 0.005f);
                return true;
            }
        } catch (Exception) { }
        return false;
    }

    private bool TryOutlineForStorage()
    {
        try
        {
            StorageViewable storageScript = Hit.transform.gameObject.GetComponent<StorageViewable>();
            if (Hit.transform.gameObject != null && storageScript.targetMaterial.transform.FindChild("Viewing") == null)
            {
                save = Hit.transform.gameObject;
                Hit.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                GameObject dupe = Instantiate(storageScript.targetMaterial) as GameObject;
                try
                {
                    Destroy(dupe.GetComponent<StorageViewable>());
                }
                catch (Exception) { }
                dupe.transform.SetParent(storageScript.targetMaterial.transform);
                dupe.transform.localPosition = new Vector3(0, 0, 0);
                dupe.transform.localEulerAngles = new Vector3(0, 0, 0);
               /* if (save.transform.parent != null)
                {
                    dupe.transform.localScale = new Vector3(Hit.transform.localScale.x * Hit.transform.parent.localScale.x,
                        Hit.transform.localScale.y * Hit.transform.parent.localScale.y,
                        Hit.transform.localScale.z * Hit.transform.parent.localScale.z);
                
                else
                {
                    dupe.transform.localScale = new Vector3(Hit.transform.localScale.x, Hit.transform.localScale.y, Hit.transform.localScale.z);
                }*/
                dupe.transform.localScale = new Vector3(1f, 1f, 1f);
                dupe.name = "Viewing";
                dupe.GetComponent<Renderer>().material = UI.root.BlankMat;
                dupe.GetComponent<Renderer>().material.shader = UI.root.ObjectOutlineShader;
                dupe.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.green);
                dupe.GetComponent<Renderer>().material.SetFloat("_Outline", 0.005f);
                return true;
            }
        }
        catch (Exception) { }
        return false;
    }

    /*bool held;
    void CheckHold() {
        if (save != null) {
            if (save.transform.FindChild("Viewing") != null) {
                if (AInput.IsPressed("Hold")) {
                    held = true;
                    save.transform.parent = UI.root.DefaultArms.transform;
                } else {
                    held = false;
                    save.transform.parent = null;
                }
            } else {
                held = false;
            }
        } else {
            held = false;
        }
    }*/

    void CheckRemoveOutline(String type)
    {
        if (save != null)
        {
            if (type.Equals("pickable"))
            {
                if (save.transform.FindChild("Viewing") != null)
                {
                    Destroy(save.transform.FindChild("Viewing").gameObject);
                    save.transform.parent = null;
                    if (!save.transform.name.Contains("Block")) {
                        save.GetComponent<Rigidbody>().isKinematic = false;
                        save.GetComponent<Rigidbody>().WakeUp();
                    }
                    save = null;
                }
                else if (save.GetComponent<ItemPickable>().ItemBody != null)
                {
                    Destroy(save.GetComponent<ItemPickable>().ItemBody.FindChild("Viewing").gameObject);
                    save.transform.parent = null;
                    if (!save.transform.name.Contains("Block")) {
                        save.GetComponent<Rigidbody>().isKinematic = false;
                        save.GetComponent<Rigidbody>().WakeUp();
                    }
                    save = null;
                }
            }
            else if (type.Equals("storage"))
            {
                StorageViewable storageScript = save.GetComponent<StorageViewable>();
                if (storageScript.targetMaterial.transform.FindChild("Viewing") != null)
                {
                    Destroy(storageScript.targetMaterial.transform.FindChild("Viewing").gameObject);
                    save.transform.parent = null;
                    if (!save.transform.name.Contains("Block")) {
                        save.GetComponent<Rigidbody>().isKinematic = false;
                        save.GetComponent<Rigidbody>().WakeUp();
                    }
                    save = null;
                }
            }
        }
    }

    private void CheckQuickSlotInput()
    {
        QuickSlotGridManager grid = UI.root.QuickSlotGrid.GetComponent<QuickSlotGridManager>();
        int slotNumber = -1;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            slotNumber = 0;
            grid.changeSlot(slotNumber);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            slotNumber = 1;
            grid.changeSlot(slotNumber);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            slotNumber = 2;
            grid.changeSlot(slotNumber);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            slotNumber = 3;
            grid.changeSlot(slotNumber);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            slotNumber = 4;
            grid.changeSlot(slotNumber);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            slotNumber = 5;
            grid.changeSlot(slotNumber);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            slotNumber = 6;
            grid.changeSlot(slotNumber);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            slotNumber = 7;
            grid.changeSlot(slotNumber);
        }
    }

    void DoorFunction()
    {
        Door = Hit.transform.gameObject;
        if (Input.GetKeyUp(Keymap.Interact))
        {
            if (Door.transform.localRotation.y <= 0)
            { // Closed State at 0 degrees
                // OPEN DOOR
                openDoor = true;
                //Door.transform.localEulerAngles = new Vector3(0f, 100f, 0f);
            }
            else if (Door.transform.localRotation.y > 0)
            { // Opened State up to 100 degrees
                // CLOSE DOOR
                closeDoor = true;
                //Door.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            }
        }
    }

    void OpenDoor()
    {
        if (Door.transform.localEulerAngles.y <= 90f || Door.transform.localEulerAngles.y >= 350f)
        {
            Door.transform.localEulerAngles += new Vector3(0f, Time.deltaTime * doorSpeed, 0f);
        }
        else if (Door.transform.localEulerAngles.y > 90f)
        {
            openDoor = false;
            closeDoor = false;
            Door = null;
        }
    }

    void CloseDoor()
    {
        if (Door.transform.localEulerAngles.y >= 0f && Door.transform.localEulerAngles.y < 100f)
        {
            Door.transform.localEulerAngles -= new Vector3(0f, Time.deltaTime * doorSpeed, 0f);
        }
        else if (Door.transform.localEulerAngles.y < 0f)
        {
            openDoor = false;
            closeDoor = false;
            Door = null;
        }
    }

    bool CheckIfBlock(GameObject item) {
        return (item.GetComponent<ItemBlock>() != null);
    }

    GameObject prevItem;
    bool ItemDebugMonitor(GameObject item)
    {

        if (CheckIfBlock(item)) {
            return false;
        }

        bool newItem = false;
        if (prevItem == null || prevItem != item)
        {
            prevItem = item;
            newItem = true;
        }
        if (item.gameObject.GetComponent<ItemPickable>() != null)
        {
            if (item.gameObject.GetComponent<ItemPickable>().ItemIcon != null)
            {
                if (item.gameObject.GetComponent<ItemPickable>().ItemIcon.name != null)
                {
                    return true; // inner
                }
                else
                {
                    if (newItem)
                    {
                        Debug.LogError("Texture2D for ItemIcon variable not found");
                    }
                    return false;
                }
            }
            else
            {
                if (newItem)
                {
                    Debug.LogError("No Texture2D for ItemIcon variable attached!");
                }
                return false;
            }
        }
        else
        {
            return false;
        }
    }


}
