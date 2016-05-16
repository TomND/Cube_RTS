using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DebugUI : MonoBehaviour, IPointerClickHandler {

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        print(eventData.pointerPress.transform.name);
    }

}
