using Game.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Control
{
    public class MovementBehavior : MonoBehaviour
    {
        [SerializeField] PlayerData data;

        float inputX;

        Rigidbody2D rb;
        Collider2D col;

        MovingDirection lastDirection = MovingDirection.right;
        MovingDirection movingDirection = MovingDirection.idle;

        public bool CanMove { get; set; }
        public bool OnGround { get; private set; }
        public MovingDirection Direction { get => movingDirection; }

        private void Awake()
        {
            rb = this.GetComponentOnRoot<Rigidbody2D>();
            col = this.GetComponentOnRoot<Collider2D>();
        }

        private void FixedUpdate()
        {
            if (CanMove)
            {
                TestGrounded();
                UpdateMoving(inputX);
            } else
            {
                UpdateMoving(0);
            }
        }

        private void TestGrounded()
        {
            RaycastHit2D[] hits = new RaycastHit2D[2];
            Vector2 boxSize = new Vector2(col.bounds.size.x - 0.01f, 0.1f);
            int overlaps = Physics2D.BoxCastNonAlloc(col.bounds.center, boxSize,
                0, Vector2.down, hits, col.bounds.extents.y, GameLayers.ground);
            OnGround = overlaps > 0;
        }

        private void UpdateMoving(float x)
        {
            rb.velocity = new Vector2(x, rb.velocity.y);
            CheckFriction();
        }

        private void CheckFriction()
        {
            if (movingDirection != 0 || !OnGround)
            {
                rb.sharedMaterial = data.noFriction;
            }
            else
            {
                rb.sharedMaterial = data.fullFriction;
            }
        }

        public void UpdateInputX() {
            UpdateInputX(data.speed);
        }

        public void UpdateInputX(float desiredSpeed)
        {
            float axis = Input.GetAxis("Horizontal");
            inputX = axis * desiredSpeed;
            if (axis == 0)
            {
                movingDirection = MovingDirection.idle;
            } else
            {
                movingDirection = (MovingDirection) Mathf.Sign(axis);
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
                    lastDirection = MovingDirection.left;
                    break;
                case MovingDirection.right:
                    scale.x = Mathf.Abs(scale.x);
                    lastDirection = MovingDirection.right;
                    break;
            }
            t.localScale = scale;
        }
    }

}
