using UnityEngine;
using System.Collections;

public class Player_Build : MonoBehaviour {

    private Camera camera;

    public float maxDistance;
    RaycastHit lookingAt;
    LayerMask onlyBlocks = 1 << 8;

    public GameObject holding;

    void Start() {
        camera = gameObject.GetComponent<FirstPersonController>().camera;
    }

    public void SetHolding(GameObject holding) {
        this.holding = holding;
    }

    void Update() {

        if (holding != null) {
            Physics.Raycast(camera.transform.position, camera.transform.forward, out lookingAt, maxDistance, onlyBlocks);
            if (lookingAt.collider != null) {
                holding.GetComponent<BoxCollider>().enabled = false;
                holding.SetActive(true);
                Vector3 pointedBlock = lookingAt.collider.transform.position;
                Vector3 placeBlock = new Vector3(pointedBlock.x, pointedBlock.y + 1f, pointedBlock.z);
                holding.transform.position = placeBlock;
                if (Input.GetMouseButton(0)) {
                    Place(holding, placeBlock);
                } else if (Input.GetMouseButton(1)) {
                    holding.SetActive(false);
                    holding = null;
                }
            } else {
                holding.SetActive(false);
            }
        } else {
            Physics.Raycast(camera.transform.position, camera.transform.forward, out lookingAt, maxDistance, onlyBlocks);
            if (lookingAt.collider != null) {
                GameObject pointedBlock = lookingAt.collider.gameObject;
                if (Input.GetMouseButtonUp(0)) {
                    if (pointedBlock.GetComponent<BlockStats>() != null) {
                        BlockStats stats = pointedBlock.GetComponent<BlockStats>();
                        float damageDealt = 100f - stats.Durability;
                        stats.DamageHealth(damageDealt, true);
                    }
                }
            }
        }

    }

    void Place(GameObject obj, Vector3 pos) {
        GameObject toPlace = obj;
        holding = null;
        if (obj.name.Contains("Clone")) {
            obj.name = (obj.name).Substring(0, (obj.name).Length - 7);
        }
        toPlace.transform.SetParent(GameObject.Find("Map").transform);
        toPlace.transform.position = pos;
        toPlace.GetComponent<BoxCollider>().enabled = true;
    }



}

