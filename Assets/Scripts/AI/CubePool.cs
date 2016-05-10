using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubePool : MonoBehaviour
{
   public int         size;
   private static List <GameObject> units = new List <GameObject>();

   // Use this for initialization
   void Start()
   {
      CreatePool();
   }

   // Update is called once per frame
   void Update()
   {
   }

   void CreatePool()
   {
      GameObject cube = (GameObject)Resources.Load("CubeDefault");

      for(int i = 0; i < size; i++){
          units.Add((GameObject)Instantiate(cube, new Vector3(0, -50, 0), Quaternion.identity));
          }
   }
	 //removes and returns first in list.
   public static GameObject RemoveFromPool()
   {
      GameObject first = units[0];

      units.RemoveAt(0);
      return(first);
    }

	 public static void AddToPool(GameObject unit){
        units.Add(unit);
        unit.SetActive(false);
	 }
}
