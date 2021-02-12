using UnityEngine;

namespace Heathside.Attributes
{
    [CreateAssetMenu(menuName = "Game/Attributes/MovementAttributes")]
    public class MovementAttributes : ScriptableObject
    {
        [SerializeField] private float speed;

        [SerializeField] private float activeSpeed;
        [SerializeField] private float defaultSpeed;
        public float ActiveSpeed { get => activeSpeed; set => activeSpeed = value; }
        public float DefaultSpeed { get => defaultSpeed; set => defaultSpeed = value; }
        public void RestrictMovement()
        {
            activeSpeed = 0;
        }

        public void RestoreMovement()
        {
            activeSpeed = defaultSpeed;
        }
    }
}