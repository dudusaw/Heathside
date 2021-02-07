using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game.Base
{
    public class EffectsController : MonoBehaviour
    {
        [SerializeField]
        private float minBloom = 20f;

        [SerializeField]
        private float maxDeviation = 20f;

        [SerializeField]
        private float changeSpeed = 1f;

        private Volume volume;
        private MinFloatParameter bloomIntense;

        private void Start()
        {
            volume = GetComponent<Volume>();
            if (volume.sharedProfile.TryGet(out Bloom bloom))
            {
                bloomIntense = bloom.intensity;
            }
        }

        private void Update()
        {
            if (bloomIntense != null)
            {
                bloomIntense.value = minBloom + Mathf.PerlinNoise(0, Time.time * changeSpeed) * maxDeviation;
            }
        }
    }
}