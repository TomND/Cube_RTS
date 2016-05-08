using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cube : MonoBehaviour
{
   /*
    *          Cube object. Contains all properties of a cubes. Manages Movement, shooting,
    *          and all other potential tasks that cubes do.
    *
    *
    *          === Attributes ===
    *
    *          @type Rigidbody: rb
    *                                  The Rigidbody of the cube
    *          @type float: force
    *                                  Force applied to the cube to move
    *          @type Vector3: target
    *                                  The Vector3 position of the target
    *          @type float: targetVelocity
    *                                  The max velocity to move to the target at
    *          @type float: targetAcceleration
    *                                  The acceleration at which to move towards the target at
    *          @type GameObject: targetObject
    *                                  The gameObject of the target
    *          @type float: targetDistanceGoal
    *                                  The distance to the target at which the cube stops applying force
    *          @type bool: debug
    *                                  Sets whether to enable/disable debug tools
    */


   private Rigidbody  rb;
   public float       force;
   private Vector3    target = new Vector3(0, 0, 0);
   public float       targetVelocity;
   public float       targetAcceleration;
   private GameObject targetObject;
   private float      targetDistanceGoal = 2;
   private bool       debug = false;
   public float       shootDistance;
   public float       fireRate;
   private float      nextFire = 0;
   public float       teamNumber;
   public float       health;
   private GameObject shootTarget;

   // Use this for initialization
   void Start()
   {
      rb = gameObject.GetComponent <Rigidbody>();
   }

   // Update is called once per frame
   void Update()
   {
      ManageGoals();
   }

   public bool CheckIfHaveTarget()
   {
      return(!(targetObject == null));
   }

   public float GetTeamNumber()
   {
      return(teamNumber);
   }

   void Death(Vector3 vel = default(Vector3))
   {
      gameObject.GetComponent <BoxCollider>().enabled  = false;
      gameObject.GetComponent <MeshRenderer>().enabled = false;
      GameObject        deathCube = (GameObject)Resources.Load("DeathCube");
      List <GameObject> cubes     = new List <GameObject>();
      deathCube.GetComponent <MeshRenderer>().material = GetComponent <MeshRenderer>().material;
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(0.25f, -0.25f, 0.25f), Quaternion.identity));
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(0.5f, -0.25f, -0.25f), Quaternion.identity));
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(-0.25f, -0.25f, 0.25f), Quaternion.identity));
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(-0.25f, -0.25f, 0.25f), Quaternion.identity));
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(-0.25f, 0.25f, 0.25f), Quaternion.identity));
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(0.25f, 0.25f, 0.25f), Quaternion.identity));
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(0.25f, 0.25f, 0.25f), Quaternion.identity));
      cubes.Add((GameObject)Instantiate(deathCube, transform.position + new Vector3(-0.25f, 0.25f, -0.25f), Quaternion.identity));
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
      target       = tarObj.transform.position;
      shootTarget  = targetObject;
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
      target       = tarVec;
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

   private float DistanceToTarget()
   {
      float distance = Vector3.Distance(transform.position, target);

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
         if(TargetIsUnit()){
            if(DistanceToTarget() > shootDistance){
               Move();
               }
            }
         else{
             if(DistanceToTarget() > targetDistanceGoal){
                Move();
                }
             }


         /*
          * INSERT SHOOTING CODE HERE
          */

         if(shootTarget != null){
            ManageCombat();
            }
         }
   }

   bool TargetIsUnit()
   {
      if(targetObject.tag == "Unit"){
         return(true);
         }
      else{
          return(false);
          }
   }

   void ManageCombat()
   {
      if(DistanceToTarget() < shootDistance){
         if(targetObject.tag == "Unit"){
            Shoot();
            }
         if(IsOtherDead(shootTarget)){
            shootTarget = null;
            }
         }
   }

   private bool IsOtherDead(GameObject other)
    {
        print(other.gameObject);
        if(other.GetComponent <Cube>().GetHealth() <= 0){
         return(true);
         }
      else{
          return(false);
          }
   }

   void Shoot()
   {
      if(Time.time > nextFire){
         nextFire = Time.time + fireRate;

         Quaternion direction;
         Vector3    angle = shootTarget.transform.position - transform.position;
         Vector3    laserForceDirection = shootTarget.transform.position - transform.position;
         laserForceDirection.Normalize();
         direction = Quaternion.LookRotation(shootTarget.transform.position - transform.position);
         GameObject laser     = (GameObject)Instantiate(Resources.Load("Laser"), transform.position, direction);
         Laser      laserProp = laser.GetComponent <Laser>();
         laserProp.SetSpawner(gameObject);
         Rigidbody laserRig = laser.GetComponent <Rigidbody>();
         laserRig.AddForce(laserForceDirection * laserProp.GetSpeed());
         //laser.transform.LookAt(targetObject.transform);
         }
   }

   void Move()
   {
      /*
       * Manages movement by setting direction and calling ManageForce()
       */
      Vector3 direction = PickFace();

      ManageForce(direction);
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

   void ManageForce(Vector3 direction)
   {
      /*
       *                      Manages the force applied to the cube
       *                      @type Vector3: direction
       *                                              The direction that force should be applied to.
       */
      direction.Normalize();
      if(rb.velocity.magnitude - targetVelocity < -0.5){
         direction = Vector3.Lerp(direction, direction * force, targetAcceleration);
         rb.AddForce(direction, ForceMode.Acceleration);
         }
      else if(rb.velocity.magnitude - targetVelocity > 0.5){
              direction = Vector3.Lerp(direction, direction.normalized, targetAcceleration);
              rb.AddForce(direction, ForceMode.Acceleration);
              }
   }

   Vector3 PickFace()
   {
      /*
       * Finds the face of the cube that is closest to the target, and chooses that face
       *@rtype: Vector3
       */
      float right    = Vector3.Distance(transform.position + transform.right * 10, target);
      float left     = Vector3.Distance(transform.position + -transform.right * 10, target);
      float forward  = Vector3.Distance(transform.position + transform.forward * 10, target);
      float backward = Vector3.Distance(transform.position + -transform.forward * 10, target);
      float up       = Vector3.Distance(transform.position + transform.up * 10, target);
      float down     = Vector3.Distance(transform.position + -transform.up * 10, target);

      float[] directions =
      {
         right, left, forward, backward, up, down
      };

      float smallest = SmallestAngle(directions);

      if(smallest == right){
         return(transform.right);
         }
      else if(smallest == left){
              return(-transform.right);
              }
      else if(smallest == forward){
              return(transform.forward);
              }
      else if(smallest == backward){
              return(-transform.forward);
              }
      else if(smallest == up){
              return(transform.up);
              }
      else if(smallest == down){
              return(-transform.up);
              }
      else{
          print("NON SENSE");
          return(new Vector3(0, 0, 0));
          }
   }

   float SmallestAngle(float[] angles)
   {
      /*
       *                      Takes a list of floats and returns the smallest value
       *                      @type float[]: angles
       *                                      a list of floats representing angles
       */
      float smallest = angles[0];

      for(int i = 0; i < angles.Length; i++){
          if(angles[i] < smallest){
             smallest = angles[i];
             }
          }
      return(smallest);
   }
}
