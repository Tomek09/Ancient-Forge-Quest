using AncientForgeQuest.Instances;
using UnityEngine;

namespace AncientForgeQuest.UI.Tooltips
{
    public class SimpleTooltipProvider : MonoBehaviour, ITooltipProvider
    {
        [Header("Settings")]
        [SerializeField] private string _header;
        [SerializeField] private string _description;

        public TooltipContent GetContent()
        {
            return new TooltipContent(_header, _description);
        }
    }
}
