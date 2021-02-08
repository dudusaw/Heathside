﻿using Cinemachine;
using Game.Base;
using UnityEngine;

namespace Game.Control
{
    /// <summary>
    /// Must be executed after controller movement
    /// </summary>
    public class DashBehavior : BehaviorBase
    {
        private Animator anim;
        private Rigidbody2D rb;
        private Collider2D col;
        private CinemachineImpulseSource impulseSrc;

        private MovingDirection direction = MovingDirection.idle;

        private bool aKey;
        private bool dKey;
        private bool active;

        private float initialGrav;
        private float timeSinceLastAKey;
        private float timeSinceLastDKey;
        private float speedTime;

        [SerializeField] private DashData data;

        private void Awake()
        {
            rb = this.GetComponentOnRoot<Rigidbody2D>();
            anim = this.GetComponentOnRoot<Animator>();
            col = this.GetComponentOnRoot<Collider2D>();
            impulseSrc = GetComponent<CinemachineImpulseSource>();
            Utils.CheckComponents(rb, anim, col, data, impulseSrc);
            impulseSrc.m_ImpulseDefinition.m_AmplitudeGain = data.impulseAmp;
            impulseSrc.m_ImpulseDefinition.m_FrequencyGain = data.impulseFreq;
        }

        public override void StateUpdate()
        {
            if (active) return;

            if (Input.GetKeyDown(KeyCode.A))
            {
                direction = MovingDirection.left;
                KeyDown(ref aKey, ref timeSinceLastAKey);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                direction = MovingDirection.right;
                KeyDown(ref dKey, ref timeSinceLastDKey);
            }
        }

        public override bool IsActive => active;

        public override void Interrupt()
        {
            DashStop();
        }

        private void FixedUpdate()
        {
            if (active)
            {
                float u = 1f - (speedTime / data.dashTime);
                float velX = Mathf.Lerp(data.minDashSpeed, data.maxDashSpeed, u) * (int)direction;
                float velY = 0;

                // Trying to resolve collision by pushing upwards when we hit the ground on the way
                if (u >= data.castPercent)
                {
                    float vResolvingDistance = col.bounds.extents.y;
                    Vector2 point = new Vector2();
                    point.x = col.bounds.center.x + col.bounds.extents.x * (int)direction;
                    point.y = col.bounds.min.y + vResolvingDistance / 2;
                    Collider2D[] collider2Ds = new Collider2D[2];
                    Vector2 size = new Vector2();
                    size.x = data.castDistance;
                    size.y = vResolvingDistance - data.colliderEdge;
                    int overlapCount = Physics2D.OverlapBoxNonAlloc(point, size,
                        0, collider2Ds, GameLayers.Ground);
                    if (overlapCount > 0)
                    {
                        point.y = col.bounds.center.y + vResolvingDistance / 2;
                        size.y = col.bounds.size.y - vResolvingDistance - data.colliderEdge;
                        overlapCount = Physics2D.OverlapBoxNonAlloc(point, size,
                            0, collider2Ds, GameLayers.Ground);
                        if (overlapCount == 0)
                        {
                            velY = data.yResolvingVelocity;
                        }
                    }
                }
                rb.velocity = new Vector2(velX, velY);
            }
        }

        private void Update()
        {
            UpdateTimeIfCasting();
            UpdateKeyTime(ref aKey, ref timeSinceLastAKey);
            UpdateKeyTime(ref dKey, ref timeSinceLastDKey);
        }

        private void UpdateTimeIfCasting()
        {
            if (active)
            {
                speedTime += Time.deltaTime;
                if (speedTime > data.dashTime)
                {
                    DashStop();
                }
            }
        }

        private void DashStop()
        {
            anim.Play(PlayerAnimationInts.Player_dash_end);
            active = false;
        }

        private void UpdateKeyTime(ref bool key, ref float time)
        {
            if (key)
            {
                time += Time.deltaTime;
                if (time > data.maxTimeBetweenPresses)
                {
                    key = false;
                }
            }
        }

        private void KeyDown(ref bool key, ref float time)
        {
            time = 0;
            if (!key)
            {
                key = true;
            }
            else
            {
                key = false;
                StartDash();
            }
        }

        private void StartDash()
        {
            if (active) return;

            anim.Play(PlayerAnimationInts.Player_dash_start);
            Transform root = transform.root.transform;
            Vector3 scale = root.localScale;
            scale.x = (int)direction * Mathf.Abs(scale.x);
            root.localScale = scale;
            speedTime = 0;
            active = true;
            impulseSrc.GenerateImpulse(Vector2.left);
            DashExplosionEffect exp = Instantiate(data.explosion, transform.position, Quaternion.identity);
            exp.StartExplosion();
        }
    }
}