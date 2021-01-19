using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game.Base
{
    public class EffectsController : MonoBehaviour
    {
        [SerializeField]
        float minBloom = 20f;
        [SerializeField]
        float maxDeviation = 20f;
        [SerializeField]
        float changeSpeed = 1f;

        Volume volume;
        MinFloatParameter bloomIntense;

        void Start()
        {
            volume = GetComponent<Volume>();
            if (volume.sharedProfile.TryGet(out Bloom bloom))
            {
                bloomIntense = bloom.intensity;
            }
        }

        void Update()
        {
            if (bloomIntense != null)
            {
                bloomIntense.value = minBloom + Mathf.PerlinNoise(0, Time.time * changeSpeed) * maxDeviation;
            }
        }
    }
}

