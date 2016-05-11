using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubePool : MonoBehaviour
{
   public int size;
   public int LaserSize;
   private static List <GameObject> units  = new List <GameObject>();
   private static List <GameObject> lasers = new List <GameObject>();

   // Use this for initialization
   void Start()
   {
      CreatePool();
      CreateLaserPool();
   }

   void CreateLaserPool()
   {
      GameObject laser = (GameObject)Resources.Load("Laser");

      for(int i = 0; i < LaserSize; i++){
          lasers.Add((GameObject)Instantiate(laser, new Vector3(0, -50, 0), Quaternion.identity));
          }
   }

   public static GameObject RemoveFromLaserPool()
   {
      GameObject first = lasers[0];

      lasers.RemoveAt(0);
      return(first);
   }

   public static void AddToLaserPool(GameObject laser)
   {
        lasers.Add(laser);
        laser.GetComponent<Laser>().enabled = false;
        laser.GetComponent<MeshRenderer>().enabled = false;
        laser.GetComponent<BoxCollider>().enabled = false;
        //laser.SetActive(false);
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

   public static void AddToPool(GameObject unit)
   {
      units.Add(unit);
      unit.SetActive(false);
   }
}
