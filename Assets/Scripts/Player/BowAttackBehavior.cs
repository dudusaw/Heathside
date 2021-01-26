using Game.Animation;
using UnityEngine;

namespace Game.Control
{
    public class BowAttackBehavior : MonoBehaviour, IStateBehavior
    {
        private Animator anim;
        private SpriteRenderer rend;
        private bool active;

        private void Start()
        {
            rend = GetComponent<SpriteRenderer>();
            rend.enabled = false;
        }

        public bool IsActive => active;

        public bool Interruptible => false;

        public MovementAbility Movement => new MovementAbility(true);

        public void Construct(Animator anim)
        {
            this.anim = anim;
        }

        public void StateUpdate()
        {
            if (active && !Input.GetMouseButton(0))
            {
                active = false;
                anim.Play(AnimatorArgs.Player_idle);
                rend.enabled = false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                active = true;
                anim.Play(AnimatorArgs.Player_bow_stand);
                rend.enabled = true;
            }

            if (active)
            {
                Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dis = point - new Vector2(transform.position.x, transform.position.y);
                float ang = Vector2.SignedAngle(Vector2.down, dis);
                Debug.Log(ang);
                Debug.DrawRay(transform.position, dis);
                Vector3 rot = new Vector3();
                rot.z = ang - 225;
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Sign(transform.root.transform.localScale.x) * Mathf.Abs(scale.x);
                transform.localScale = scale;
                transform.rotation = Quaternion.Euler(rot);
            }
        }

        public void Interrupt()
        {
            throw new System.NotImplementedException();
        }
    }
}