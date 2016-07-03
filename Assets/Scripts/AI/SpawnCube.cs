using UnityEngine;
using System.Collections;

public class SpawnCube : MonoBehaviour {


    public int teamNumber;
    public int health;
    private GameObject unit;
    public Vector3 spawnPosition;
    public Material material;// the color of the cube 
    public float scale;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            spawnPosition = new Vector3(-16, 5, 96);
            Spawn();
        }
	}

    void Spawn()
    {
        unit = CubePool.RemoveFromPool();
        SetupAndEnable();
    }

    public void SetupAndEnable() { 

        unit.GetComponent<BoxCollider>().enabled = true;
        unit.GetComponent<MeshRenderer>().enabled = true;
        Cube c = unit.GetComponent<Cube>();
        c.health = health;     // TODO: Make this into a method
        c.SetTeamNumber(teamNumber);
        unit.GetComponent<MeshRenderer>().material = material;


        unit.GetComponent<CubeCombat>().SetupRadius();
        unit.transform.localScale = new Vector3(scale,scale,scale);

        foreach(MonoBehaviour mono in unit.GetComponents <MonoBehaviour>()){
            mono.enabled = true;
            }
        unit.layer = 10;
        unit.tag   = "Unit";
        unit.GetComponent <CubeCleanUp>().enabled = false;
        unit.GetComponentInChildren <SphereCollider>().enabled = true;
        unit.transform.position = spawnPosition;
        unit.SetActive(true);
    }
}
