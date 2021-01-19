using Game.Animation;
using Game.Base;
using System.Collections;
using UnityEngine;

namespace Game.Control
{
    public class SwordAttackBehavior : MonoBehaviour, IStateBehavior
    {
        [SerializeField]
        private Collider2D hitArea;
        [SerializeField]
        private Collider2D hitAreaExtended;
        [SerializeField]
        private GameObject slash;

        private Animator anim;

        private IAttackState defaultAttack;
        private IAttackState activeAttack;

        private bool coroutineStarted;

        [SerializeField, Tooltip("Maximum gap beetwen 2 combo attack before reset")]
        private float maxDelayBetweenAttacks = 1f;
        [SerializeField, Tooltip("Minimum time before being able to attack again")]
        private float minDelayBetweenAttacks = 0.5f;

        private float timeSinceLastAttack = Mathf.Infinity;

        public bool IsActive => anim.GetCurrentAnimatorStateInfo(0).tagHash == AnimatorArgs.attackTag;

        public bool Interruptible => true;

        public MovementAbility Movement => new MovementAbility(false);

        public void Construct(Animator animator)
        {
            this.anim = animator;
            var ev = animator.GetComponent<AnimationEventsDelegate>();
            if (ev)
            {
                ev.hitEvent += HitEvent;
            }
            else
            {
                Debug.LogWarning($"No object of type AnimationEventsDelegate on animator {animator.name}");
            }

            SetupStates();
        }

        private void SetupStates()
        {
            AttackInfo info = new AttackInfo(anim, hitArea);
            AttackInfo infoExt = new AttackInfo(anim, hitAreaExtended);

            IAttackState state1 = new Attack_1(info);
            IAttackState state2 = new Attack_2(info);
            IAttackState state3 = new Attack_3(infoExt, slash);

            state1.NextState = state2;
            state2.NextState = state3;
            state3.NextState = state1;

            defaultAttack = state1;
            activeAttack = state1;
        }

        public void HitEvent()
        {
            activeAttack.ProcessAttack();
            activeAttack = activeAttack.NextState;
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        public void StateUpdate()
        {
            if (!Input.GetMouseButtonDown(0) || coroutineStarted)
            {
                return;
            }
            if (timeSinceLastAttack < minDelayBetweenAttacks)
            {
                StartCoroutine(AttackWithDelay(minDelayBetweenAttacks - timeSinceLastAttack));
                return;
            }
            StartAttack();
        }

        private void StartAttack()
        {
            CheckForLastAttack();
            activeAttack.StartAttack();
        }

        private IEnumerator AttackWithDelay(float delay)
        {
            coroutineStarted = true;
            yield return new WaitForSeconds(delay);
            StartAttack();
            coroutineStarted = false;
        }

        private void CheckForLastAttack()
        {
            if (timeSinceLastAttack > maxDelayBetweenAttacks)
            {
                activeAttack = defaultAttack;
            }
            timeSinceLastAttack = 0;
        }

        public void Interrupt()
        {
            throw new System.NotImplementedException();
        }
    }
    abstract class SwordAttackState : BaseAttackState
    {
        protected float attackDamage;

        public SwordAttackState(AttackInfo info) : base(info)
        {
        }

        public override void ProcessAttack()
        {
            int a = ActOnAllOverlaps<BaseStats>(touch =>
            {
                touch.TakeDamage(attackDamage);
            });
        }
    }

    class Attack_1 : SwordAttackState
    {
        public Attack_1(AttackInfo info) : base(info)
        {
            attackDamage = 1f;
        }

        public override void StartAttack() => anim.Play(AnimatorArgs.Player_sword_attack1);
    }

    class Attack_2 : SwordAttackState
    {
        public Attack_2(AttackInfo info) : base(info)
        {
            attackDamage = 2f;
        }

        public override void StartAttack() => anim.Play(AnimatorArgs.Player_sword_attack2);
    }

    class Attack_3 : SwordAttackState
    {
        private Animator slashAnimator;
        private SpriteRenderer slashRend;

        public Attack_3(AttackInfo info, GameObject slash) : base(info)
        {
            attackDamage = 3f;
            slashRend = slash.GetComponent<SpriteRenderer>();
            slashRend.enabled = false;
            slashAnimator = slash.GetComponent<Animator>();
            SlashEvents animExitEvent = slashAnimator.GetBehaviour<SlashEvents>();
            animExitEvent.exitEvent += AnimStop;
        }

        private void AnimStop()
        {
            slashRend.enabled = false;
        }

        public override void ProcessAttack()
        {
            base.ProcessAttack();
            slashRend.enabled = true;
            slashAnimator.SetTrigger(AnimatorArgs.activateSlash);
        }

        public override void StartAttack() => anim.Play(AnimatorArgs.Player_sword_attack3);
    }
}
