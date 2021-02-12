using Heathside.Control;
using UnityEngine;

namespace Heathside.Items
{
    public enum EquipSlot
    {
        Offense,
        Defense,
    }

    [CreateAssetMenu(menuName = "Game/EquipableItem")]
    public class EquipItem : MaterialItem
    {
        [SerializeField] private EquipSlot slot;
        [SerializeField] private IBehaviorState behavior;

        public EquipSlot Slot { get => slot; }
        public IBehaviorState Behavior { get => behavior; }

        private void Awake()
        {
            MaxStack = 1;
        }
    }
}