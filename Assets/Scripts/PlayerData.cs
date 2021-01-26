using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField]
        public PhysicsMaterial2D noFriction;

        [SerializeField]
        public PhysicsMaterial2D fullFriction;

        [SerializeField]
        public GameObject startAttack;

        [SerializeField, Tooltip("Should be negative")]
        public float fallingEnterVelocity = -20f;

        [SerializeField]
        public float speed = 5f;

        [SerializeField]
        public float speedWhileAttacking = 2f;

        [SerializeField]
        public float healthMaskEasing = 0.05f;

        [SerializeField]
        public float maskSnapping = 0.0025f;

        [SerializeField]
        public float defaultGravity = 5f;

        [SerializeField]
        public float jumpForce = 16f;

        [SerializeField]
        public float preJumpTime = 0.1f;
    }
}