using UnityEngine;

namespace Heathside
{
    public static class GameLayers
    {
        public static readonly int Ground;
        public static readonly int Entity;

        static GameLayers()
        {
            Ground = GetLayer("Ground");
            Entity = GetLayer("Entity");
        }

        public static int Group(params int[] layers)
        {
            int result = 0;
            foreach (var layer in layers)
            {
                result |= layer;
            }
            return result;
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