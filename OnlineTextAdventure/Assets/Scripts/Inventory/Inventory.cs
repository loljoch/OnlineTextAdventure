using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public List<InventoryItem> items;
        public InventorySlot prefab;

        public List<string> GetAllItems()
        {
            List<string> gItems = new List<string>();

            for (int i = 0; i < items.Count; i++)
            {
                gItems.Add(items[i].name);
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
