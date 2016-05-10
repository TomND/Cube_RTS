using UnityEngine;
using System.Collections;

public class CubeAreaDetection : MonoBehaviour
{
   Cube       cube;
   CubeCombat cubeCombat;

   // Use this for initialization
   void Start()
   {
      cube       = GetComponent <Cube>();
      cubeCombat = GetComponent <CubeCombat>();
   }

   // Update is called once per frame
   void Update()
   {
   }

   void OnTriggerStay(Collider other)
   {
      if(this.enabled){
         if(cubeCombat.CheckIfHaveTarget() == false){
            if(IsValidTarget(other)){
               cube.SetUnitTarget(other.transform.gameObject);
               }
            }
         }
   }

   bool IsValidTarget(Collider other)
   {
      if(other.GetType() == typeof(BoxCollider) && other.tag == "Unit"){
         if(other.GetComponent <Cube>().GetTeamNumber() != cube.GetTeamNumber()){
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
