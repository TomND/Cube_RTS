using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class PanelButtonManager : MonoBehaviour, IPointerClickHandler {

    public GameObject panel;

    public void OnPointerClick(PointerEventData eventData) {
        PanelButtonManager[] p = (PanelButtonManager[])FindObjectsOfType(typeof(PanelButtonManager));
        for (int i = 0; i < p.Length; i++) {
            p[i].panel.SetActive(false);
        }
        panel.SetActive(true);
    }

}
