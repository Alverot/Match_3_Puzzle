using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;         //[HideInInspector] hides it in the editor
                                                                // this is a variable for memorating and changing the parent
    public Image Image;
    public static event Action OnItemMoved;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);        // moves the item in the root of the hierarchy
        transform.SetAsLastSibling();               //set it as the last object created so it can be displayed over all others objects
        Image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        transform.SetParent(parentAfterDrag);
        Image.raycastTarget = true;
        OnItemMoved?.Invoke();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
