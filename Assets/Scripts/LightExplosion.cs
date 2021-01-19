using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Base
{
    public class LightExplosion : MonoBehaviour
    {
        [SerializeField]
        float speed = 1f;
        [SerializeField]
        float maxScale = 2f;

        [SerializeField]
        [Range(0.95f, 0.99999f)]
        float snapStart;

        [SerializeField]
        [Range(2, 4)]
        int exp;

        Vector3 initScale;

        private void Start()
        {
            initScale = transform.localScale;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                StopAllCoroutines();
                StartCoroutine(StartExp());
            }
        }

        IEnumerator StartExp()
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

        void SetScale(float u)
        {
            Vector3 res = transform.localScale;
            res.Set(u, u, u);
            transform.localScale = res;
        }
    }
}
