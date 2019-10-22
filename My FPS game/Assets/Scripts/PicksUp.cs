using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicksUp : MonoBehaviour
{
    public Item item;	// Item to put in the inventory if picked up

    public void OnMouseDown()
    {
        if(Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position) < 2f)
        {
            bool ok = Inventory.instance.Add(item);   // Add to inventory
            if (ok)
            {
                Destroy(gameObject);    // Destroy item from 
            }
        }
    }

}
