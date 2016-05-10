using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Laser : MonoBehaviour
{
   public float       speed; // make private
   public float       DespawnTime;
   private float      spawnTime;
   private GameObject Spawner;// the object that spawned me
   private Rigidbody  rb;
   public float       impactForce;
   public int         damage;
   public int         team;
   public int         cubeAmountMod;// reduce/increase debris

   // Use this for initialization
   void Start()
   {
      spawnTime = Time.time;
      rb        = GetComponent <Rigidbody>();
   }

   // Update is called once per frame
   void Update()
   {
      Despawn();
   }

   public void SetTeam(int t)
   {
      team = t;
   }

   public int GetTeam()
   {
      return(team);
   }

   public float GetSpeed()
   {
      return(speed);
   }

   void Despawn()
   {
      if(Time.time > spawnTime + DespawnTime){
         Destroy(gameObject);
         }
   }

   public void SetSpawner(GameObject theSpawner)
   {
      Spawner = theSpawner;
   }

   void OnTriggerEnter(Collider other)
   {
      if(rb != null){ //TODO: this if statement is temporary, not a real solution
         if(Spawner != null){
            if(other.transform.tag == "Unit" && other.gameObject != Spawner && other.GetType() == typeof(BoxCollider) && OnDifferentTeam(other.gameObject)){
               GameObject hitUnit = other.gameObject;
               Cube       hitCube = hitUnit.GetComponent <Cube>();
               hitCube.TakeDamage(damage, rb.velocity);
               Rigidbody hitRig = hitUnit.GetComponent <Rigidbody>();
               hitRig.AddForce(rb.velocity.normalized * impactForce, ForceMode.Impulse);
               Destroy(gameObject);
               }
            }
         if(other.transform.tag == "Laser" && OnDifferentTeam(other.gameObject)){
            LaserCollision();
            }
         }
   }

   bool OnDifferentTeam(GameObject other)
   {
      int t = 99;

      if(other.tag == "Unit"){
         Cube c = other.GetComponent <Cube>();
         t = c.GetTeamNumber();
         }
      else if(other.tag == "Laser"){
              Laser c = other.GetComponent <Laser>();
              t = c.GetTeam();
              }

      if(team != t){
         return(true);
         }
      else{
          return(false);
          }
   }

   void LaserCollision()
   {
      gameObject.GetComponent <BoxCollider>().enabled  = false;
      gameObject.GetComponent <MeshRenderer>().enabled = false;
      GameObject        deathCube  = (GameObject)Resources.Load("DeathCube");
      List <GameObject> cubes      = new List <GameObject>();
      float             cubeAmount = gameObject.transform.localScale.z / 0.25f;
      cubeAmount = cubeAmount / cubeAmountMod;
      deathCube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0.25f);
      deathCube.GetComponent <MeshRenderer>().material = GetComponent <MeshRenderer>().material;
      float   startZ        = transform.position.z - (transform.localScale.z / 2);
      Vector3 startPosition = new Vector3(transform.position.x, transform.position.y, startZ);
      Vector3 additive      = new Vector3(0, 0, 0);
      for(int i = 0; i < cubeAmount; i++){
          cubes.Add((GameObject)Instantiate(deathCube, startPosition + additive, Quaternion.identity));
          additive += new Vector3(0, 0, 0.25f * cubeAmountMod);
          }
      foreach(GameObject cube in cubes){
          if(rb != null){
             cube.GetComponent <Rigidbody>().AddForce(rb.velocity * 10);
             }
          }
      Destroy(gameObject);
   }
}
