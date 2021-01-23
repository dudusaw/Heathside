using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Items
{
    public class InventoryManager : MonoBehaviour
    {
        List<InventorySlot> slots = new List<InventorySlot>();
        List<EquipSlot> equipSlots = new List<EquipSlot>();

        private void Awake()
        {
            InventorySlot[] allSlots = GetComponentsInChildren<InventorySlot>();
            foreach (var item in allSlots)
            {
                if (item is EquipSlot equipSlot)
                {
                    equipSlots.Add(equipSlot);
                } else
                {
                    slots.Add(item);
                }
            }
        }

        public bool AddItem(Item item, int count)
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
            foreach (var slot in slots)
            {
                if (slot.AddItemIfEmpty(item, count))
                {
                    return true;
                }
            }
            return false;
        }

        public void EquipItem(InventorySlot slot)
        {
        }
    }
}
