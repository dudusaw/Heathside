using Heathside.Base;
using System.Collections;
using UnityEngine;

namespace Heathside.Control
{
    public class SpearAttackBehavior : BehaviorBase
    {
        [SerializeField] private SpearAttackBase[] attacks;
        [SerializeField] private float maxTimeBetweenAttacks;
        [SerializeField] private float activeTime;
        [SerializeField] private float minTimeForQueue;
        [SerializeField] private bool queueNextAttackWhenReady;

        private int prevAttack;
        private int nextAttack;
        private float timeSinceLastAttack;
        private bool queueCoStarted;

        private void Awake()
        {
            foreach (var item in attacks)
            {
                if (item.ActiveTime <= 0)
                {
                    item.ActiveTime = activeTime;
                }
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        public override bool IsActive => attacks[prevAttack].IsActive;

        public override void Interrupt()
        {
            attacks[prevAttack].Interrupt();
            StopAllCoroutines();
            queueCoStarted = false;
        }

        public override void StateUpdate(System.Action interruptionCallback)
        {
            if (!queueCoStarted && CInput.GetMouseButtonDownNonUI(0))
            {
                if (!IsActive)
                {
                    interruptionCallback();
                    ProcessAttack();
                }
                else if (queueNextAttackWhenReady)
                {
                    float prevActiveTime = attacks[prevAttack].ActiveTime;
                    if (timeSinceLastAttack > prevActiveTime - minTimeForQueue)
                    {
                        interruptionCallback();
                        StartCoroutine(QueueNextAttack(prevActiveTime - timeSinceLastAttack));
                    }
                }
            }
        }

        private void ProcessAttack()
        {
            if (timeSinceLastAttack > maxTimeBetweenAttacks)
            {
                nextAttack = 0;
            }
            attacks[nextAttack].ProcessAttack();
            prevAttack = nextAttack;
            nextAttack = (nextAttack + 1) % attacks.Length;
            timeSinceLastAttack = 0;
        }

        private IEnumerator QueueNextAttack(float time)
        {
            queueCoStarted = true;
            yield return new WaitForSeconds(time);
            ProcessAttack();
            queueCoStarted = false;
        }
    }
}