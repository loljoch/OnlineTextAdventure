using UnityEngine;

namespace Game.Inventory
{
    [System.Flags]
    public enum TargetableOn
    {
        Allies = (1 << 0),
        Enemies = (1 << 1)
    }

    [System.Serializable]
    public class InventoryItem
    {
        public TargetableOn options;
        public string name;
        public Sprite sprite;
    }
}
