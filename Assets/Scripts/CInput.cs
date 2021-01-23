using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    /// <summary>
    /// Decorator for default Input class used for blocking some input through ui elements
    /// </summary>
    public static class CInput
    {

        public static bool GetMouseButtonDownNonUI(int button)
        {
            return Input.GetMouseButtonDown(button) && PointerIsNotOverUI();
        }

        public static bool GetMouseButtonNonUI(int button)
        {
            return Input.GetMouseButton(button) && PointerIsNotOverUI();
        }

        private static bool PointerIsNotOverUI()
        {
            return !EventSystem.current.IsPointerOverGameObject();
        }
    }

}
