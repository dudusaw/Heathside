using System;
using UnityEngine;

namespace Game.Animation
{
    public class AnimationEventsDelegate : MonoBehaviour
    {
        public event Action hitEvent;

        /// <summary>
        /// Called on animation event inside unity
        /// </summary>
        public void HitEvent()
        {
            if (hitEvent != null)
            {
                hitEvent.Invoke();
            }
        }
    }
}
