using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {

    public Camera camera;

    public float walkingSpeed;
    public float jumpingSpeed;
    public float mouseSensitivity;
    public float gravity;

    public float velocityLimit;

    private Rigidbody rb;
    private bool onGround = false;
    private bool pressedJump = false;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public static void LockCursor (bool state) {
        if (state) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Update() {

        if (Input.GetKeyUp(Keymap.DisableCursor)) {
            LockCursor(Cursor.visible);
        }
        if (!Cursor.visible) {
            if (RTSViewManager.CommanderMode) {
                MouseLook();
            } 
        }

    }

    void FixedUpdate() {

        if (RTSViewManager.CommanderMode) {
            Movement();    
        } 

    }

    void MouseLook() {

        float mouseHorizontal = Input.GetAxis("Mouse X");
        float mouseVertical = Input.GetAxis("Mouse Y");

        camera.transform.localEulerAngles = new Vector3(camera.transform.localEulerAngles.x + (mouseVertical * -mouseSensitivity), 
            camera.transform.localEulerAngles.y + (mouseHorizontal * mouseSensitivity), 0);

        transform.eulerAngles = new Vector3(0, 
            camera.transform.localEulerAngles.y + (mouseHorizontal * mouseSensitivity), 0);

    }

    void Movement() {
        /*float cameraX = camera.transform.localPosition.x;
        float cameraY = camera.transform.localPosition.y;
        float cameraZ = camera.transform.localPosition.z;*/

        if (Input.GetKey(KeyCode.W)) {
            rb.AddForce(camera.transform.forward * walkingSpeed);
        } else if (Input.GetKeyUp(KeyCode.W)){
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.A)) {
            rb.AddForce(-camera.transform.right * walkingSpeed);
        } else if (Input.GetKeyUp(KeyCode.A)) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.S)) {
            rb.AddForce(-camera.transform.forward * walkingSpeed);
        } else if (Input.GetKeyUp(KeyCode.S)) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.D)) {
            rb.AddForce(camera.transform.right * walkingSpeed);
        } else if (Input.GetKeyUp(KeyCode.D)) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.Space) && onGround) {
            rb.velocity = new Vector3(0, jumpingSpeed, 0);
            pressedJump = true;
        }

        if (!onGround) {
            if (Input.GetKey(KeyCode.W)) {
                rb.AddForce(-camera.transform.forward * rb.velocity.magnitude);
            }
            rb.AddForce(new Vector3(0, -1 * gravity, 0));
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, velocityLimit);

    }
     
    void OnCollisionEnter (Collision c) {
        pressedJump = false;
    }

    void OnCollisionStay(Collision c) {
        onGround = true;
    }

    void OnCollisionExit(Collision c) {
        onGround = false;
    }
	
}
