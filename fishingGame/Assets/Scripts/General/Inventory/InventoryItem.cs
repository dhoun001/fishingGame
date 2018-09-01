using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class InventoryItem: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public InventorySlot currentInventorySlot { get; private set; }

    /// <summary>
    /// Change to set inventory item to new slot on EndDrag
    /// Otherwise, stay in the current slot
    /// </summary>
    [HideInInspector]
    public InventorySlot tempInventorySlot = null;

    /// <summary>
    /// Reference to the actual data type housing its actual ingame behavior
    /// </summary>
    public ItemBehavior itemSpecificBehavior { get { return GetComponent<ItemBehavior>(); } }

    public bool isInteractable { get { return button.interactable; } }

    /// <summary>
    /// If true, items that are dropped will move to the closest valid slot
    /// </summary>
    [SerializeField]
    private bool moveToClosestSlot = true;

    [Space(10)]

    private Button button;
    private Image imageSprite;
    private LayoutElement layoutElement;

    private void Awake()
    {
        button = GetComponent<Button>();
        imageSprite = GetComponent<Image>();
        layoutElement = GetComponent<LayoutElement>();
    }

    private void Start()
    {
        currentInventorySlot = GetComponentInParent<InventorySlot>();
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        currentInventorySlot.RemoveItem(this);

        //Set parent to ensure item will not be hidden from any inventory slots
        transform.SetParent(ItemManager.Instance.transform, false);

        layoutElement.ignoreLayout = true;
        imageSprite.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        imageSprite.raycastTarget = true;
        layoutElement.ignoreLayout = false;

        //Find the new inventory slot for this item
        InventorySlot new_item_slot = null;
        if (tempInventorySlot != null)
        {
            new_item_slot = tempInventorySlot;
        }
        else
        {
            if (moveToClosestSlot)
                new_item_slot = Inventory.GetClosestValidSlot(this);

            if (new_item_slot == null)
                new_item_slot = currentInventorySlot;
        }

        currentInventorySlot.RemoveItem(this);
        new_item_slot.AddItem(this);
        currentInventorySlot = new_item_slot;

        //If exchanging item to a completely new Inventory, fire off new item event
        if (currentInventorySlot.parentInventory != null)
            currentInventorySlot.parentInventory.addNewItemEvent.Invoke(this);

        tempInventorySlot = null;
    }

    /// <summary>
    /// Run click events, if any
    /// </summary>
    public virtual void OnClick()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public bool IsSameItemName(InventoryItem item)
    {
        return item.name == name;
    }
}
