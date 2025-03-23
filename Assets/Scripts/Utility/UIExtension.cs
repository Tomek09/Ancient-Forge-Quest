using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AncientForgeQuest.Utility
{
    public static class UIExtension
    {
        public static void Toggle(this CanvasGroup canvasGroup, bool value)
        {
            canvasGroup.SetAlpha(value);
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }

        public static void SetAlpha(this CanvasGroup canvasGroup, bool value)
        {
            canvasGroup.alpha = value ? 1f : 0f;
        }

        public static bool TryGetUIGameObject(out GameObject target)
        {
            var eventSystem = EventSystem.current;
            PointerEventData pointerEventData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };
            
            var results = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerEventData, results);

            target = results.Count > 0 ? results[0].gameObject : null;
            return target != null;
        }
        
        public static bool TryGetUIComponent<T>(out T target)
        {
            target = default;
            return TryGetUIGameObject(out var gameObject) && gameObject.TryGetComponent(out target);
        }
    }
}
