using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public List<InventoryItem> items;
        public InventorySlot prefab;

        public List<InventoryItem> GetAllItems(Options options)
        {
            List<InventoryItem> gItems = new List<InventoryItem>();

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].options == options)
                {
                    gItems.Add(items[i]);
                }
            }

            return gItems;
        }

        [EasyAttributes.Button]
        public void VisualizeItems()
        {
            for (int i = 0; i < items.Count; i++)
            {
                Instantiate(prefab.SetItem(items[i]), transform);
            }
        }
    }
}
