using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeController : MonoBehaviour
{
   /*
    *
    * This class controls cubes and manages the controls to use to control cubes.
    * Includes selecting, deselecting, etc..
    *
    * === Attributes ===
    *@type List<Cube>: cubes
    *      A list of cube objects
    *@type bool: debug
    *      Sets debugging on or off.
    */

   private List <Cube> cubes = new List <Cube>();
   public bool         debug;
   private LayerMask   mask;

   void Start()
   {
      mask = ~(1 << 8);
      Physics.IgnoreLayerCollision(9, 10);
        Physics.IgnoreLayerCollision(9, 9);
      //Physics.IgnoreLayerCollision(10,11);
   }

   // Update is called once per frame
   void Update()
   {
      SelectorManager();
   }

   void SelectorManager()
   {
      /*
       *  Manages The selection Methods, and when they can be used.
       */
      SelectUnit();
      if(cubes.Count != 0){
         SelectTarget();
         SetDebug();
         }
   }

   void SelectUnit()
   {
      /*
       * Selects units based on tag. uses raycast when a specific button is pressed
       * to find units and adds them into a list.
       */
      if(Input.GetButtonDown("Fire1")){
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

         RaycastHit hit;
         if(Physics.Raycast(ray, out hit, 100, mask)){
            GameObject hitObj = hit.collider.gameObject;
            if(hit.collider.gameObject.tag == "Unit"){
               CubeListManager(hitObj.GetComponent <Cube>());
               }
            else{
                cubes = new List <Cube>();
                }
            }
         }
   }

   void CubeListManager(Cube newCube)
   {
      /*
       * manages the addition and removal of values from the Cube list
       *@type Cube: newCube
       *      The cube object being added or removed.
       */
      if(cubes.Contains(newCube)){
         cubes.Remove(newCube);
         }
      else{
          cubes.Add(newCube);
          }
   }

   void SelectTarget()
   {
      /*
       * Using raycast during specific button inputs, uses tags to select targets
       * and manage the use of them.
       */
      if(Input.GetButtonDown("Fire2")){
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

         RaycastHit hit;
         if(Physics.Raycast(ray, out hit, 100, mask)){
            GameObject hitObj = hit.collider.gameObject;
            if(hit.collider.gameObject.tag == "Unit"){
               foreach(Cube subCube in cubes){
                   subCube.SetUnitTarget(hitObj);
                   }
               }
            else if(hit.collider.gameObject.tag == "Floor"){
                    foreach(Cube subCube in cubes){
                        subCube.SetFloorTarget(hitObj, hit.point);
                        }
                    }
            }
         }
   }

   void SetDebug()
   {
      /*
       * Enables of disables debug mode.
       */
      if(Input.GetButton("Debug")){
         foreach(Cube subCube in cubes){
             subCube.SetDebug(true);
             }
         }
   }
}
