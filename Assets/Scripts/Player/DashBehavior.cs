using Cinemachine;
using Heathside.Base;
using UnityEngine;

namespace Heathside.Control
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

        private float gravity;
        private bool aKey;
        private bool dKey;
        private bool active;

        private float timeSinceLastAKey;
        private float timeSinceLastDKey;
        private float speedTime;

        [SerializeField] private DashData dashData;
        [SerializeField] private PlayerData playerData;

        private void Awake()
        {
            rb = this.GetComponentOnRoot<Rigidbody2D>();
            anim = this.GetComponentOnRoot<Animator>();
            col = this.GetComponentOnRoot<Collider2D>();
            impulseSrc = GetComponent<CinemachineImpulseSource>();
            Utils.CheckComponents(rb, anim, col, dashData, impulseSrc);
            impulseSrc.m_ImpulseDefinition.m_AmplitudeGain = dashData.impulseAmp;
            impulseSrc.m_ImpulseDefinition.m_FrequencyGain = dashData.impulseFreq;
        }

        public override void StateUpdate(System.Action interruptionCallback)
        {
            if (active) return;

            bool activated = false;

            if (Input.GetKeyDown(KeyCode.A))
            {
                direction = MovingDirection.left;
                activated |= KeyDown(ref aKey, ref timeSinceLastAKey);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                direction = MovingDirection.right;
                activated |= KeyDown(ref dKey, ref timeSinceLastDKey);
            }

            if (activated)
            {
                interruptionCallback();
                StartDash();
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
                float u = 1f - (speedTime / dashData.dashTime);
                float velX = Mathf.Lerp(dashData.minDashSpeed, dashData.maxDashSpeed, u) * (int)direction;
                float velY = 0;

                // Trying to resolve collision by pushing upwards when we hit the ground on the way
                if (u >= dashData.castPercent)
                {
                    float vResolvingDistance = col.bounds.extents.y;
                    Vector2 point = new Vector2();
                    point.x = col.bounds.center.x + col.bounds.extents.x * (int)direction;
                    point.y = col.bounds.min.y + vResolvingDistance / 2;
                    Collider2D[] collider2Ds = new Collider2D[2];
                    Vector2 size = new Vector2();
                    size.x = dashData.castDistance;
                    size.y = vResolvingDistance - dashData.colliderEdge;
                    int overlapCount = Physics2D.OverlapBoxNonAlloc(point, size,
                        0, collider2Ds, GameLayers.Ground);
                    if (overlapCount > 0)
                    {
                        point.y = col.bounds.center.y + vResolvingDistance / 2;
                        size.y = col.bounds.size.y - vResolvingDistance - dashData.colliderEdge;
                        overlapCount = Physics2D.OverlapBoxNonAlloc(point, size,
                            0, collider2Ds, GameLayers.Ground);
                        if (overlapCount == 0)
                        {
                            velY = dashData.yResolvingVelocity;
                        }
                    }
                }
                rb.velocity = new Vector2(velX, velY);
            }
        }

        private void Update()
        {
            UpdateTimeIfActive();
            UpdateKeyTime(ref aKey, ref timeSinceLastAKey);
            UpdateKeyTime(ref dKey, ref timeSinceLastDKey);
        }

        private void UpdateTimeIfActive()
        {
            if (active)
            {
                speedTime += Time.deltaTime;
                if (speedTime > dashData.dashTime)
                {
                    DashStop();
                }
            }
        }

        private void DashStop()
        {
            anim.Play(PlayerAnimationInts.Player_dash_end);
            active = false;
            playerData.attributes.RestoreMovement();
            rb.gravityScale = gravity;
        }

        private void UpdateKeyTime(ref bool key, ref float time)
        {
            if (key)
            {
                time += Time.deltaTime;
                if (time > dashData.maxTimeBetweenPresses)
                {
                    key = false;
                }
            }
        }

        private bool KeyDown(ref bool key, ref float time)
        {
            time = 0;
            if (!key)
            {
                key = true;
                return false;
            }
            else
            {
                key = false;
                return true;
            }
        }

        private void StartDash()
        {
            if (active) return;

            gravity = rb.gravityScale;
            rb.gravityScale = 0;
            playerData.attributes.RestrictMovement();
            anim.Play(PlayerAnimationInts.Player_dash_start);
            Transform root = transform.root.transform;
            Vector3 scale = root.localScale;
            scale.x = (int)direction * Mathf.Abs(scale.x);
            root.localScale = scale;
            speedTime = 0;
            active = true;
            impulseSrc.GenerateImpulse(Vector2.left);
            DashExplosionEffect exp = Instantiate(dashData.explosion, transform.position, Quaternion.identity);
            exp.StartExplosion();
        }
    }
}