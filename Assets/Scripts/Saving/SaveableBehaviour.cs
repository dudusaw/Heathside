using UnityEngine;

namespace Game.Saving
{
    public abstract class SaveableBehaviour : MonoBehaviour, ISaveable, ISerializationCallbackReceiver
    {
        public abstract object SavedData { get; }
        public abstract void LoadFromData(object data);

        public string SaveID
        {
            get
            {
                return _saveID;
            }
            private set
            {
                _saveID = value;
            }
        }

        [HideInInspector]
        [SerializeField]
        private string _saveID;

        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
            if (_saveID == null)
            {
                _saveID = System.Guid.NewGuid().ToString();
            }
        }
    }
}

