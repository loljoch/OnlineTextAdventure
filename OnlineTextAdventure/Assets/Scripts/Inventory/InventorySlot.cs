using UnityEngine;
using UnityEngine.UI;
using Game.Inventory;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    public Image image;
    public EventTrigger eventTrigger;
    public string toolTip;

    public Object SetItem(InventoryItem inventoryItem)
    {
        image.sprite = inventoryItem.sprite;
        toolTip = inventoryItem.name;

        return gameObject;
    }

    public void ShowToolTip()
    {
        GlobalToolTip.Instance.ActivateToolTip(toolTip, transform);
    }

    public void CloseToolTip()
    {
        GlobalToolTip.Instance.DeactivateToolTip();
    }
}
