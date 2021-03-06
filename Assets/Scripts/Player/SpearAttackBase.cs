﻿using System.Collections;
using UnityEngine;

namespace Heathside.Control
{
    public class SpearAttackBase : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer swing;
        [SerializeField] private SpriteRenderer spear;
        [SerializeField] private Collider2D hitArea;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] [Tooltip("Set to 0 to inherit")] 
        private float activeTime;
        [SerializeField] [Tooltip("name of the corresponding with this state animation to play")] 
        private string animationName;

        private Animator anim;
        private Material swingMat;
        private AudioSource swingAudio;
        private Coroutine co;

        private int animHash;
        private static readonly int shaderTimeID = Shader.PropertyToID("_FadeTime");

        public bool IsActive { get; private set; }

        public float ActiveTime { get => activeTime; set => activeTime = value; }

        private void Awake()
        {
            swingAudio = transform.parent.GetComponent<AudioSource>();
            anim = this.GetComponentOnRoot<Animator>();
            swingMat = swing.sharedMaterial;
            animHash = Animator.StringToHash(animationName);
            if (!anim.HasState(0, animHash))
            {
                Debug.LogError($"Wrong animation name: {animationName}");
            }
            SetEnabledState(false);
        }

        private void SetEnabledState(bool enabled)
        {
            swing.enabled = enabled;
            spear.enabled = enabled;
            hitArea.enabled = enabled;
            IsActive = enabled;
        }

        public void ProcessAttack()
        {
            co = StartCoroutine(StartAttack(activeTime));
        }

        public void Interrupt()
        {
            StopCoroutine(co);
            StopAttack();
        }

        private IEnumerator StartAttack(float waitTime)
        {
            SetEnabledState(true);
            swingAudio.clip = audioClip;
            swingAudio.Play();
            anim.Play(animHash);
            float initTime = waitTime;

            CombatUtils.ActOnAllOverlapsOneTime<IDamageReceiver>(hitArea, receiver =>
            {
                receiver.TakeDamage(5f);
            });

            while (waitTime > 0)
            {
                yield return null;
                waitTime -= Time.deltaTime;
                float u = waitTime / initTime;
                swingMat.SetFloat(shaderTimeID, u);
            }

            StopAttack();
        }

        private void StopAttack()
        {
            if (anim.GetCurrentAnimatorStateInfo(0).shortNameHash == animHash)
            {
                anim.Play(PlayerAnimationInts.Player_idle);
            }
            SetEnabledState(false);
        }
    }
}