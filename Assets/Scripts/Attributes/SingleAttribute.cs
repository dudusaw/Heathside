using System;
using UnityEngine;

namespace Heathside.Attributes
{
    [Serializable]
    public sealed class SingleAttribute
    {
        public event Action OnChanged;
        [SerializeField]
        private float _attribute;
        public float Value
        {
            get => _attribute;
            set
            {
                _attribute = value;
                OnChanged?.Invoke();
            }
        }

        public SingleAttribute(float attribute)
        {
            _attribute = attribute;
        }
    }
}