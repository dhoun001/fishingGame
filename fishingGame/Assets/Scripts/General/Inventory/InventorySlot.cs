using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    /// <summary>
    /// How many items can fit in this slot?
    /// </summary>
    [SerializeField]
    private int _maxItemsHeld = 1;
    public int maxItemsHeld { get { return _maxItemsHeld; } private set { _maxItemsHeld = value; } }

    /// <summary>
    /// Allowed items in this slot
    /// </summary>
    public MultiTags currentFilter { get; private set; }

    /// <summary>
    /// All items currently being handled by this slot
    /// </summary>
    public List<InventoryItem> allItems = new List<InventoryItem>();

    /// <summary>
    /// Inventory that this Inventory Slot resides in
    /// This Assums that the Slot is a child of its Inventory
    /// </summary>
    public Inventory parentInventory { get; private set; }

    public Action<InventoryItem> newItemAdded = delegate { };

    public bool slotFull { get { return totalItems >= maxItemsHeld; } }

    public int totalItems
    {
        get { return allItems.Count; }
    }

    protected void Awake()
    {
        RefreshInventoryItems();
        parentInventory = GetComponentInParent<Inventory>();
    }

    public void Init(MultiTags filter)
    {
        currentFilter = filter;
    }

    /// <summary>
    /// Reinit inventory items
    /// </summary>
    private void RefreshInventoryItems()
    {
        allItems = new List<InventoryItem>(GetComponentsInChildren<InventoryItem>());
    }

    /// <summary>
    /// Return true if this slot contains the specified item
    /// </summary>
    public bool ContainsItem(InventoryItem item)
    {
        RefreshInventoryItems();
        return allItems.Contains(item);
    }

    /// <summary>
    /// Look for an item with a matching name, but not the same reference
    /// </summary>
    public InventoryItem GetMirroredItem(InventoryItem item)
    {
        RefreshInventoryItems();
        foreach (InventoryItem slotted_item in allItems)
        {
            if (item.IsSameItemName(slotted_item) && item != slotted_item)
                return slotted_item;
        }
        return null;
    }

    /// <summary>
    /// Removes an item if they both share names, or are the same reference
    /// </summary>
    public bool RemoveIdenticleItem(InventoryItem item)
    {
        RefreshInventoryItems();
        foreach (InventoryItem slotted_item in allItems)
        {
            if (item.IsSameItemName(slotted_item) || slotted_item == item)
            {
                allItems.Remove(item);
                allItems.Remove(slotted_item);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Add a new item into this slot, assuming it can fit, by parenting the item to this slot.
    /// </summary>
    public void AddItem(InventoryItem item)
    {
        RefreshInventoryItems();
        if (!isValidItem(item))
        {
            OnRejectItem(item);
            return;
        }

        item.transform.SetParent(transform, false);
        allItems.Add(item);
        OnAcceptItem(item);


        //TODO: ew
        Inventory parent_inventory = GetComponentInParent<Inventory>();
        if (parent_inventory != null)
            parent_inventory.AddToMirroredInventory(item);

    }

    /// <summary>
    /// Remove item from inventory.
    /// </summary>
    public void RemoveItem(InventoryItem item, bool destroy = false)
    {
        if (!allItems.Contains(item))
            return;

        allItems.Remove(item);

        if (destroy)
        {
            Destroy(item.gameObject);
        }
        else
            item.transform.SetParent(null, false);


        //TODO: ew
        Inventory parent_inventory = GetComponentInParent<Inventory>();
        if (parent_inventory)
            parent_inventory.RemoveFromMirroredInventory(item);

        OnRemoveItem(item);
    }

    /// <summary>
    /// TODO: Remove all items from this slot
    /// </summary>
    public void RemoveAllItems()
    {

    }

    /// <summary>
    /// Returns if item can fit into this slot
    /// </summary>
    public bool isValidItem(InventoryItem item)
    {
        if (item == null)
            return false;

        RefreshInventoryItems();

        if (slotFull)
            return false;

        if (currentFilter == null)
            return true;

        if (currentFilter.localTagList.Count == 1 && item.tag == "Untagged")
            return true;

        foreach (MT multi_tag in currentFilter.localTagList)
        {
            if (item.gameObject.HasTag(multi_tag.Name) && multi_tag.Name != "Untagged")
                return true;
        }
        return false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging)
            return;

        InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (isValidItem(item))
            item.tempInventorySlot = this;  
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging)
            return;

        InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (item == null)
            return;

        item.tempInventorySlot = null; 
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }



    /// <summary>
    /// Occurs when this slot successfully recieves an item
    /// </summary>
    protected virtual void OnAcceptItem(InventoryItem item)
    {
        newItemAdded.Invoke(item);
    }

    /// <summary>
    /// Occurs when an item is removed from this slot
    /// </summary>
    public virtual void OnRemoveItem(InventoryItem item)
    {

    }

    /// <summary>
    /// Occurs when this slot rejects an item
    /// </summary>
    protected virtual void OnRejectItem(InventoryItem item)
    {

    }
}
