using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cube : MonoBehaviour
{
   /*
    *          Cube object. Manages all propoerties of cubes. The central brain so to speak.
    *
    *
    *          === Attributes ===
    *
    *          @type GameObject: targetObject
    *                                  The Target to move towards
    *          @type bool: debug
    *                                  Sets whether to enable/disable debug tools
    *          @type float: teamNumber
    *                                  The Cubes Team
    *          @type float: health
    *                                  Cubes health. dead when <= 0
    *          @type CubeMovement: cubeMovement
    *                                  CubeMovement Game object
    *          @type CubeCombat: cubeCombat
    *                                  cubeCombat game object.
    */


   private GameObject        targetObject;
   private bool              debug = false;
   public int                teamNumber;
   public float              health;
   private CubeMovement      cubeMovement;
   private CubeCombat        CubeCombat;
    private List<GameObject> subCubes = new List<GameObject>();
    public bool boom;

    // Use this for initialization
    void Start()
   {
      cubeMovement = GetComponent <CubeMovement>();
      CubeCombat   = GetComponent <CubeCombat>();
   }

   // Update is called once per frame
   void FixedUpdate()
   {
        ManageGoals();
   }


   public int GetTeamNumber()
   {
      /* returns the teamnumber
       */
      return(teamNumber);
   }

   public void SetTeamNumber(int t)
   {
      teamNumber = t;
   }

   void GetSubCubes()
   {
      subCubes = new List <GameObject>();
      for(int i = 0; i < 8; i++){
          subCubes.Add(CubePool.RemoveFromPool());
          }
      //gameObject.SetActive(false);
   }

   void PrepSubCubesDefaults()
   {
      foreach(GameObject sub in subCubes){
          sub.GetComponent <BoxCollider>().enabled  = true;
          sub.GetComponent <MeshRenderer>().enabled = true;
          Cube c = sub.GetComponent <Cube>();
          c.health = 100;     // TODO: Make this into a method
          c.SetTeamNumber(GetTeamNumber());
          sub.GetComponent <MeshRenderer>().material = gameObject.GetComponent <MeshRenderer>().material;
          sub.GetComponent <CubeCombat>().SetupRadius();
          sub.transform.localScale = gameObject.transform.localScale / 2;
          }
   }

   void PrepDeathSubCube()
   {
      foreach(GameObject sub in subCubes){
          foreach(MonoBehaviour mono in sub.GetComponents <MonoBehaviour>()){
              mono.enabled = false;
              }

          sub.layer = 9;
          sub.GetComponent <CubeCleanUp>().enabled = true;
          sub.tag = "Untagged";
          //sub.GetComponent <BoxCollider>().enabled = false;  // temp and temp below
          sub.GetComponentInChildren <SphereCollider>().enabled = false;
          }
   }

   void PrepRegularSubCube()
   {
      foreach(GameObject sub in subCubes){
          foreach(MonoBehaviour mono in sub.GetComponents <MonoBehaviour>()){
              mono.enabled = true;
              }
          sub.layer = 10;
          sub.tag   = "Unit";
          sub.GetComponent <CubeCleanUp>().enabled = false;
          }
   }

   void PositionSubCubes(float pos)
   {
      subCubes[0].transform.position = transform.position + new Vector3(pos, pos, pos);
      subCubes[1].transform.position = transform.position + new Vector3(pos, -pos, pos);
      subCubes[2].transform.position = transform.position + new Vector3(pos, pos, -pos);
      subCubes[3].transform.position = transform.position + new Vector3(pos, -pos, -pos);
      subCubes[4].transform.position = transform.position + new Vector3(-pos, pos, pos);
      subCubes[5].transform.position = transform.position + new Vector3(-pos, -pos, pos);
      subCubes[6].transform.position = transform.position + new Vector3(-pos, pos, -pos);
      subCubes[7].transform.position = transform.position + new Vector3(-pos, -pos, -pos);
   }

   void EnableSubCubes()
   {
      foreach(GameObject sub in subCubes){
          if(sub.tag == "Unit"){
             sub.SetActive(true);
             }
          else{
              //subCubes[0].SetActive(true);
              subCubes[3].SetActive(true);
              subCubes[5].SetActive(true);
              }
          }
   }

   void GiveSubsVelocity(Vector3 vel = default(Vector3))
   {
      foreach(GameObject sub in subCubes){
          sub.GetComponent <Rigidbody>().AddForce(vel * 10);
          //cube.GetComponent<Cube>().teamNumber = teamNumber;
          }
   }

   void Death(Vector3 vel = default(Vector3))
   {
      gameObject.GetComponent <BoxCollider>().enabled  = false;
      gameObject.GetComponent <MeshRenderer>().enabled = false;
      float   newScale       = transform.localScale.x / 2;
      Vector3 newVectorScale = transform.localScale / 2;
      GetSubCubes();
      PrepSubCubesDefaults();
      if(newScale < 1){
         PrepDeathSubCube();
         }
      else{
          PrepRegularSubCube();
          }
      PositionSubCubes(newScale);
      EnableSubCubes();
      CubePool.AddToPool(gameObject);
      GiveSubsVelocity(vel);
   }

   void DeathOld(Vector3 vel = default(Vector3))
   {
      gameObject.GetComponent <BoxCollider>().enabled  = false;
      gameObject.GetComponent <MeshRenderer>().enabled = false;
      float      newScale       = transform.localScale.x / 2;
      Vector3    newVectorScale = transform.localScale / 2;
      GameObject deathCube;
      if(newScale < 1){
         deathCube = (GameObject)Resources.Load("DeathCube");
         deathCube.transform.localScale = newVectorScale;
         deathCube.GetComponent <MeshRenderer>().material = GetComponent <MeshRenderer>().material;
         deathCube.layer = 9;
         }
      else{
          deathCube = gameObject;
          deathCube.GetComponent <BoxCollider>().enabled  = true;
          deathCube.GetComponent <MeshRenderer>().enabled = true;
          deathCube.GetComponent <Cube>().health          = 100; // TODO: Make this into a method
          deathCube.transform.localScale = deathCube.transform.localScale / 2;
          deathCube.layer = 10;
          }
      List <GameObject> cubes = new List <GameObject>();
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(newScale, newScale, newScale), Quaternion.identity));
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(newScale, -newScale, newScale), Quaternion.identity));
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(newScale, newScale, -newScale), Quaternion.identity));
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(newScale, -newScale, -newScale), Quaternion.identity));
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(-newScale, newScale, newScale), Quaternion.identity));
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(-newScale, -newScale, newScale), Quaternion.identity));
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(-newScale, newScale, -newScale), Quaternion.identity));
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(-newScale, -newScale, -newScale), Quaternion.identity));
      foreach(GameObject cube in cubes){
          cube.GetComponent <Rigidbody>().AddForce(vel * 10);
          //cube.GetComponent<Cube>().teamNumber = teamNumber;
          }
      Destroy(gameObject);
   }

   public void TakeDamage(float damage, Vector3 vel = default(Vector3))
   {
      health -= damage;
      if(health <= 0){
         Death(vel);
         }
   }

   public float GetHealth()
   {
      return(health);
   }

   public void SetUnitTarget(GameObject tarObj)
   {
      /*
       * Sets the targetObject and the target Vector3 position if the target is a unit
       *
       * @type gameObject: tarObj
       *        The Target gameObject
       * @rtype: Null
       *
       */

      targetObject = tarObj;
      cubeMovement.GetTarget(tarObj.transform.position);
      CubeCombat.SetShootTarget(targetObject);
   }

   public void SetFloorTarget(GameObject tarObj, Vector3 tarVec)
   {
      /*
       * Sets the targetObject and target Vector3 if the Target is the floor
       * @type GameObject: tarObj
       *       the Target GameObject
       * @type Vector3: tarVec
       *       The Target Vector3 position
       */
      targetObject = tarObj;
      cubeMovement.GetTarget(tarVec);
   }

   public void SetDebug(bool option)
   {
      /*
       * Sets the value of Debug to either true or false.
       *@type bool: option
       *      bool value given to debug. sets debug on or off
       *
       */
      debug = option;
   }

   public float DistanceToTarget(GameObject target)
   {
      float distance = Vector3.Distance(transform.position, target.transform.position);

      return(distance);
   }

   void ManageGoals()
   {
      /*
       * Sets the cubes goals, including movement and shooting goals.
       */

      if(targetObject == gameObject){
         targetObject = null;
         }

      if(targetObject != null){
         if(TargetIsUnit(targetObject)){
            if(DistanceToTarget(targetObject) > CubeCombat.GetShootDistance()){
               cubeMovement.Move();
               }
            }
         else{
             if(DistanceToTarget(targetObject) > cubeMovement.GetTargetDistance()){
                cubeMovement.Move();
                }
             }

         /*
          * INSERT SHOOTING CODE HERE
          */


         CubeCombat.ManageCombat();
         }
   }

   public bool TargetIsUnit(GameObject targ)
   {
      if(targ.tag == "Unit"){
         return(true);
         }
      else{
          return(false);
          }
   }

   void DebugRays(Vector3 direction)
   {
      /*
       * Draws debug rays to help debug the program.
       *
       *@type Vector3: direction
       *	    The direction value the cube moves in
       */
      Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);
      Debug.DrawRay(transform.position, -transform.forward * 10, Color.black);
      Debug.DrawRay(transform.position, transform.right * 10, Color.gray);
      Debug.DrawRay(transform.position, -transform.right * 10, Color.magenta);
      Debug.DrawRay(transform.position, transform.up * 10, Color.white);
      Debug.DrawRay(transform.position, -transform.up * 10, Color.red);
      Debug.DrawRay(transform.position, direction * 20, Color.green);
   }
}
