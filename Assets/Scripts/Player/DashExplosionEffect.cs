using Game.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Control
{
    public class DashExplosionEffect : MonoBehaviour
    {
        [SerializeField] float maxScale = 2f;
        [SerializeField] float initialScale = 0.1f;
        [SerializeField] float speed = 5f;

        SpriteRenderer mat;
        static readonly int timeProperty = Shader.PropertyToID("_FadeTime");

        // Start is called before the first frame update
        void Start()
        {
            mat = GetComponent<SpriteRenderer>();
        }

        public void StartExplosion()
        {
            StartCoroutine(Explode());
        }

        private IEnumerator Explode()
        {
            float maxScaleOffset = 0.01f;
            Utils.SetScale(transform, initialScale);
            while (transform.localScale.x < maxScale - maxScaleOffset)
            {
                yield return null;
                float curSc = transform.localScale.x;
                float scale = Mathf.Lerp(curSc, maxScale, speed * Time.deltaTime);
                float value = scale / maxScale;
                mat.material.SetFloat(timeProperty, value);
                Utils.SetScale(transform, scale);
            }
            Destroy(gameObject);
        }
    }
}

