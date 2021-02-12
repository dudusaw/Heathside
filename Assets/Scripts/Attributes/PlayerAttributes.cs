using System;
using System.Collections;
using UnityEngine;

namespace Heathside.Attributes
{
    [CreateAssetMenu(menuName = "Game/Attributes/PlayerAttributes")]
    public class PlayerAttributes : ScriptableObject
    {
        [SerializeField] private HealthAttributes healthAttributes;
        [SerializeField] private DamageAttributes damageAttributes;
        [SerializeField] private MovementAttributes movementAttributes;
        public HealthAttributes HealthAttributes { get => healthAttributes; }
        public DamageAttributes DamageAttributes { get => damageAttributes; }
        public MovementAttributes MovementAttributes { get => movementAttributes; }
    }
}