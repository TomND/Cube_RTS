using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Laser : MonoBehaviour
{
   public float              speed; // make private
   public float              DespawnTime;
   private float             spawnTime;
   private GameObject        Spawner;// the object that spawned me
   private Rigidbody         rb;
   public float              impactForce;
   public int                damage;
   public int                team;
   public int                cubeAmountMod;// reduce/increase debris
   public int                particleCount;
   private List <GameObject> deathSubs = new List <GameObject>();


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
         CubePool.AddToLaserPool(gameObject);
         }
   }

   public void SetSpawner(GameObject theSpawner)
   {
      Spawner = theSpawner;
   }

   void OnTriggerEnter(Collider other)
   {
      if(rb != null){                          //TODO: this if statement is temporary, not a real solution
         if(Spawner != null){
            if(other.transform.tag == "Unit"){ //&& other.gameObject != Spawner && other.GetType() == typeof(BoxCollider) && OnDifferentTeam(other.gameObject)){
               if(other.gameObject != Spawner && other.GetType() == typeof(BoxCollider) && OnDifferentTeam(other.gameObject)){
                  GameObject hitUnit = other.gameObject;
                  Cube       hitCube = hitUnit.GetComponent <Cube>();
                  hitCube.TakeDamage(damage, rb.velocity);
                  Rigidbody hitRig = hitUnit.GetComponent <Rigidbody>();
                  hitRig.AddForce(rb.velocity.normalized * impactForce, ForceMode.Impulse);
                  CubePool.AddToLaserPool(gameObject);
                  }
               }
            }
         else if(other.transform.tag == "Laser" && OnDifferentTeam(other.gameObject)){
            //LaserCollision(other.gameObject);
            }
         }
    }


    void OnCollisionEnterasd(Collision other)
    {
       if(rb != null){                          //TODO: this if statement is temporary, not a real solution
          if(Spawner != null){
             if(other.transform.tag == "Unit"){ //&& other.gameObject != Spawner && other.GetType() == typeof(BoxCollider) && OnDifferentTeam(other.gameObject)){
                if(other.gameObject != Spawner && other.GetType() == typeof(BoxCollider) && OnDifferentTeam(other.gameObject)){
                   GameObject hitUnit = other.gameObject;
                   Cube       hitCube = hitUnit.GetComponent <Cube>();
                   hitCube.TakeDamage(damage, rb.velocity);
                   Rigidbody hitRig = hitUnit.GetComponent <Rigidbody>();
                   hitRig.AddForce(rb.velocity.normalized * impactForce, ForceMode.Impulse);
                   CubePool.AddToLaserPool(gameObject);
                   }
                }
             }
          if(other.transform.tag == "Laser" && OnDifferentTeam(other.gameObject)){
             LaserCollision(other.gameObject);
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

   void LaserCollisionOld(GameObject other)
   {
      print("BROKEN");
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
      GetComponent <BoxCollider>().enabled = false;
      CubePool.AddToLaserPool(gameObject);
   }

   void LaserCollision(GameObject other)
   {
      //gameObject.GetComponent <MeshRenderer>().enabled = false;
      Vector3 vel = rb.velocity;

      gameObject.GetComponent <BoxCollider>().enabled = false;
      GetCubes();
      SetupSubs();
      EnableSubs();
      PrepDeathSubCube();
      AddVelocityToSubs(vel);

      CubePool.AddToLaserPool(gameObject);
   }

   void GetCubes()
   {
      deathSubs = new List <GameObject>();
      for(int i = 0; i < particleCount; i++){
          deathSubs.Add(CubePool.RemoveFromPool());
          }
   }

   void PrepDeathSubCube()
   {
      foreach(GameObject sub in deathSubs){
          foreach(MonoBehaviour mono in sub.GetComponents <MonoBehaviour>()){
              mono.enabled = false;
              }

          sub.layer = 9;
          sub.GetComponent <CubeCleanUp>().enabled = true;
          sub.tag = "Untagged";
          sub.GetComponentInChildren <SphereCollider>().enabled = false;
          }
   }

   void SetupSubs()
   {
        foreach (GameObject sub in deathSubs)
        {
          sub.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0.25f);
          sub.GetComponent <MeshRenderer>().material = GetComponent <MeshRenderer>().material;
          }
      float   startZ        = transform.position.z - (transform.localScale.z / 2);
      Vector3 startPosition = new Vector3(transform.position.x, transform.position.y, startZ);
      Vector3 additive      = new Vector3(0, 0, 0);
      for(int i = 0; i < deathSubs.Count; i++){
          deathSubs[i].transform.position = startPosition + additive;
          deathSubs[i].transform.rotation = Quaternion.identity;
          additive += new Vector3(0, 0, 0.25f * cubeAmountMod);
          }
   }

   void AddVelocityToSubs(Vector3 vel)
   {
        foreach (GameObject sub in deathSubs)
        {
            sub.GetComponent <Rigidbody>().AddForce(vel * Random.Range(3, 11));
          }
   }

   void EnableSubs()
   {
      foreach(GameObject sub in deathSubs){
          sub.SetActive(true);
          }
   }
}
