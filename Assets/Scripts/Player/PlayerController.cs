using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerData data;

        private AnimationHandler anim;
        private Combat combat;

        private MovementBehavior movementBehavior;
    }
}
