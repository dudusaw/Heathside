using Game.Base;
using Game.Saving;
using System.Collections;
using UnityEngine;

namespace Game.Control
{

    public class SpearAttackBehavior : BehaviorBase
    {
        [SerializeField] SpearAttackBase[] attacks;
        [SerializeField] float maxTimeBetweenAttacks = 0.3f;
        [SerializeField] float activeTime = 0.35f;
        [SerializeField] float minTimeForQueue = 0.3f;
        [SerializeField] bool queueNextAttackWhenReady;

        int prevAttack;
        int nextAttack;
        float timeSinceLastAttack;
        bool queueCoStarted;

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
        }

        public override void StateUpdate()
        {
            if (!queueCoStarted && Input.GetMouseButtonDown(0))
            {
                if (!IsActive)
                {
                    ProcessAttack();
                }
                else
                {
                    float prevActiveTime = attacks[prevAttack].ActiveTime;
                    if (timeSinceLastAttack > prevActiveTime - minTimeForQueue)
                    {
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
