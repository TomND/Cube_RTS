using UnityEngine;
using System.Collections;

public class NPCInventory : MonoBehaviour {

	public ItemPickable[] inventory;

	void Update() {
		// WHAT TO CHECK PERIODICALLY TO DO THE FOLLOWING METHODS
	}

	public void AddItem(ItemPickable item) { // ADD ITEM TO INVENTORY
		print("called");
		if (item.GetComponent<ItemPickable>() == null) {
			print ("The item trying to be added does not contain ItemPickable script.");
		}else if (item.GetComponent<ItemPickable>() != null) {
			for (int i = 0; i < inventory.Length; i++) {
				if (inventory[i] == null) {
					inventory[i] = item;
					item.gameObject.SetActive (false);
					break;
					print("picked up");
				}
			}
		}
	}

	public void DropItem(ItemPickable item) { // DROPS ITEM FROM INVENTORY PHYSICALLY SHOWING
		for (int i = 0; i < inventory.Length; i++) {
			if (item == inventory[i]) {
				item.transform.position = gameObject.transform.position;
				item.gameObject.SetActive (true);
				break;
			}
		}
	}

    public bool SearchForExists(string tag)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
                break;
            if (inventory[i].tag == tag)
            {
                return true;
            }
        }
        return false;
    }

    public GameObject ReturnItem(string tag)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].tag == tag)
            {
                return inventory[i].gameObject;
            }
        }
        return null;
    }

	public void RemoveItem(ItemPickable item) { // REMOVE ITEM FROM INVENTORY
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] == item) {
				inventory[i] = null;
				Destroy (item.gameObject);
				break;
			}
		}
	}

    public void RemoveItemByName(string item, int amount)
    { // REMOVE ITEM FROM INVENTORY
        for (int r = 0; r < amount; r++)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == null)
                    break;

                if (inventory[i].tag == item)
                {
                    inventory[i] = null;
                    //Destroy(inventory[i].gameObject);
                    i = inventory.Length;
                    break;
                }
            }
        }
    }

    public bool InventoryFull()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                return false;
            }
        }


        return true;
    }

    public int AmountOfItem(string item)
    {
        int amount = 0;
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
                break; 

            if (inventory[i].tag == item)
            {
                amount += 1;
            }
        }
        return amount;

    }

	public void RemoveAllItems() { // REMOVE ALL ITEMS FROM INVENTORY
		for (int i = 0; i < inventory.Length; i++) {
				inventory[i] = null;
				Destroy (inventory[i].gameObject);
		}
	}

	public void UseItem(ItemPickable item) { // USE ITEM FROM INVENTORY

		bool exists = false;
		int index = -1;
		for (int i = 0; i < inventory.Length; i++) {
			if (item == inventory[i]) {
				exists = true;
				index = i;
				break;
			}
		}

		if (exists) {// CHECKING IF ITEM EXISTS IN INVENTORY
			if (item.GetComponent<ItemPickable>().itemType == ItemType.Food) {

				// NPC WHEN EATING FOOD

			}else if (item.GetComponent<ItemPickable>().itemType == ItemType.Drink) {

				// NPC WHEN DRINKING

			}else if (item.GetComponent<ItemPickable>().itemType == ItemType.Medical) {

				// NPC WHEN USING MEDICAL ITEM

			}else if (item.GetComponent<ItemPickable>().itemType == ItemType.Ammunition) {

				// NPC AMMUNITION AMOUNT (IF YOU WANT TO SYNC THAT IN WITH PROJECTILEWEAPON)

			}else if (item.GetComponent<ItemPickable>().itemType == ItemType.Apparel) {
				
				// NPC'S WEARING CLOTHES - APPAREL WEARING NOT IMPLEMENTED YET
				
			}else if (item.GetComponent<ItemPickable>().itemType == ItemType.MeleeWeapon) {
				
				// NPC BEING ABLE TO HOLD GUN
				
			}else if (item.GetComponent<ItemPickable>().itemType == ItemType.ProjectileWeapon) {
				
				// NPC BEING ABLE TO HOLD WEAPON

			}
		}

  	}

}

