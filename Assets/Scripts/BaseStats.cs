using UnityEngine;

namespace Game.Base
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] float hp;
        [SerializeField] float maxHp;
        [SerializeField] float baseAttack;
        [SerializeField] float baseCrit;
        [SerializeField] float baseCritChance;

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
