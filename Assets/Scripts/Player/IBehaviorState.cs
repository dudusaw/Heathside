namespace Heathside.Control
{
    public interface IBehaviorState
    {
        /// <summary>
        /// Called every frame when it should update its state.
        /// It can update its generic state (timing, etc.) in default update.
        /// </summary>
        /// <param name="interruptionCallback">
        /// Call this right before state gets activated to interrupt all other states.
        /// </param>
        void StateUpdate(System.Action interruptionCallback);
        /// <summary>
        /// Is the behavior currently active or not.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Can it be interrupted by other states or not.
        /// </summary>
        bool Interruptible { get; }

        /// <summary>
        /// Interrupt the behavior activity and reset it.
        /// Can be only invoked if the Interruptible of this state is true.
        /// </summary>
        void Interrupt();
    }
}