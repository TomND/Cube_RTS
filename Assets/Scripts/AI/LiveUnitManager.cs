using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/* manages all living units, returns into on closest unit to another unit. */

public class LiveUnitManager : MonoBehaviour {


    public List<GameObject> team0Units = new List<GameObject>();
    public List<GameObject> team1Units = new List<GameObject>();

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        RemoveTheDead();
    }


    // removes dead untis from living lists
    void RemoveTheDead()
    {
        for(int i = 0; i < team0Units.Count; i++)
        {
            GameObject unit = team0Units[i];
            if (unit.GetComponent<Cube>().health <= 0)
            {
                team0Units.Remove(unit);
            }
        }
        for (int i = 0; i < team1Units.Count; i++)
        {
            GameObject unit = team1Units[i];
            if (unit.GetComponent<Cube>().health <= 0)
            {
                team1Units.Remove(unit);
            }
        }

    }

    void Remove(List<GameObject> list, List<GameObject> remove)
    {
        
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Unit")
        {
            Cube cube = other.GetComponent<Cube>();
            if(cube.teamNumber == 0)
            {
                team0Units.Add(other.gameObject);
            }
            else if(cube.teamNumber == 1)
            {
                team1Units.Add(other.gameObject);
            }
            other.GetComponent<CubeAreaDetection>().unitManager = this;
        }
    }
}
