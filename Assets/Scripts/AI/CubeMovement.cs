using UnityEngine;
using System.Collections;

public class CubeMovement : MonoBehaviour
{
   private Rigidbody rb;
   public float      force;
   private Vector3   target = new Vector3(0, 0, 0);
   public float      targetVelocity;
   public float      targetAcceleration;
   private float     targetDistanceGoal = 2;
   private Cube      cube;


   // Use this for initialization
   void Start()
   {
      cube = GetComponent <Cube>();
      rb   = GetComponent <Rigidbody>();
   }

   // Update is called once per frame
   void Update()
   {
   }

   public void GetTarget(Vector3 targ)
   {
      target = targ;
    }

	 public float GetTargetDistance(){
        return targetDistanceGoal;
    }

   void ManageMovement()
   {
   }

   public void Move()
   {
      /*
       * Manages movement by setting direction and calling ManageForce()
       */
      Vector3 direction = PickFace();

      ManageForce(direction);
   }

   public void ManageForce(Vector3 direction)
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

   public Vector3 PickFace()
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
