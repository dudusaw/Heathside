using Game.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public enum EquipableSlot
    {
        Offense,
        Defense,
    }

    [CreateAssetMenu(menuName = "Game/EquipableItem")]
    public class EquipableItem : MaterialItem
    {
        [SerializeField] EquipableSlot slot;
        [SerializeField] IStateBehavior behavior;
        public new const int maxStack = 1;

        public EquipableSlot Slot { get => slot; }
        public IStateBehavior Behavior { get => behavior; }

        public override void Use(InventoryManager manager, GameObject owner)
        {
            Debug.Log("clicked on " + itemName);
            manager.EquipItem(this);
        }
    }
}
