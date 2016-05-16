using UnityEngine;
using System.Collections;

public class RTSViewManager : MonoBehaviour {

    public static bool CommanderMode;

    void Start() {
        RTSViewManager.SetCommanderMode(true);
    }

    public static void SetCommanderMode(bool state) {
        RTSViewManager.CommanderMode = state;
        UI.root.BottomEconomicPanel.SetActive(!state);
        UI.root.QuickSlotPanel.SetActive(state);
    }

}
