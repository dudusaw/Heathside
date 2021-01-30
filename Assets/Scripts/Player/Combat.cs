using System.Collections.Generic;
using UnityEngine;

namespace Game.Control
{
    public class Combat
    {

        private List<IStateBehavior> behaviors;
        private bool active;

        public Combat(MonoBehaviour root)
        {
            IStateBehavior[] array = root.GetComponentsInChildren<IStateBehavior>();
            behaviors = new List<IStateBehavior>(array);
        }

        public MovementAbility UpdateStates()
        {
            active = false;
            foreach (var item in behaviors)
            {
                if (item.IsActive && !item.Interruptible)
                {
                    active = true;
                    item.StateUpdate();
                    return item.Movement;
                }
            }

            foreach (var item in behaviors)
            {
                bool wasInactive = item.IsActive == false;
                item.StateUpdate();
                if (wasInactive && item.IsActive)
                {
                    foreach (var item2 in behaviors)
                    {
                        if (item2 != item && item2.IsActive && item2.Interruptible)
                        {
                            item2.Interrupt();
                        }
                    }
                }
                if (item.IsActive)
                {
                    active = true;
                    return item.Movement;
                }
            }
            return new MovementAbility(false);
        }

        public bool IsActiveAny()
        {
            return active;
        }
    }
}