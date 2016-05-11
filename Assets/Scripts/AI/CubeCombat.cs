using UnityEngine;
using System.Collections;

public class CubeCombat : MonoBehaviour {
   // Use this for initialization

   public float       shootDistance;
   public float       fireRate;
   public float       fireRateModifier;
   private float      nextFire;
   public GameObject  shootTarget;
   private Cube       cube;
   private GameObject laser;
   public float       sightDistance;

   void Start()
   {
      fireRateModifier = transform.localScale.x;
      nextFire         = 0;
      fireRate         = fireRate / fireRateModifier;
      cube             = GetComponent <Cube>();
      SetupRadius();
   }

   public void SetupRadius()
   {
      GetComponentInChildren <SphereCollider>().radius = sightDistance / transform.localScale.x;
   }

   // Update is called once per frame
   void Update()
   {
   }

   public void SetShootTarget(GameObject targ)
   {
      shootTarget = targ;
   }

   public GameObject GetShootTarget()
   {
      return(shootTarget);
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
               cube.SetFloorTarget(null, shootTarget.transform.position);
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
         SetupLaser(direction);
         ApplyLaserForce(laserForceDirection);
         }
   }

   void SetupLaser(Quaternion direction)
   {
      laser = CubePool.RemoveFromLaserPool();
      Laser laserProp = laser.GetComponent <Laser>();
      laserProp.SetTeam(cube.teamNumber);     //TODO: make method for this
      laserProp.SetSpawner(gameObject);
      laser.transform.position                    = transform.position;
      laser.transform.rotation                    = direction;
      laser.GetComponent <Laser>().enabled        = true;
      laser.GetComponent <MeshRenderer>().enabled = true;
      laser.GetComponent <BoxCollider>().enabled  = true;
      laser.SetActive(true);
   }

   void ApplyLaserForce(Vector3 laserForceDirection)
   {
      Laser     laserProp = laser.GetComponent <Laser>();
      Rigidbody laserRig  = laser.GetComponent <Rigidbody>();

      laserRig.AddForce(laserForceDirection * laserProp.GetSpeed());
   }

   void ShootOld()
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
