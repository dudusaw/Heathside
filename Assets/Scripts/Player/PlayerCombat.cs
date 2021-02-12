using System;
using System.Collections.Generic;
using UnityEngine;
using Heathside.Attributes;

namespace Heathside.Control
{
    public class PlayerCombat
    {

        private List<IBehaviorState> behaviors;

        public PlayerCombat(MonoBehaviour root)
        {
            IBehaviorState[] array = root.GetComponentsInChildren<IBehaviorState>();
            behaviors = new List<IBehaviorState>(array);
        }

        public void UpdateStates()
        {
            if (UpdateNonInterruptibles())
            {
                return;
            }

            UpdateAll();
        }

        private bool UpdateNonInterruptibles()
        {
            foreach (var item in behaviors)
            {
                if (item.IsActive && !item.Interruptible)
                {
                    item.StateUpdate(() => { });
                    return true;
                }
            }
            return false;
        }

        private void UpdateAll()
        {
            foreach (var item in behaviors)
            {
                Action interruptionCallback = GetInterruptionCallback(item);
                item.StateUpdate(interruptionCallback);
            }
        }

        private Action GetInterruptionCallback(IBehaviorState item)
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