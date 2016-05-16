using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UI : MonoBehaviour{

    public static UIRoot root;
    //public static CraftingDatabase craftDB;
	void Awake () {
        root = GetComponent<UIRoot>();
        //craftDB = GetComponent<CraftingDatabase>();
	}
    
}
