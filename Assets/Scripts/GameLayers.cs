using UnityEngine;

namespace Game.Base
{
    public static class GameLayers
    {
        public static int ground;
        public static int hittable;

        static GameLayers()
        {
            ground = 1 << LayerMask.NameToLayer("Ground");
            hittable = 1 << LayerMask.NameToLayer("Hittable");
        }
    }
}
