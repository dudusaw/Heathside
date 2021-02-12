using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heathside.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerData data;

        private Rigidbody2D rb;
        private Animator anim;
        private PlayerCombat combat;
        private MovementBehavior movementBehavior;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            movementBehavior = GetComponent<MovementBehavior>();
            rb.gravityScale = data.defaultGravity;
            combat = new PlayerCombat(this);
        }

        private void FixedUpdate()
        {
            movementBehavior.MovementFixedUpdate();
        }

        private void Update()
        {
            combat.UpdateStates();
            movementBehavior.InputUpdate();
            UpdateAnimations();

            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.position = Vector3.zero;
            }
        }

        private void UpdateAnimations()
        {
            bool onGround = movementBehavior.OnGround;
            bool isRunning = movementBehavior.Direction != MovingDirection.idle;
            anim.SetBool(PlayerAnimationInts.onGround, onGround);
            anim.SetBool(PlayerAnimationInts.isRunning, isRunning);

            movementBehavior.ScaleFlipFromDirection(transform);

            if (!combat.IsActiveAny())
            {
                FallingCheck(onGround);
            }
        }

        private void FallingCheck(bool onGround)
        {
            if (!onGround && rb.velocity.y < data.fallingEnterVelocity
                                && anim.GetCurrentAnimatorStateInfo(0).shortNameHash != PlayerAnimationInts.Player_fall)
            {
                anim.Play(PlayerAnimationInts.Player_fall);
            }
        }
    }
}
