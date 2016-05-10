using UnityEngine;
using System.Collections;

public class MapBounds : MonoBehaviour {
   // Use this for initialization
   void Start()
   {
   }

   // Update is called once per frame
   void Update()
   {
   }

   void OnTriggerExit(Collider other)
    {
		 if(other.GetType() == typeof(BoxCollider)){//TODO: make add to pool instead of destroy
			 //CubePool.AddToPool(other.gameObject);
			 Destroy(other.gameObject);
		 }
   }
}
