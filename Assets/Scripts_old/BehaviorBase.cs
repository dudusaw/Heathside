using Game.Control;
using UnityEngine;

namespace Game.Base
{
    public class BehaviorBase : MonoBehaviour, IStateBehavior
    {
        [SerializeField] protected MovementAbility movement;
        [SerializeField] protected bool interruptible = true;

        public virtual bool IsActive => false;

        public virtual MovementAbility Movement => movement;

        public virtual bool Interruptible => interruptible;

        public virtual void Interrupt()
        {
        }

        public virtual void StateUpdate()
        {
        }
    }
}