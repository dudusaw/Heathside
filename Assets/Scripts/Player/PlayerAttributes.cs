using System;
using System.Collections;
using UnityEngine;

namespace Heathside.Control
{
    [CreateAssetMenu(menuName = "Game/PlayerAttributes")]
    public class PlayerAttributes : ScriptableObject
    {
        [SerializeField] private float activeSpeed;
        [SerializeField] private float defaultSpeed;
        [SerializeField] private float health;
        [SerializeField] private float healthMax;
        [SerializeField] private float armor;
        [SerializeField] private float baseDamage;
        [SerializeField] private float criticalChance;
        [SerializeField] private float criticalDamage;

        public float ActiveSpeed { get => activeSpeed; }
        public float DefaultSpeed { get => defaultSpeed; }
        public float Health { get => health; }
        public float HealthMax { get => healthMax; }
        public float BaseDamage { get => baseDamage; }
        public float CriticalChance { get => criticalChance; }
        public float CriticalDamage { get => criticalDamage; }
        public float Armor { get => armor; }

        public void RestrictMovement()
        {
            activeSpeed = 0;
        }

        public void RestoreMovement()
        {
            activeSpeed = defaultSpeed;
        }
    }
}