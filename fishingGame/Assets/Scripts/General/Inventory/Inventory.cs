using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// Number of items being managed by this inventory
    /// </summary>
    public int numberOfItems
    {
        get
        {
            int items = 0;
            foreach(InventorySlot slot in listOfSlots)
            {
                items += slot.allItems.Count;
            }
            return items;
        }
    }

    /// <summary>
    /// Slots in this inventory
    /// </summary>
    public List<InventorySlot> listOfSlots { get; private set; }


    /// <summary>
    /// Events fired off when adding a new item to this slot
    /// </summary>
    public Action<InventoryItem> addNewItemEvent = delegate { };

    [SerializeField]
    private List<Inventory> mirroredInventories = new List<Inventory>();

    private void Awake()
    {
        listOfSlots = new List<InventorySlot>(GetComponentsInChildren<InventorySlot>());
        foreach(InventorySlot i in listOfSlots)
        {
            i.Init(GetComponent<MultiTags>());
        }
    }

    /// <summary>
    /// Add item to the next avaliable open slot
    /// </summary>
    public void AddNewItem(InventoryItem item)
    {
        if (listOfSlots.Count == 0)
        {
            Debug.LogWarning("Can't add " + item.name + ", there are no slots in this inventory.");
            return;
        }

        foreach (InventorySlot slot in listOfSlots)
        {
            if (slot.isValidItem(item))
            {
                slot.AddItem(item);
                AddToMirroredInventory(item);
                return;
            }
        }


        Debug.LogWarning("Can't add " + item.name + ", all slots in this inventory are full or there are no more valid slots.");
    }

    /// <summary>
    /// Remove an item in this inventory, if it exists
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(InventoryItem item, bool destroy = false)
    {
        foreach(InventorySlot slot in listOfSlots)
        {
            slot.RemoveItem(item, destroy);
        }
    }

    /// <summary>
    /// Add item to all mirrored inventories
    /// </summary>
    /// <param name="item"></param>
    public void AddToMirroredInventory(InventoryItem item)
    {
        //Add to all mirrored inventories, if it does not already contain it
        foreach (Inventory inventory in mirroredInventories)
        {
            if (!inventory.GetMirroredItem(item))
            {
                InventoryItem new_item = Instantiate(item);
                new_item.name = item.name;
                inventory.AddNewItem(new_item);
            }
        }
    }

    /// <summary>
    /// Remove item from all mirrored inventories
    /// </summary>
    /// <param name="item"></param>
    public void RemoveFromMirroredInventory(InventoryItem item)
    {
        //Remove from all mirrored inventories
        foreach (Inventory inventory in mirroredInventories)
        {
            InventoryItem mirrored_item = inventory.GetMirroredItem(item);
            if (mirrored_item != null)
                inventory.RemoveItem(mirrored_item, true);
        }
    }

    /// <summary>
    /// Look for an item with a matching name
    /// </summary>
    public InventoryItem GetMirroredItem(InventoryItem item)
    {
        foreach(InventorySlot slot in listOfSlots)
        {
            InventoryItem mirrored_item = slot.GetMirroredItem(item);
            if (mirrored_item != null)
                return mirrored_item;
        }
        return null;
    }

    /// <summary>
    /// Returns true if this inventory contains the specified item
    /// </summary>
    public bool ContainsItem(InventoryItem item)
    {
        foreach(InventorySlot slot in listOfSlots)
        {
            if (slot.ContainsItem(item))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Transfer contents of argument to this inventory
    /// </summary>
    public void TransferInventory(Inventory inventory)
    {
        for(int i = inventory.listOfSlots.Count - 1; i >= 0; i--)
        {
            InventorySlot slot = inventory.listOfSlots[i];
            for (int j = slot.allItems.Count - 1; j >= 0; j--)
            {
                InventoryItem item = slot.allItems[j];
                slot.RemoveItem(item);
                AddNewItem(item);
            }
        }
    }

    /// <summary>
    /// TODO: Remove all items from this inventory
    /// </summary>
    public void RemoveAllItems()
    {

    }

    /// <summary>
    /// Given an item, find the closest valid slot that the item can go into
    /// </summary>
    public static InventorySlot GetClosestValidSlot(InventoryItem item)
    {
        InventorySlot[] slots = FindObjectsOfType<InventorySlot>();
        if (slots.Length == 0)
            return null;
        
        int closest_slot = -1;
        float[] slot_distances = new float[slots.Length];

        for (int i = 0; i < slots.Length; ++i)
        {
            float current_slot_distance = Vector2.Distance(slots[i].transform.position, item.transform.position);

            //Check if slot is valid for item and that it is the closest slot found so far
            if (slots[i].isValidItem(item) && 
                (closest_slot == -1 || 
                current_slot_distance < slot_distances[closest_slot]))
            {
                closest_slot = i;
            }
            slot_distances[i] = current_slot_distance;
        }

        //If there is no closest valid slot, return null
        if (closest_slot == -1)
            return null;

        return slots[closest_slot];
    }
}
