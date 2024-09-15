using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShelfLogic : MonoBehaviour
{
    public int[] itemLayerList;
    public GameObject[] shelfPlaces;
    public GameObject[] backSpaces;

    public GameObject[] itemsPrefabs;
    public static event Action OnValidMatch;
    private int indexForItemsLayerList;
    [SerializeField] private Color colorForBakrowItems;
    private const int spacesOnAShelf = 3;


    private void OnEnable()
    {
        Drag.OnItemMoved += ItemSlot_OnItemMoved;
        LevelHandler.OnFinishLevelLoad += InitiateItemsOnShelfs;
    }
    private void OnDisable()
    {
        Drag.OnItemMoved -= ItemSlot_OnItemMoved;
        LevelHandler.OnFinishLevelLoad -= InitiateItemsOnShelfs;
    }

    private void InitiateItemsOnShelfs()
    {
        int index = indexForItemsLayerList;
        int indexForCurentSlot = 0;
        if (itemLayerList.Length > indexForItemsLayerList)
        {
            while (indexForItemsLayerList < index + spacesOnAShelf) // there spaces on the front of the shelf
            {
                string itemName = itemLayerList[indexForItemsLayerList].ToString();
                for (int i = 0; i < itemsPrefabs.Length; i++)
                {
                    if (itemName == itemsPrefabs[i].name)
                    {
                        Vector3 tmp = new Vector3(); // i have set a grid so as long as it is the child of the borect object it shood be fine
                        Instantiate(itemsPrefabs[i], tmp, Quaternion.identity, shelfPlaces[indexForCurentSlot++].transform);
                    }
                }
                indexForItemsLayerList++;
            }
        }
        if(itemLayerList.Length > indexForItemsLayerList)
        {
            indexForCurentSlot = 0;
            index = indexForItemsLayerList;
            while (index < indexForItemsLayerList + spacesOnAShelf) // there spaces on the back of a shelf
            {
                string itemName = itemLayerList[index].ToString();
                for (int i = 0; i < itemsPrefabs.Length; i++)
                {
                    if (itemName == itemsPrefabs[i].name)
                    {
                        Vector3 tmp = new Vector3(); // i have set a grid so as long as it is the child of the borect object it shood be fine
                        GameObject backRowItem = Instantiate(itemsPrefabs[i], tmp, Quaternion.identity, backSpaces[indexForCurentSlot++].transform);
                        Image image = backRowItem.GetComponent<Image>();
                        image.color = colorForBakrowItems;
                    }
                }
                index++;
            }
        }
    }

    private void ItemSlot_OnItemMoved()
    {
        Transform[] itemSlots = GetComponentsInChildren<Transform>();
        int i = 0;
        string[] items = new string[3] { "0", "0", "0" }; //some values that will never be items prefabs names and they are difernt so that they dont trigger the check condition

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
            else
            {
                items[i] = "0";
                i++;
            }
        }
        if (items[0] != "0" && items[0] == items[1] && items[1] == items[2])
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
            items[0] = "0";
            items[1] = "0";
            items[2] = "0";
            OnValidMatch?.Invoke();
        }

        if (items[0] == "0" && items[0] == items[1] && items[1] == items[2])
        {
            foreach (Transform slot in itemSlots)
            {
                if (!(slot.name.Contains("BackSpace")))
                {
                    continue;
                }
                if (slot.childCount != 0)
                {
                    Transform item = slot.GetChild(0);
                    Drag drag = item.GetComponent<Drag>();
                    drag.DestroySelf();
                }
                
            }
            InitiateItemsOnShelfs();
        }


    }


}
