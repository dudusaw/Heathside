using UnityEngine;

namespace Heathside.Attributes
{
    [CreateAssetMenu(menuName = "Game/Attributes/DamageAttributes")]
    public class DamageAttributes : ScriptableObject
    {
        [SerializeField] private float baseDamage;
        [SerializeField, Range(0, 1)] private float criticalChance;
        [SerializeField] private float criticalMultiplier;

        public float BaseDamage { get => baseDamage; set => baseDamage = value; }
        public float CriticalChance { get => criticalChance; set => criticalChance = value; }
        public float CriticalMultiplier { get => criticalMultiplier; set => criticalMultiplier = value; }
    }
}