using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PouchSlot : InventorySlot
{
    private PouchList pouchBehavior;

    protected new void Awake()
    {
        base.Awake();
        pouchBehavior = GetComponent<PouchList>();
    }

    /// <summary>
    /// Occurs when this slot successfully recieves an item
    /// </summary>
    protected override void OnAcceptItem(InventoryItem item)
    {
        item.GetComponent<IllumBehavior>().pouch = pouchBehavior;
        newItemAdded.Invoke(item);
    }

    /// <summary>
    /// Occurs when an item is removed from this slot
    /// </summary>
    public override void OnRemoveItem(InventoryItem item)
    {
        item.GetComponent<IllumBehavior>().pouch = null;
    }
}
