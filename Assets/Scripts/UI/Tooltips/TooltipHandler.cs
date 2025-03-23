using AncientForgeQuest.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AncientForgeQuest.UI.Tooltips
{
    [RequireComponent(typeof(ITooltipProvider))]
    public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private ITooltipProvider _tooltipProvider;

        private void Awake()
        {
            _tooltipProvider = GetComponent<ITooltipProvider>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var content = _tooltipProvider.GetContent();
            TooltipManager.Instance.Show(content);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.Hide();
        }
    }
}
