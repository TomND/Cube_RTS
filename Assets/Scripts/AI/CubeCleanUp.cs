using UnityEngine;
using System.Collections;

public class CubeCleanUp : MonoBehaviour {
   private float startTime;
   private float cleanTime;

   // Use this for initialization
   void Start()
    {
        cleanTime = Random.Range(4, 7);
        startTime = Time.time;
   }

   // Update is called once per frame
   void Update()
    {
		 CleanUp();
    }

   void CleanUp()
   {
      if(Time.time > startTime + cleanTime){
         Destroy(gameObject);
         }
   }
}
