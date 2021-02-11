using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heathside.Control
{
    public class PlayerCombat
    {

        private List<IStateBehavior> behaviors;

        public PlayerCombat(MonoBehaviour root)
        {
            IStateBehavior[] array = root.GetComponentsInChildren<IStateBehavior>();
            behaviors = new List<IStateBehavior>(array);
        }

        public void UpdateStates()
        {
            foreach (var item in behaviors)
            {
                if (item.IsActive && !item.Interruptible)
                {
                    item.StateUpdate(() => { });
                    return;
                }
            }

            foreach (var item in behaviors)
            {
                Action interruptionCallback = GetInterruptionCallback(item);
                item.StateUpdate(interruptionCallback);
            }
        }

        private Action GetInterruptionCallback(IStateBehavior item)
        {
            return () =>
            {
                foreach (var interruptCandidate in behaviors)
                {
                    if (interruptCandidate != item && interruptCandidate.IsActive && interruptCandidate.Interruptible)
                    {
                        interruptCandidate.Interrupt();
                    }
                }
            };
        }

        public bool IsActiveAny()
        {
            foreach (var item in behaviors)
            {
                if (item.IsActive)
                {
                    return true;
                }
            }
            return false;
        }
    }
}