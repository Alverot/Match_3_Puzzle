using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class ShelfLogic : MonoBehaviour
{
    private void OnEnable()
    {
        Drag.OnItemMoved += ItemSlot_OnItemMoved;
    }
    private void OnDisable()
    {
        Drag.OnItemMoved -= ItemSlot_OnItemMoved;
    }

    private void ItemSlot_OnItemMoved()
    {
        Transform[] itemSlots = GetComponentsInChildren<Transform>();
        int i = 0;
        string[] items = new string[3] { "-1", "0", "0" };

        foreach (Transform slot in itemSlots)
        {
            
            if (!(slot.name.Contains("ShelfPlace1"))) 
            {
                continue; 
            }
            
            if (slot.childCount != 0)
            {
                Transform item = slot.GetChild(0);
                items[i] = item.gameObject.name;
                //Debug.Log(item.gameObject.name);
                i++;
            }
        }
        if (items[0] == items[1] && items[1] == items[2])
        {
            Debug.Log("Merged" + items[0] + items[1] + items[2]);
            foreach (Transform slot in itemSlots)
            {
                if (!(slot.name.Contains("ShelfPlace1")))
                {
                    continue;
                }
                Transform item = slot.GetChild(0);
                Drag drag = item.GetComponent<Drag>();
                drag.DestroySelf();
            }
        }
        
    }


}
