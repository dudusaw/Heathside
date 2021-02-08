using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class InventoryManager : MonoBehaviour
    {
        private List<InventorySlot> slots = new List<InventorySlot>();
        private List<InventorySlot> equipSlots = new List<InventorySlot>();
        [SerializeField] private GameObject owner;

        private void Awake()
        {
            InventorySlot[] allSlots = GetComponentsInChildren<InventorySlot>();
            foreach (var item in allSlots)
            {
                if (item.IsEquipSlot)
                {
                    equipSlots.Add(item);
                }
                else
                {
                    slots.Add(item);
                }
                item.Construct(this, owner);
            }
            int eqSlotsEnumLength = System.Enum.GetValues(typeof(EquipSlot)).Length;
            if (eqSlotsEnumLength != equipSlots.Count)
            {
                Debug.LogError($"EquipableSlot enum length ({eqSlotsEnumLength}) is not equals equipSlots.Count ({equipSlots.Count})");
            }
        }

        public bool AddItemToInventory(Item item, int count)
        {
            if (item.MaxStack > 1)
            {
                foreach (var slot in slots)
                {
                    if (slot.AddItemToStack(item, count, out int countLeft))
                    {
                        if (countLeft > 0)
                        {
                            count = countLeft;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }

            foreach (var slot in slots)
            {
                if (slot.AddItemIfEmpty(item, count))
                {
                    return true;
                }
            }
            return false;
        }

        public void UseItem(InventorySlot slot)
        {
            if (slot.Item is EquipItem eqItem)
            {
                var eqSlot = equipSlots[(int)eqItem.Slot];
                if (slot == eqSlot)
                {
                    slot.Item = null;
                    AddItemToInventory(eqItem, 1);
                }
                else if (eqSlot.AddItemIfEmpty(eqItem, 1))
                {
                    slot.Item = null;
                }
                else
                {
                    var oldItem = eqSlot.Item;
                    eqSlot.Item = eqItem;
                    slot.Item = null;
                    AddItemToInventory(oldItem, 1);
                }
            }
        }
    }
}