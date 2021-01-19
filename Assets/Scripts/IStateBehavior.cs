using UnityEngine;

namespace Game.Control
{
    public interface IStateBehavior
    {
        /// <summary>
        /// Called every frame when it should update its state. 
        /// It can update its generic state (timing etc.) in default update.
        /// </summary>
		void StateUpdate();

        /// <summary>
        /// Is the behavior currently active or not.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Indicates the ability to move while active.
        /// </summary>
        MovementAbility Movement { get; }

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

    [System.Serializable]
    public struct MovementAbility
    {
        [SerializeField] bool influenceSpeed;
        [SerializeField] float desiredSpeed;

        /// <summary>
        /// true if object that has the behavior should change the speed to the desired.
        /// If false, the speed of an object will not be affected by this behavior
        /// </summary>
        public readonly bool InfluenceSpeed { get => influenceSpeed; }
        /// <summary>
        /// Controls the speed of owner object if InfluenceSpeed is true
        /// </summary>
        public readonly float DesiredSpeed { get => desiredSpeed; }

        public MovementAbility(bool influence, float desiredSpeed) : this(influence)
        {
            this.desiredSpeed = desiredSpeed;
        }

        /// <summary>
        /// If influence set to true and desired speed is not set (set to 0) then object will not move
        /// </summary>
        /// <param name="influence"></param>
        public MovementAbility(bool influence) : this()
        {
            influenceSpeed = influence;
        }
    }
}
