using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeAreaDetection : MonoBehaviour
{
   Cube                      cube;
   CubeCombat                cubeCombat;
   public float              targetCheckRate;
   private float             currentTime = 0;
    private List<GameObject> enemies = new List<GameObject>();
    private SphereCollider sphere;

    // Use this for initialization
    void Start()
   {
      targetCheckRate = Random.Range(1, 3);
      cube            = GetComponent <Cube>();
        cubeCombat = GetComponent<CubeCombat>();
        sphere = GetComponent<SphereCollider>();
    }

   // Update is called once per frame
   void Update()
   {
      ManageEnemies();
   }

   void ManageEnemies()
   {
      CheckForDead();
      if(EnemyInRange()){
            if (cubeCombat.CheckIfHaveTarget() == false)
            {
                cube.SetUnitTarget(FindClosestTarget());
            }
         else{
             CheckForCloserUnit();
             }
        }

   }


   void CheckAlly(){

   }

   bool EnemyInRange()
   {
      if(enemies.Count > 0){
         return(true);
         }
      else{
          return(false);
          }
   }

   void CheckForDead()
   {
      for(int i = 0; i < enemies.Count; i++){
          if(enemies[i] != null){
             if(enemies[i].GetComponent <Cube>().GetHealth() <= 0){
                enemies.Remove(enemies[i]);
                }
             }
          }
   }

   void CheckForCloserUnit()
   {
      if(Time.time > currentTime){
         currentTime += targetCheckRate;


         GameObject closest = enemies[0];
         Vector3    closestVector;
         if(closest == null){
            closestVector = new Vector3(1000, 1000, 1000);
            }
         else{
             closestVector = closest.transform.position;
             }


         for(int i = 0; i < enemies.Count; i++){
             if(enemies[i] != null){
                Vector3 distCheck = enemies[i].transform.position;
                if(Vector3.Distance(distCheck, transform.position) < Vector3.Distance(transform.position, closestVector)){
                   closest = enemies[i];
                   }
                }
             }

         if(closest != cubeCombat.GetShootTarget()){
            cube.SetUnitTarget(FindClosestTarget());
            }
         else{
             return;
             }
         }
      //return(closest);
   }

   void OnTriggerEnter(Collider other)
   {
      if(this.enabled){
         if(IsValidTarget(other)){
            enemies.Add(other.gameObject);
            }
         }
   }

   void OnTriggerExit(Collider other)
   {
      if(enemies.Contains(other.gameObject)){
         enemies.Remove(other.gameObject);
         }
   }

   //TODO:implement faster Search Algorithim
   GameObject FindClosestTarget()
   {
      GameObject closest = enemies[0];
      Vector3    closestVector;

      if(closest == null){
         closestVector = new Vector3(1000, 1000, 1000);
         }
      else{
          closestVector = closest.transform.position;
          }


      for(int i = 0; i < enemies.Count; i++){
          if(enemies[i] != null){
             Vector3 distCheck = enemies[i].transform.position;
             if(Vector3.Distance(distCheck, transform.position) < Vector3.Distance(transform.position, closestVector)){
                closest = enemies[i];
                }
             }
          }

      return(closest);
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
