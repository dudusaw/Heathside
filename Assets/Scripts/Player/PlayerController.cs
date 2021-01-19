using Game.Animation;
using Game.Base;
using Game.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Control
{
	public enum MovingDirection
	{
		idle = 0,
		left = -1,
		right = 1
	}

	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		PlayerData data;

		Rigidbody2D rb;
		Animator anim;
		BaseStats stats;

		MovementBehavior movementBehavior;
		JumpBehavior jumpBehavior;

		List<IStateBehavior> allBehaviors;

		void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
			stats = GetComponent<BaseStats>();
			anim = GetComponent<Animator>();

			ConstructBehaviors();

			rb.gravityScale = data.defaultGravity;
		}

		private void ConstructBehaviors()
		{
			IStateBehavior attack = GetComponentInChildren<SpearAttackBehavior>();
			if (attack == null)
            {
				attack = new BehaviorBase();
            }

			MovementBehavior move = GetComponent<MovementBehavior>();
			movementBehavior = move;

			JumpBehavior jump = GetComponent<JumpBehavior>();
			jump.Construct(() => move.OnGround);
			jumpBehavior = jump;

			DashBehavior dash = GetComponentInChildren<DashBehavior>();

			allBehaviors = new List<IStateBehavior>();
			allBehaviors.Add(attack);
			allBehaviors.Add(dash);
			Utils.CheckComponents(dash, move, jump);
		}

		void Update()
		{
			UpdateSkills();
			KeysForTesting();
			UpdateAnimations();
		}

		private void UpdateSkills()
		{
			bool influenceSpeed = false;
			float desiredSpeed = 0;
			foreach (var item in allBehaviors)
			{
				if (item.IsActive)
				{
					if (item.Movement.InfluenceSpeed)
                    {
						influenceSpeed = true;
						desiredSpeed = item.Movement.DesiredSpeed;
					}
					if (!item.Interruptible)
					{
						UpdateMoving(influenceSpeed, desiredSpeed);
						item.StateUpdate();
						return;
					}
				}
			}

			UpdateMoving(influenceSpeed, desiredSpeed);

			foreach (var item in allBehaviors)
			{
				item.StateUpdate();
			}
		}

		private void UpdateMoving(bool influenceSpeed, float desiredSpeed)
        {
			bool canMove = !influenceSpeed || desiredSpeed > 0;
			movementBehavior.CanMove = canMove;
			if (influenceSpeed)
            {
				movementBehavior.UpdateInputX(desiredSpeed);
            }
			else
            {
				movementBehavior.UpdateInputX();
			}

			if (canMove)
			{
				jumpBehavior.CheckJump();
			}
		}

		private void KeysForTesting()
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				transform.position = Vector3.zero;
			}
		}

		private bool IsActiveAny()
		{
            foreach (var item in allBehaviors)
            {
				if (item.IsActive)
                {
					return true;
                }
            }
			return false;
		}

		private void UpdateAnimations()
		{
			bool onGround = movementBehavior.OnGround;
			if (!IsActiveAny())
			{
				movementBehavior.ScaleFlipFromDirection(transform);

				if (movementBehavior.Direction == MovingDirection.idle)
				{
					anim.SetBool(AnimatorArgs.isRunning, false);
				} else
				{
					anim.SetBool(AnimatorArgs.isRunning, true);
				}

				if (!onGround && rb.velocity.y < data.fallingEnterVelocity 
					&& anim.GetCurrentAnimatorStateInfo(0).shortNameHash != AnimatorArgs.Player_fall)
				{
					anim.Play(AnimatorArgs.Player_fall);
				}
			}

			anim.SetBool(AnimatorArgs.onGround, onGround);
		}
	}
}
