using UnityEngine;

namespace Heathside
{
    public static class Utils
    {
        /// <summary>
        /// Shorthand for transform.root.GetComponent<>() with presence check. Logs a warning if return value is null
        /// </summary>
        public static T GetComponentOnRoot<T>(this Component obj)
        {
            T comp = obj.transform.root.GetComponent<T>();
            if (comp == null)
            {
                Debug.LogWarning($"Component wasn't found on object {obj} root");
            }
            return comp;
        }

        /// <summary>
        /// Logs an error if one of provided components is null.
        /// </summary>
        /// <returns>true if all of the objects are not null</returns>
        public static bool CheckComponents(params Object[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                Object item = objects[i];
                if (item == null)
                {
                    Debug.LogError($"Missing component {i}");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compares the distance between two vectors with value
        /// </summary>
        /// <returns>1 if distance is greater than value, -1 if less, 0 if equal</returns>
        public static int CompareDistance(Vector2 v1, Vector2 v2, float valueToCompare)
        {
            float sqrMag = (v1 - v2).sqrMagnitude;
            float sqrVal = valueToCompare * valueToCompare;
            if (sqrMag > sqrVal)
            {
                return 1;
            }
            else if (sqrMag < sqrVal)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public static void SetPos(Transform o, float x, float y, float z)
        {
            Vector3 sc = o.position;
            sc.Set(x, y, z);
            o.position = sc;
        }

        public static void SetPos(Transform o, float val)
        {
            Vector3 sc = o.position;
            sc.Set(val, val, val);
            o.position = sc;
        }

        public static void SetScale(Transform o, float x, float y, float z)
        {
            Vector3 sc = o.localScale;
            sc.Set(x, y, z);
            o.localScale = sc;
        }

        public static void SetScale(Transform o, float val)
        {
            Vector3 sc = o.localScale;
            sc.Set(val, val, val);
            o.localScale = sc;
        }

        public static float EaseIn(float u, float eMod = 2)
        {
            return Mathf.Pow(u, eMod);
        }

        public static float EaseOut(float u, float eMod = 2)
        {
            return 1 - Mathf.Pow(1 - u, eMod);
        }

        public static float EaseInOut(float u, float eMod = 2)
        {
            if (u <= 0.5f)
            {
                return 0.5f * Mathf.Pow(u * 2, eMod);
            }
            else
            {
                return 0.5f + 0.5f * (1 - Mathf.Pow(1 - (2 * (u - 0.5f)), eMod));
            }
        }
    }
}