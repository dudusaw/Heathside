using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class InventoryManager : MonoBehaviour
    {
        List<InventorySlot> slots = new List<InventorySlot>();
        List<InventorySlot> equipSlots = new List<InventorySlot>();
        [SerializeField] GameObject owner;

        private void Awake()
        {
            InventorySlot[] allSlots = GetComponentsInChildren<InventorySlot>();
            foreach (var item in allSlots)
            {
                if (item.IsEquipSlot)
                {
                    equipSlots.Add(item);
                } else
                {
                    slots.Add(item);
                }
                item.Construct(this, owner);
            }
            int eqSlotsEnumLength = System.Enum.GetValues(typeof(EquipableSlot)).Length;
            if (eqSlotsEnumLength != equipSlots.Count)
            {
                Debug.LogError($"EquipableSlot enum length ({eqSlotsEnumLength}) is not equals equipSlots.Count ({equipSlots.Count})");
            }
        }

        public bool AddItem(Item item, int count)
        {
            if (item.maxStack > 1)
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

        public void EquipItem(EquipableItem item)
        {
            int index = (int)item.Slot;
            var slot = equipSlots[index];
            if (!slot.AddItemIfEmpty(item, 1))
            {
                var oldItem = slot.Item;
                slot.Item = item;
                AddItem(oldItem, 1);
            }
            slot.UpdateIcon();
        }
    }
}
