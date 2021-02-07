using Game.Base;
using System;
using UnityEngine;

namespace Game.Control
{
    public readonly struct AttackInfo
    {
        public readonly Animator anim;
        public readonly Collider2D col;

        public AttackInfo(Animator anim, Collider2D col)
        {
            this.anim = anim;
            this.col = col;
        }
    }

    public interface IAttackState
    {
        void StartAttack();

        void ProcessAttack();

        IAttackState NextState { get; set; }
    }

    public abstract class BaseAttackState : IAttackState
    {
        protected Animator anim;
        protected Collider2D hitArea;

        private Collider2D[] preallocArray;
        private ContactFilter2D filter;
        private int preallocCap = 20;

        public BaseAttackState(Collider2D col)
        {
            Init(col);
        }

        public BaseAttackState(AttackInfo info)
        {
            Utils.CheckComponents(info.col, info.anim);
            anim = info.anim;
            Init(info.col);
        }

        private void Init(Collider2D col)
        {
            hitArea = col;
            preallocArray = new Collider2D[preallocCap];
            filter = new ContactFilter2D();
            filter.SetLayerMask(GameLayers.hittable);
            filter.useTriggers = true;
        }

        public abstract void StartAttack();

        public abstract void ProcessAttack();

        public IAttackState NextState
        {
            get;
            set;
        }

        protected int CheckOverlaps(Collider2D col, out Collider2D[] overlaps)
        {
            Array.Clear(preallocArray, 0, preallocArray.Length);
            overlaps = preallocArray;
            int overlapCount = 0;
            if (col.isActiveAndEnabled)
            {
                overlapCount = col.OverlapCollider(filter, overlaps);
            }
            else
            {
                Debug.LogError($"Collider is not active {col.name}");
            }
            return overlapCount;
        }

        protected int ActOnAllOverlaps<T>(Action<T> action)
        {
            int a = CheckOverlaps(hitArea, out Collider2D[] overlaps);
            foreach (Collider2D touch in overlaps)
            {
                if (touch == null) break;

                T obj = touch.GetComponent<T>();
                if (obj != null)
                {
                    action.Invoke(obj);
                }
            }
            return a;
        }
    }
}