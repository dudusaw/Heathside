using System.Collections;
using UnityEngine;

namespace Game.Control
{
    public class DashExplosionEffect : MonoBehaviour
    {
        [SerializeField] private float maxScale = 2f;
        [SerializeField] private float initialScale = 0.1f;
        [SerializeField] private float speed = 5f;

        private SpriteRenderer mat;
        private static readonly int timeProperty = Shader.PropertyToID("_FadeTime");

        // Start is called before the first frame update
        private void Start()
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