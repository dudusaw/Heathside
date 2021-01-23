using UnityEngine;

namespace Game.Control
{
    [CreateAssetMenu(menuName = "Game/DashData")]
    public class DashData : ScriptableObject
    {
        [Header("Default properties")]
        [SerializeField] public DashExplosionEffect explosion;
        [SerializeField] public PhysicsMaterial2D noFriction;
        [SerializeField] public float maxTimeBetweenPresses = 0.2f;
        [SerializeField] public float dashTime = 0.4f;
        [SerializeField] public float minDashSpeed = 5f;
        [SerializeField] public float maxDashSpeed = 30f;
        [SerializeField] public float impulseAmp;
        [SerializeField] public float impulseFreq;
        

        [Header("Collision resolving")]
        [SerializeField] public float castDistance = 2f;
        [SerializeField] public float yResolvingVelocity = 10f;
        [SerializeField] public float colliderEdge = 0.05f;
        [SerializeField] [Range(0, 1)] public float castPercent = 0.2f;
    }
}
