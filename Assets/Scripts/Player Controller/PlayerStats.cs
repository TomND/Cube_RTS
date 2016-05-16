using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

    public int health = 100;

    public void healHealth(int heal) {
        health += heal;
    }
	
}
