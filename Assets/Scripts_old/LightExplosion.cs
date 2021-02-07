using System.Collections;
using UnityEngine;

namespace Game.Base
{
    public class LightExplosion : MonoBehaviour
    {
        [SerializeField]
        private float speed = 1f;

        [SerializeField]
        private float maxScale = 2f;

        [SerializeField]
        [Range(0.95f, 0.99999f)]
        private float snapStart;

        [SerializeField]
        [Range(2, 4)]
        private int exp;

        private Vector3 initScale;

        private void Start()
        {
            initScale = transform.localScale;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                StopAllCoroutines();
                StartCoroutine(StartExp());
            }
        }

        private IEnumerator StartExp()
        {
            transform.localScale = initScale;
            while (transform.localScale.x <= maxScale)
            {
                float sc = transform.localScale.x;
                float u = sc / maxScale;
                if (u > snapStart)
                {
                    SetScale(maxScale);
                    yield break;
                }
                else
                {
                    u = Utils.EaseOut(u, exp);
                    u *= Time.deltaTime * speed;
                    u = Mathf.Lerp(sc, maxScale, u);
                }

                SetScale(u);
                yield return null;
            }
        }

        private void SetScale(float u)
        {
            Vector3 res = transform.localScale;
            res.Set(u, u, u);
            transform.localScale = res;
        }
    }
}