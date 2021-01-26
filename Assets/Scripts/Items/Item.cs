using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{

    public abstract class Item : ScriptableObject
    {
        [SerializeField] public Sprite icon;
        [SerializeField] public string itemName;
        [SerializeField] [TextArea] public string description;
        [SerializeField] [Range(1, 16)] public int maxStack = 1;

        public abstract void Use(InventoryManager manager, GameObject ownerObject);
    }

}
