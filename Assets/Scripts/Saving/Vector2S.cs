using System;
using UnityEngine;

namespace Game.Saving
{
    [Serializable]
    /// <summary>
    /// Just a serializable vector
    /// </summary>
    public struct Vector2S
    {
        public float x { get; set; }
        public float y { get; set; }

        public Vector2S(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2 ToVec2()
        {
            return new Vector2(x, y);
        }
    }
}