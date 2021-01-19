using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ScreenCameraSettings : MonoBehaviour
    {
        [SerializeField]
        RenderTexture tex;

        void Start()
        {
            Renderer imageRenderer = GetComponentInChildren<Renderer>();
            Shader.SetGlobalTexture(Shader.PropertyToID("_ScreenTex"), tex);
            float aspect = (float)tex.width / tex.height;
            Vector3 scale = new Vector3(aspect, 1, 1);
            imageRenderer.transform.localScale = scale;
            GetComponent<Camera>().orthographicSize = 0.5f;
        }
    }
}