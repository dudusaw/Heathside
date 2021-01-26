using Game.Items;
using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(InventoryManager))]
    public class InventoryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var manager = (InventoryManager)target;

            GUILayout.Space(10f);
            if (GUILayout.Button("Update icons"))
            {
                var slots = manager.GetComponentsInChildren<InventorySlot>();
                foreach (var item in slots)
                {
                    item.UpdateIcon();
                }
            }
        }
    }
}