using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class HubButtonManager : MonoBehaviour, IPointerClickHandler {

    void Start() {
        UI.root.ActiveCrafting.SetActive(true);
        UI.root.CraftingPanel.SetActive(true);
        UI.root.ActiveProfile.SetActive(false);
        UI.root.ProfilePanel.SetActive(false);
        UI.root.ActiveFriendsList.SetActive(false);
        UI.root.FriendsListPanel.SetActive(false);
        UI.root.ActiveSettings.SetActive(false);
        UI.root.SettingsPanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (transform.name.Contains("Craft")) {
            UI.root.ActiveCrafting.SetActive(true);
            UI.root.CraftingPanel.SetActive(true);
            UI.root.ActiveProfile.SetActive(false);
            UI.root.ProfilePanel.SetActive(false);
            UI.root.ActiveFriendsList.SetActive(false);
            UI.root.FriendsListPanel.SetActive(false);
            UI.root.ActiveSettings.SetActive(false);
            UI.root.SettingsPanel.SetActive(false);
        } else if (transform.name.Contains("Profile")) {
            UI.root.ActiveCrafting.SetActive(false);
            UI.root.CraftingPanel.SetActive(false);
            UI.root.ActiveProfile.SetActive(true);
            UI.root.ProfilePanel.SetActive(true);
            UI.root.ActiveFriendsList.SetActive(false);
            UI.root.FriendsListPanel.SetActive(false);
            UI.root.ActiveSettings.SetActive(false);
            UI.root.SettingsPanel.SetActive(false);
        } else if (transform.name.Contains("Friends")) {
            UI.root.ActiveCrafting.SetActive(false);
            UI.root.CraftingPanel.SetActive(false);
            UI.root.ActiveProfile.SetActive(false);
            UI.root.ProfilePanel.SetActive(false);
            UI.root.ActiveFriendsList.SetActive(true);
            UI.root.FriendsListPanel.SetActive(true);
            UI.root.ActiveSettings.SetActive(false);
            UI.root.SettingsPanel.SetActive(false);
        } else if (transform.name.Contains("Settings")) {
            UI.root.ActiveCrafting.SetActive(false);
            UI.root.CraftingPanel.SetActive(false);
            UI.root.ActiveProfile.SetActive(false);
            UI.root.ProfilePanel.SetActive(false);
            UI.root.ActiveFriendsList.SetActive(false);
            UI.root.FriendsListPanel.SetActive(false);
            UI.root.ActiveSettings.SetActive(true);
            UI.root.SettingsPanel.SetActive(true);
        }
    }

}
