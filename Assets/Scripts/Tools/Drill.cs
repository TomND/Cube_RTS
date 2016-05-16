using UnityEngine;
using System.Collections;

public class Drill : MonoBehaviour {

    public GameObject Drill_Impact; 

    public float fromPos = -1.5f;
    public float toPos = -1f;

    GameObject point;
    public float drillSpeed;

    private float t;
    private bool reachedTop = false;

    void Start() {
        point = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        GameObject dust = (GameObject)Instantiate(Drill_Impact);
        dust.transform.SetParent(gameObject.transform.GetChild(0));
        dust.transform.localPosition = new Vector3(0, point.transform.localPosition.y - (point.transform.localScale.y / 2), 0);
    }

    void Update() {
        DrillAnimation();
    }


    void DrillAnimation() {
        if (t < 1 && !reachedTop) {
            t += Time.deltaTime * drillSpeed;
            point.transform.localPosition = new Vector3(0, Mathf.Lerp(fromPos, toPos, t), 0);
        }
        if (point.transform.localPosition.y == toPos) {
            reachedTop = true;
            t = 0;
        }
        if (t < 1 && reachedTop) {
            t += Time.deltaTime * drillSpeed;
            point.transform.localPosition = new Vector3(0, Mathf.Lerp(toPos, fromPos, t), 0);
        }
        if (point.transform.localPosition.y == fromPos) {
            reachedTop = false;
            t = 0;
        }
    }
       
        
	
}
