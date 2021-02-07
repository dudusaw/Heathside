using UnityEngine;

namespace Game.Base
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] private float hp;
        [SerializeField] private float maxHp;
        [SerializeField] private float baseAttack;
        [SerializeField] private float baseCrit;
        [SerializeField] private float baseCritChance;

        private void Start()
        {
            hp = Mathf.Min(hp, maxHp);
        }

        public float healthPoints
        {
            get => hp;
            private set => hp = value;
        }

        public float maxHealthPoints
        {
            get => maxHp;
            private set => maxHp = value;
        }

        public void TakeDamage(float amount)
        {
            hp = Mathf.Max(0, hp - amount);
        }

        public void Heal(float amount)
        {
            hp = Mathf.Min(maxHp, hp + amount);
        }
    }
}