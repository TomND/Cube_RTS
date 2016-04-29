using UnityEngine;
using System.Collections;

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

   void ManageGoals()
   {
      /*
       * Sets the cubes goals, including movement and shooting goals.
       */

      if(targetObject != null){
         if(Vector3.Distance(transform.position, target) < targetDistanceGoal){
            targetObject = null;
            }
         else{
             Move();
             }
         }

      /*
       * INSERT SHOOTING CODE HERE
       */
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
       *Finds the face of the cube that is closest to the target, and chooses that face
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
				Takes a list of floats and returns the smallest value
				@type float[]: angles
						a list of floats representing angles
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
