using UnityEngine;
using System.Collections;

public class BlockStats : MonoBehaviour {

    public float Durability = 20f;
    public float Health = 100f;

    public void DamageHealth(float dmg, bool gathering) {
        Health -= dmg;
        if (Health <= 0f) {
            if (gathering) {
                UI.root.playerInventory.MoveItemToInventory(gameObject.GetComponent<ItemPickable>(),
                                                                                        1, false);
                UI.root.playerInventory.DisplayGeneralNotificationBox("1 "
                    + gameObject.GetComponent<ItemBlock>().BlockName + " has been added to your inventory.", 1f);
            } else {
                DestroyBlock();
            }
        }
    }

    public void HealHealth(float heal) {
        Health += heal;
        if (Health > 100f) {
            Health = 100f;
        }
    }

    public void DestroyBlock() {
        Destroy(gameObject);
    }
   
}

