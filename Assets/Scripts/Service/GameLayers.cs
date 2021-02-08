using UnityEngine;

namespace Game
{
    public static class GameLayers
    {
        public static readonly int Ground;
        public static readonly int Hittable;

        static GameLayers()
        {
            Ground = GetLayer("Ground");
            Hittable = GetLayer("Hittable");
        }

        private static int GetLayer(string name)
        {
            int layer = LayerMask.NameToLayer(name);
            if (layer < 0)
            {
                Debug.LogError($"No such layer: {name}");
            }
            return 1 << layer;
        }
    }
}