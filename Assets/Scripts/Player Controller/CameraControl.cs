using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
   /*
    * manages the cameras controls. has two modes, Developer mode, and traditional
    * mode. Developer mode emulates the unity editor camera, traditional mode
    * is more of a RTS camera. The variable "DevControls" manages which
    * mode is being used.
    *
    * === Attributes ===
    *
    * @type float: angleMin
    *    Lower camera angle limit
    * @type float: angleMax
    *    Upper camera angle limit
    * @type float: maxHeight
    *    Max camera height
    * @type float: minHeight
    *    Min allowed camera height
    * @type float: currentHeight
    *    Current camera height, begins at the half way point
    * @type float: heightSmooth
    *    smoothening variable for height change
    * @type float: heightRotationSmooth
    *    smoothening variable for rotation change
    * @type float: heightScrollSmooth
    *    smothening variable for mouse scroll
    * @type float: tiltSmooth
    *    makes camera manual tilt smooth
    * @type float:moveVelocity
    *    speed camera moves at
    * @type float:moveAcceleration
    *    Camera acceleration rate
    * @type float:speedMultiplier
    *    multiplies speed by this value when the shift button is helf
    * @type bool: DevControls
    *    enables/disables developer controls
    * @type Transform: parentTransform
    *    transform of the parentTransform
    * #type Camera:
    *    The camera object
    *
    */
   public float      angleMin  = 0;
   public float      angleMax  = 45;
   public float      maxHeight = 20;
   public float      minHeight = 5;
   public float      currentHeight;
   public float      heightSmooth;
   public float      heightRotationSmooth;
   public float      heightScrollSmooth;
   public float      tiltSmooth = 2.0f;
   public float      tiltSpeed  = 30.0f;
   public float      moveVelocity;
   public float      moveAcceleration;
   public float      speedMultiplier;
   public bool       DevControls;
   private Transform parentTransform;
   private Camera    camera;


   // Use this for initialization
   void Start()
   {
      currentHeight   = maxHeight - minHeight;
      parentTransform = gameObject.GetComponentInParent <Transform>();
      camera          = gameObject.GetComponent <Camera>();
   }

// Update is called once per frame
   void Update()
   {
      MoveControls();
      CameraTiltControlDeveloper();
   }

   void MoveControls()
   {
      /*
       * manages movement controls.
       * Chooses between developer controls or traditional controls
       *
       * @rtype: Null
       */
      if(DevControls){
         DeveloperControls();
         CameraTiltControlDeveloper();
         }
      else{
          TraditionalControls();
          CameraTiltControlTraditional();
          }
   }

   void DeveloperControls()
   {
      /*
       * Contains developer controls.
       * Makes camera control like the unity editor camera
       *
       * @rtype: null
       */
      float multiplier = 1;

      if(Input.GetButton("Shift")){
         multiplier = speedMultiplier;
         }
      else{
          multiplier = 1;
          }
      //transform.position += new Vector3(Input.GetAxis("Horizontal") * moveVelocity, 0, Input.GetAxis("Vertical") * moveVelocity);
      Vector3 forward       = Camera.main.transform.forward * Input.GetAxis("Vertical") * moveVelocity * Time.deltaTime * multiplier;
      Vector3 scrollForward = Camera.main.transform.forward * Input.GetAxis("Mouse ScrollWheel") * moveVelocity * Time.deltaTime * 50;
      Vector3 right         = Camera.main.transform.right * Input.GetAxis("Horizontal") * moveVelocity * Time.deltaTime * multiplier;
      transform.position += forward + right + scrollForward;
   }

   void TraditionalControls()
   {
      /*
       * contains "traditional" controls.
       * Controls camera similar to a regular RTS game
       * @rtype: null
       */
      Vector3 forward = transform.forward * Input.GetAxis("Vertical") * moveVelocity * Time.deltaTime;
      //Vector3 scrollForward = transform.forward * Input.GetAxis("Mouse ScrollWheel") * moveVelocity * Time.deltaTime * 50;
      Vector3 right = transform.right * Input.GetAxis("Horizontal") * moveVelocity * Time.deltaTime;

      transform.position += forward + right;
   }

   void CameraTiltControlDeveloper()
   {
      /*
       * Manages developer tilt controls. allows user to tild camera rotation when Fire2 is pressed
       * using mouse X and mouse Y.
       *
       */
      if(Input.GetButton("MiddleMouseClick")){
         float      tiltAroundY = Input.GetAxis("Mouse X") * tiltSpeed * Time.deltaTime;
         float      tiltAroundX = -Input.GetAxis("Mouse Y") * tiltSpeed * Time.deltaTime;
         Quaternion target      = Quaternion.Euler(transform.eulerAngles.x + tiltAroundX, transform.eulerAngles.y + tiltAroundY, 0);
         transform.rotation = Quaternion.Slerp(transform.rotation, target, tiltSmooth);
         }
   }

   void CameraTiltControlTraditional()
   {
      /*
       * more traditional camera tilt controls. tilts the camera only in the X axis manually
       * also calls heightManager and HeightRotationManager
       */
      heightManager();
      HeightRotationManager();
      if(Input.GetButton("MiddleMouseClick")){
         float      tiltAroundY = Input.GetAxis("Mouse X") * tiltSpeed * Time.deltaTime;
         Quaternion target      = Quaternion.Euler(0, transform.eulerAngles.y + tiltAroundY, 0);
         transform.rotation = Quaternion.Slerp(transform.rotation, target, tiltSmooth);
         }
   }

   void heightManager()
   {
      /*
       * manages the cameras height.
       * Mouse ScrollWheel changes value of currentHeight,
       * while raycast mantains currentHeight at a smooth rate
       */
      if(Input.GetAxis("Mouse ScrollWheel") < 0){
         currentHeight = Mathf.Lerp(currentHeight, minHeight, -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 100);
         }
      else if(Input.GetAxis("Mouse ScrollWheel") > 0){
              currentHeight = Mathf.Lerp(currentHeight, maxHeight, Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 100);
              }

      float      distanceToGround = 0;
      Vector3    ground           = new Vector3(0, 0, 0);
      RaycastHit hit;

      if(Physics.Raycast(transform.position, -Vector3.up, out hit)){
         distanceToGround = hit.distance;
         ground           = hit.point;
         }
      else{
          distanceToGround = 100;
          }
      if(distanceToGround - currentHeight > 0.5){
         transform.position = Vector3.Lerp(transform.position, ground, Time.deltaTime * heightSmooth);
         }
      else if(distanceToGround - currentHeight < -0.5){
              Vector3 above = new Vector3(transform.position.x, transform.position.y + maxHeight + 1, transform.position.z);
              transform.position = Vector3.Lerp(transform.position, above, Time.deltaTime * heightSmooth);
              }
   }

   void HeightRotationManager()
   {
      /*
       * calculates correct ratio for the rotation.
       */
      float heightRatio     = currentHeight / (maxHeight - minHeight);
      float desiredRotation = (angleMax - angleMin) * heightRatio;

      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(desiredRotation, transform.eulerAngles.y, transform.eulerAngles.z), Time.deltaTime * heightRotationSmooth);
   }
}
