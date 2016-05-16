using UnityEngine;
using System.Collections;

public class BasicFollow : MonoBehaviour {
   private Rigidbody  rb;
   public bool        fol;
   public float       force1;
   public float       force2;
   public Vector3     target = new Vector3(0, 0, 0);
   private GameObject unit;
   public float       targetVelocity;
   public float       targetAcceleration;
   private bool       useForward;
   private bool       useRight;
   public float       torqueFactor;
   private GameObject targetObject;

   // Use this for initialization
   void Start()
   {
      //rb = gameObject.GetComponent <Rigidbody>();
   }

   // Update is called once per frame
   void Update()
   {
      /*if(fol){
       * Move();
       * }
       */
      SelectUnit();

      if(unit != null){
         selectTarget();
         Debug.DrawRay(unit.transform.position, unit.transform.forward * 10, Color.blue);
         Debug.DrawRay(unit.transform.position, -unit.transform.forward * 10, Color.black);
         Debug.DrawRay(unit.transform.position, unit.transform.right * 10, Color.gray);
         Debug.DrawRay(unit.transform.position, -unit.transform.right * 10, Color.magenta);
         Debug.DrawRay(unit.transform.position, unit.transform.up * 10, Color.white);
         Debug.DrawRay(unit.transform.position, -unit.transform.up * 10, Color.red);
         }
      if(targetObject != null){
        FollowTarget();
      }
   }

   void FollowTarget(){
     target = targetObject.transform.position;
   }

   void SelectUnit()
   {
      if(Input.GetButton("Fire1")){
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

         RaycastHit hit;
         if(Physics.Raycast(ray, out hit, 100)){
            GameObject hitObj = hit.collider.gameObject;
            if(hit.collider.gameObject.tag == "Unit"){
               unit = hitObj;
               rb   = unit.GetComponent <Rigidbody>();
               print(hit.collider.gameObject);
               }
            }
         }
   }

   void selectTarget()
   {
      if(Input.GetButton("Fire2")){
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

         RaycastHit hit;
         if(Physics.Raycast(ray, out hit, 100)){
            GameObject hitObj = hit.collider.gameObject;
            if(hit.collider.gameObject.tag == "Unit"){
               targetObject = hitObj;
               target = hitObj.transform.position;
               }
            else if(hit.collider.gameObject.tag == "Floor"){
                    target = hit.point;
                    }
            }
         }
      GoToTarget();
   }

   void GoToTarget()
   {
      if(Vector3.Distance(unit.transform.position, target) > 3 && target != new Vector3(0, 0, 0)){
         Move();
         }
   }

   void Move()
   {
      Vector3 direction = PickFace();
      Debug.DrawRay(unit.transform.position, direction * 20, Color.green);
      /*
       * print(direction);
       * print("forward" + Vector3.Angle(unit.transform.forward, target));
       * print("up" + Vector3.Angle(unit.transform.up, target));
       * print("right" + Vector3.Angle(unit.transform.right, target));
       * print("back" + Vector3.Angle(-unit.transform.forward, target));
       * print("down" + Vector3.Angle(-unit.transform.up, target));
       * print("left" + Vector3.Angle(-unit.transform.right, target));
       */
      ManageForce(direction);
      //ManageTorque();
   }

   void ManageForce(Vector3 direction)
   {
      direction.Normalize();
      if(rb.velocity.magnitude - targetVelocity < -0.5){
         direction = Vector3.Lerp(direction, direction * force1, targetAcceleration);
         rb.AddForce(direction, ForceMode.Acceleration);
         }
      else if(rb.velocity.magnitude - targetVelocity > 0.5){
              direction = Vector3.Lerp(direction, direction.normalized, targetAcceleration);
              rb.AddForce(direction, ForceMode.Acceleration);
              }
   }

/*   void ManageTorque()
   {
      //print("managingTorque");
      Vector3 torque = target - unit.transform.position;

      torque.Normalize();
      rb.AddTorque(torque * torqueFactor, ForceMode.Acceleration);
   }
*/
   /* private void CalculateDirection()
    * {
    *  float angle = CalculateAngle();
    * }
    */


   Vector3 PickFace()
   {// Somethng is wrong with these angles
      /*float right    = Vector3.Angle(unit.transform.right, target);
      float left     = Vector3.Angle(-unit.transform.right, target);
      float forward  = Vector3.Angle(unit.transform.forward, target);
      float backward = Vector3.Angle(-unit.transform.forward, target);
      float up       = Vector3.Angle(unit.transform.up, target);
      float down     = Vector3.Angle(-unit.transform.up, target);*/

      float right    = Vector3.Distance(unit.transform.position + unit.transform.right*10, target);
      float left     = Vector3.Distance(unit.transform.position + -unit.transform.right*10, target);
      float forward  = Vector3.Distance(unit.transform.position + unit.transform.forward*10, target);
      float backward = Vector3.Distance(unit.transform.position + -unit.transform.forward*10, target);
      float up       = Vector3.Distance(unit.transform.position + unit.transform.up*10, target);
      float down     = Vector3.Distance(unit.transform.position + -unit.transform.up*10, target);

      float[] directions =
      {
         right, left, forward, backward, up, down
      };

      float smallest = SmallestAngle(directions);

      if(smallest == right){
         return(unit.transform.right);
         }
      else if(smallest == left){
              return(-unit.transform.right);
              }
      else if(smallest == forward){
              return(unit.transform.forward);
              }
      else if(smallest == backward){
              return(-unit.transform.forward);
              }
      else if(smallest == up){
              return(unit.transform.up);
              }
      else if(smallest == down){
              return(-unit.transform.up);
              }
      else{
          print("NON SENSE");
          return(new Vector3(0, 0, 0));
          }
   }

   float SmallestAngle(float[] angles)
   {
      float smallest = angles[0];

      for(int i = 0; i < angles.Length; i++){
          if(angles[i] < smallest){
             smallest = angles[i];
             }
          }
      return(smallest);
   }
}
