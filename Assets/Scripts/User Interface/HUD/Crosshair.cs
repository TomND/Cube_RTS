using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

    public GameObject CrosshairImage;
    public static bool Show = true;

    void Start() {
        CrosshairImage.SetActive(Show);
    }

    public static void SetCrosshair(bool state) {
        Crosshair.Show = state;
    }

    void FixedUpdate() {
        CrosshairImage.SetActive(Crosshair.Show);
    }

}

