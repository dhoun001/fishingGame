using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage a collection of Inventories and their respective tabs
/// </summary>
public class InventoryGroup : MonoBehaviour
{
    public PouchList Pouch;

    /// <summary>
    /// Order of tabs, where each tab corresponds to an inventory
    /// Tabs are listed in order
    /// </summary>
    [SerializeField]
    private List<Button> tabOrder = new List<Button>();

    private int currentFocusedIndex = 0;

    private List<Inventory> allInventories = new List<Inventory>();

    private void Awake()
    {
        allInventories = new List<Inventory>(GetComponentsInChildren<Inventory>());
    }

    /// <summary>
    /// Focus on the specified inventory, active that inventory and deactivate all others
    /// </summary>
    public void FocusOnInventory(Inventory focus)
    {
        foreach(Inventory inventory in allInventories)
        {
            if (inventory == focus)
            {
                currentFocusedIndex = allInventories.IndexOf(inventory);
                focus.gameObject.SetActive(true);
            }
            else
                inventory.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Focused on the currently set inventory
    /// </summary>
    public void FocusSetInventory()
    {
        tabOrder[currentFocusedIndex].Select();
        tabOrder[currentFocusedIndex].onClick.Invoke();
    }
}
