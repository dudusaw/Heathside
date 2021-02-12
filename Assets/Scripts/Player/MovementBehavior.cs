using Heathside.Attributes;
using System.Collections;
using UnityEngine;

namespace Heathside.Control
{
    public enum MovingDirection
    {
        idle = 0,
        left = -1,
        right = 1
    }

    public class MovementBehavior : MonoBehaviour, IMovementController
    {
        private Animator anim;
        private bool jumpCoroStarted;
        private float xInputAxis;
        private PlayerAttributes attributes;
        private PlayerData data;
        private Rigidbody2D rb;
        private Collider2D col;
        private Collider2D[] groundTestAlloc;

        private MovingDirection lastDirection = MovingDirection.right;
        private MovingDirection movingDirection = MovingDirection.idle;

        public bool OnGround { get; private set; }
        public bool CanMove { get; set; }
        public MovingDirection Direction { get => movingDirection; }

        public void Awake()
        {
            this.anim = GetComponent<Animator>();
            this.rb = GetComponent<Rigidbody2D>();
            CanMove = true;
            groundTestAlloc = new Collider2D[1];
            Collider2D[] col2ds = new Collider2D[1];
            int colCount = rb.GetAttachedColliders(col2ds);
            if (colCount != 1)
            {
                Debug.LogError($"wrong collider count {colCount}, should be 1");
            }
            else
            {
                col = col2ds[0];
            }
        }

        public void Construct(PlayerData data)
        {
            this.data = data;
            this.attributes = data.attributes;
        }

        public void MovementFixedUpdate()
        {
            float speed = attributes.MovementAttributes.ActiveSpeed;
            if (speed > 0.0001f && CanMove)
            {
                TestGrounded();
                UpdateMoving(speed);
            }
        }

        /// <param name="obj">MonoBehaviour to start a coroutine from</param>
        public void InputUpdate()
        {
            xInputAxis = Input.GetAxis("Horizontal");
            if (Mathf.Approximately(xInputAxis, 0))
            {
                movingDirection = MovingDirection.idle;
            }
            else
            {
                movingDirection = (MovingDirection)Mathf.Sign(xInputAxis);
            }
            CheckJump();
        }

        private void TestGrounded()
        {
            //RaycastHit2D[] hits = new RaycastHit2D[1];
            //Vector2 boxSize = new Vector2(col.bounds.size.x - 0.01f, 0.1f);
            //int overlaps = Physics2D.BoxCastNonAlloc(col.bounds.center, boxSize,
            //    0, Vector2.down, hits, col.bounds.extents.y, GameLayers.Ground);
            float size = 0.05f;
            float xEdge = 0.01f;
            Vector2 pointA = new Vector2(col.bounds.min.x + xEdge, col.bounds.min.y + size);
            Vector2 pointB = new Vector2(col.bounds.max.x - xEdge, col.bounds.min.y - size);
            int overlaps = Physics2D.OverlapAreaNonAlloc(pointA, pointB, groundTestAlloc, GameLayers.Ground);
            OnGround = overlaps > 0;
        }

        private void UpdateMoving(float speed)
        {
            rb.AddForce(new Vector2(xInputAxis * speed * data.accelerationValue, 0));
            Vector2 vel = rb.velocity;
            vel.x = Mathf.Clamp(vel.x, -speed, speed);
            rb.velocity = vel;
            //rb.velocity = new Vector2(xInputAxis * speed, rb.velocity.y);
        }

        private void CheckJump()
        {
            if (!jumpCoroStarted && Input.GetKeyDown(KeyCode.Space))
            {
                if (OnGround)
                {
                    PerformJump();
                }
                else
                {
                    StartCoroutine(TimeDelayedJump(data.preJumpTime));
                }
            }
        }

        public void ScaleFlipFromDirection(Transform t)
        {
            Vector3 scale = t.localScale;
            switch (movingDirection)
            {
                case MovingDirection.idle:
                    scale.x = (int)lastDirection * Mathf.Abs(scale.x);
                    break;

                case MovingDirection.left:
                    scale.x = -Mathf.Abs(scale.x);
                    lastDirection = movingDirection;
                    break;

                case MovingDirection.right:
                    scale.x = Mathf.Abs(scale.x);
                    lastDirection = movingDirection;
                    break;
            }
            t.localScale = scale;
        }

        private void PerformJump()
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, data.jumpForce), ForceMode2D.Impulse);
            anim.Play(PlayerAnimationInts.Player_jump);
        }

        private IEnumerator TimeDelayedJump(float timeToWait)
        {
            jumpCoroStarted = true;
            while (timeToWait > 0)
            {
                yield return null;
                timeToWait -= Time.deltaTime;
                if (OnGround)
                {
                    PerformJump();
                    break;
                }
            }
            jumpCoroStarted = false;
        }
    }
}