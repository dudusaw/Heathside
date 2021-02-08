using UnityEngine;

namespace Game.Control
{
    [CreateAssetMenu(menuName = "Game/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField, Tooltip("Should be negative")]
        public float fallingEnterVelocity = -20f;

        [SerializeField]
        public float initialSpeed = 5f;

        [SerializeField]
        public float accelerationValue = 5f;

        [SerializeField]
        public float speedWhileAttacking = 2f;

        [SerializeField]
        public float defaultGravity = 5f;

        [SerializeField]
        public float jumpForce = 16f;

        [SerializeField]
        public float preJumpTime = 0.1f;
    }
}