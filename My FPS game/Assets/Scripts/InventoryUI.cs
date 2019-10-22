using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/* This object manages the inventory UI. */

public class InventoryUI : MonoBehaviour {

	public GameObject inventoryUI;	// The entire UI
	public Transform itemsParent;	// The parent object of all the items
    public static bool gameIsPause = false;

    Inventory inventory;	// Our current inventory

	void Start ()
	{
		inventory = Inventory.instance;
		inventory.onItemChangedCallback += UpdateUI;
	}

	// Check to see if we should open/close the inventory
	void Update ()
	{
        Debug.Log(gameIsPause);
        if (Input.GetButtonDown("Inventory") && !PauseMenu.gameIsPause)
		{
            if (gameIsPause)
            {
                gameIsPause = false;
                Time.timeScale = 1;
            }
            else
            {
                gameIsPause = true;
                Time.timeScale = 0;
            }
            inventoryUI.SetActive(!inventoryUI.activeSelf);
			UpdateUI();
		}
	}

	// Update the inventory UI by:
	//		- Adding items
	//		- Clearing empty slots
	// This is called using a delegate on the Inventory.
	public void UpdateUI ()
	{
		InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();

		for (int i = 0; i < slots.Length; i++)
		{
			if (i < inventory.items.Count)
			{
				slots[i].AddItem(inventory.items[i]);
			} else
			{
				slots[i].ClearSlot();
			}
		}
	}

}
