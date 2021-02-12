using UnityEngine;

namespace Heathside.Attributes
{
    [CreateAssetMenu(menuName = "Game/Attributes/HealthAttributes")]
    public class HealthAttributes : ScriptableObject
    {
        [SerializeField] private SingleAttribute health;
        [SerializeField] private SingleAttribute healthMax;
        [SerializeField] private SingleAttribute armor;

        public SingleAttribute Health { get => health; }
        public SingleAttribute HealthMax { get => healthMax; }
        public SingleAttribute Armor { get => armor; }
    }
}