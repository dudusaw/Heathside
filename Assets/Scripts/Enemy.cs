using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heathside.Control
{
    public class Enemy : MonoBehaviour
    {
        public float Health = 50f;

        public void TakeDamage(float incomingDamage)
        {
            Debug.Log("dagame taken");
            Health -= incomingDamage;
        }
    }
}
