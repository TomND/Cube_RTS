using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GenerateMapButton : MonoBehaviour {

    public GameObject objMaster;

    public void Clicked() {
        objMaster.GetComponent<GenerateMap>().Generate();
    }
	
}
