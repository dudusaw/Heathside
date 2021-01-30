using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerData data;

        private Rigidbody2D rb;
        private Animator anim;
        private Combat combat;
        private MovementBehavior movementBehavior;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            rb.gravityScale = data.defaultGravity;
            movementBehavior = new MovementBehavior(rb, anim, data);
            combat = new Combat(this);
        }

        private void FixedUpdate()
        {
            movementBehavior.MovementFixedUpdate();
        }

        private void Update()
        {
            MovementAbility movementAbility = combat.UpdateStates();
            movementBehavior.InputUpdate(this, movementAbility);
            UpdateAnimations();

            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.position = Vector3.zero;
            }
        }

        private void UpdateAnimations()
        {
            bool onGround = movementBehavior.OnGround;
            anim.SetBool(PlayerAnimationInts.onGround, onGround);
            if (!combat.IsActiveAny())
            {
                movementBehavior.ScaleFlipFromDirection(transform);

                bool isRunning = movementBehavior.Direction != MovingDirection.idle;
                anim.SetBool(PlayerAnimationInts.isRunning, isRunning);

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
