using UnityEngine;

namespace Heathside.Items
{
    public enum ItemRarity
    {
        Common,
        Rare,
        Legendary
    }

    public abstract class Item : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string itemName;
        [SerializeField] private ItemRarity rarity;
        [SerializeField] [TextArea] private string description;
        [SerializeField] [Range(1, 16)] private int maxStack = 1;

        public Sprite Icon { get => icon; protected set => icon = value; }
        public string ItemName { get => itemName; protected set => itemName = value; }
        public ItemRarity Rarity { get => rarity; protected set => rarity = value; }
        public string Description { get => description; protected set => description = value; }
        public int MaxStack { get => maxStack; protected set => maxStack = value; }

        public abstract void Use(GameObject ownerObject);
    }
}