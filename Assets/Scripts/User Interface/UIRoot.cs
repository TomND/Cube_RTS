using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIRoot : MonoBehaviour {

    // *UI Elements*

    // Texts
    public Text txtGoldVar;
    public Text txtMunitionVar;
    public Text txtFuelVar;

    // - Panels
    public GameObject TopEconomicPanel;
    public GameObject BottomEconomicPanel;
    public GameObject EconomicPanel;
    public GameObject MasterPanel;
    public GameObject StoragePanel;
    public GameObject StructurePanel;
    public GameObject HubPanel;
    public GameObject PlayerPanel;
    public GameObject InventoryPanel;
    public GameObject QuickSlotPanel;
    public GameObject GearPanel;
    
    // Hub Panels
    public GameObject CraftingPanel;
    public GameObject ProfilePanel;
    public GameObject FriendsListPanel;
    public GameObject SettingsPanel;

    // - Grids
    public GameObject InventorySlotGrid;
    public GameObject QuickSlotGrid;
    public GameObject StorageSlotGrid;
    public GameObject CraftingListGrid;
    public GameObject CraftingInfoGrid;

    // Buttons
    public GameObject btnExit;
    public GameObject btnCraft;
    public GameObject ActiveCrafting;
    public GameObject ActiveProfile;
    public GameObject ActiveFriendsList;
    public GameObject ActiveSettings;

    // Transforms
    public Transform Storage_InventoryPos;
    public Transform StructureIncomplete_InventoryPos;

    // - Other
    public Text numberOfItems;
    public Text StorageNameText;
    public Text StructureNameText;
    public GameObject StructureCompletionBar;

    // Quantity Box
    public GameObject crafting_btnSubtractQty;
    public GameObject crafting_btnAddQty;
    public GameObject crafting_qtyText;

    // Notification Box
    public GameObject NotificationBox;
    public Text notification_txtItemName;
    public Text notification_txtItemQuantity;
    public RawImage notification_imgItem;

    // General Notification Box
    public GameObject GeneralNotificationBox;
    public Text generalNotification_txtMessage;

// =====================================

    // Player Parts
    //public GameObject DefaultArms;

    // Scripts
    public PlayerInventory playerInventory;
    public PlayerStats playerStats;

    // Icons
    public Texture2D HandGrabIcon;
    public Texture2D Checkmark;
    public Texture2D XMark;

    // Materials & Shaders
    public Material BlankMat;
    public Shader ObjectOutlineShader;
	
}
