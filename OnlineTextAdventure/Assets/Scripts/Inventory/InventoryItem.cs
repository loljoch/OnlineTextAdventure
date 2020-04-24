using UnityEngine;

namespace Game.Inventory
{
    [System.Flags]
    public enum Options
    {
        Usable = (1 << 0),
        Attackable = (1 << 1)
    }

    [System.Serializable]
    public class InventoryItem
    {
        public Options options;
        public string name;
        public Sprite sprite;
    }
}
