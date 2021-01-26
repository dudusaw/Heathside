using Game.Control;
using UnityEngine;

namespace Game.Items
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
        [SerializeField] private IStateBehavior behavior;

        public EquipSlot Slot { get => slot; }
        public IStateBehavior Behavior { get => behavior; }

        private void Awake()
        {
            MaxStack = 1;
        }
    }
}