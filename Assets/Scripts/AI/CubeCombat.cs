using UnityEngine;
using System.Collections;

public class CubeCombat : MonoBehaviour {
   // Use this for initialization

   public float       shootDistance;
   public float       fireRate;
   public float       fireRateModifier;
   private float      nextFire;
   private GameObject shootTarget;
   private Cube       cube;

   void Start()
   {
      fireRateModifier = transform.localScale.x;
      nextFire         = fireRate;
      fireRate         = fireRate / fireRateModifier;
      cube             = GetComponent <Cube>();
			SetupRadius();
    }


	 public void SetupRadius(){
		 GetComponentInChildren <SphereCollider>().radius = shootDistance;
	 }

   // Update is called once per frame
   void Update()
   {
   }

   public void SetShootTarget(GameObject targ)
   {
      shootTarget = targ;
   }

   public float GetShootDistance()
   {
      return(shootDistance);
   }

   public bool CheckIfHaveTarget()
   {
      /* return true if there is a targetObject
       * false if not.
       */
      return(!(shootTarget == null));
   }

   float DistanceToTarget(GameObject target)
   {
      float distance = Vector3.Distance(transform.position, target.transform.position);

      return(distance);
   }

   private bool IsOtherDead(GameObject other)
   {
      if(other.GetComponent <Cube>().GetHealth() <= 0){
         return(true);
         }
      else{
          return(false);
          }
   }

   public void ManageCombat()
   {
      if(shootTarget != null){
         if(DistanceToTarget(shootTarget) < shootDistance){
            Shoot();
            if(IsOtherDead(shootTarget)){
               shootTarget = null;
               }
            }
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
         laserProp.SetTeam(cube.teamNumber);   //TODO: make method for this
         laserProp.SetSpawner(gameObject);
         Rigidbody laserRig = laser.GetComponent <Rigidbody>();
         laserRig.AddForce(laserForceDirection * laserProp.GetSpeed());
         //laser.transform.LookAt(targetObject.transform);
         }
   }
}
