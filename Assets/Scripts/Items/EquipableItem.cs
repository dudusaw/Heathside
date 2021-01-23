using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public enum EquipableSlot
    {
        Offense,
        Defense,
        Custom
    }

    [CreateAssetMenu(menuName = "Game/EquipableItem")]
    public class EquipableItem : MaterialItem
    {
        public EquipableSlot slot;
    }
}
