using AncientForgeQuest.Instances;
using AncientForgeQuest.UI.Tooltips;
using AncientForgeQuest.Utility;
using UnityEngine;
namespace AncientForgeQuest.Managers
{
    public class TooltipManager : Singleton<TooltipManager>
    {
        [Header("Components")]
        [SerializeField] private TooltipContentView _tooltip;

        public void Show(TooltipContent content)
        {
            if (string.IsNullOrWhiteSpace(content.Header))
                return;
            
            _tooltip.Bind(content);
            _tooltip.Show();
        }

        public void Hide()
        {
            _tooltip.Hide();
        }
    }
}
