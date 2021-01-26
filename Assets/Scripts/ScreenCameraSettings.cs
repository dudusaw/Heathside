using UnityEngine;

namespace Game
{
    public class ScreenCameraSettings : MonoBehaviour
    {
        [SerializeField]
        private RenderTexture tex;

        private void Start()
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