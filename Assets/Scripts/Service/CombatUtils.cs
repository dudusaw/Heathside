using Heathside.Control;
using System;
using UnityEngine;

namespace Heathside
{
    public static class CombatUtils
    {
        private static ContactFilter2D filter;
        private static readonly Collider2D[] results;

        static CombatUtils()
        {
            filter = new ContactFilter2D();
            filter.SetLayerMask(GameLayers.Entity);
            filter.useTriggers = true;
            results = new Collider2D[20];
        }
        
        public static void ActOnAllOverlapsOneTime<T>(Collider2D collider, Action<T> action)
        {
            int count = collider.OverlapCollider(filter, results);
            for (int i = 0; i < count; i++)
            {
                T receiver = results[i].GetComponent<T>();
                if (receiver != null)
                {
                    action(receiver);
                }
            }
            Array.Clear(results, 0, results.Length);
        }
    }
}