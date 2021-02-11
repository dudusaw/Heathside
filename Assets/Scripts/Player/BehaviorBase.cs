using UnityEngine;

namespace Heathside.Control
{
    public abstract class BehaviorBase : MonoBehaviour, IStateBehavior
    {
        [SerializeField] protected bool interruptible = true;

        public virtual bool IsActive => false;

        public virtual bool Interruptible => interruptible;

        public virtual void Interrupt()
        {
        }

        public abstract void StateUpdate(System.Action interruptionCallback);
    }
}