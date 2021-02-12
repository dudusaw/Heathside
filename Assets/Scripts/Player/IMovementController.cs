using UnityEngine;

namespace Heathside.Control
{
    public interface IMovementController
    {
        MovingDirection Direction { get; }
        bool OnGround { get; }
        bool CanMove { get; set; }
    }
}