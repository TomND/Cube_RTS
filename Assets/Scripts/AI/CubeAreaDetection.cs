using UnityEngine;
using System.Collections;

public class CubeAreaDetection : MonoBehaviour
{
   Cube cube;

   // Use this for initialization
   void Start()
   {
      cube = GetComponent <Cube>();
   }

   // Update is called once per frame
   void Update()
   {
   }

   void OnTriggerStay(Collider other)
   {
      if(cube.CheckIfHaveTarget() == false){
         if(IsValidTarget(other)){
            cube.SetUnitTarget(other.transform.parent.gameObject);
            }
         }
   }

   bool IsValidTarget(Collider other)
   {
      if(other.GetType() == typeof(SphereCollider) && other.tag == "UnitTrigger"){
         if(other.GetComponentInParent <Cube>().GetTeamNumber() != cube.GetTeamNumber()){
            return(true);
            }
         else{
             return(false);
             }
         }
      else{
          return(false);
          }
   }
}
