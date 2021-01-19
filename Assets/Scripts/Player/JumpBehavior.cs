using Game.Animation;
using System;
using System.Collections;
using UnityEngine;


namespace Game.Control
{
    public class JumpBehavior : MonoBehaviour
    {
        [SerializeField]
        PlayerData data;

        bool jumpCoroStarted;
        Func<bool> onGround;

        Rigidbody2D rb;
        Animator anim;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = this.GetComponentOnRoot<Animator>();
        }

        public void Construct(Func<bool> onGround)
        {
            this.onGround = onGround;
        }

        public void CheckJump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (onGround())
                {
                    PerformJump();
                }
                else if (!jumpCoroStarted)
                {
                    StartCoroutine(TimeDelayedJump(data.preJumpTime));
                }
            }
        }

        private void PerformJump()
        {
            rb.velocity = new Vector2(rb.velocity.x, data.jumpForce);
            anim.Play(AnimatorArgs.Player_jump);
        }

        // These two functions below allow the player to press jump slightly before he lands, 
        // and then actually jump when he can with a specified delay either in frames or seconds.
        private IEnumerator FrameDelayedJump(int frameCount)
        {
            jumpCoroStarted = true;
            for (int i = 0; i < frameCount; i++)
            {
                yield return null;
                if (onGround())
                {
                    PerformJump();
                    break;
                }
            }
            jumpCoroStarted = false;
        }

        private IEnumerator TimeDelayedJump(float timeToWait)
        {
            jumpCoroStarted = true;
            while (timeToWait > 0)
            {
                yield return null;
                timeToWait -= Time.deltaTime;
                if (onGround())
                {
                    PerformJump();
                    break;
                }
            }
            jumpCoroStarted = false;
        }
    }
}

