using AncientForgeQuest.Inventories;
using UnityEngine;

namespace AncientForgeQuest.Models
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Models/Recipe")]
    public class RecipeModel : Model
    {
        [Header("Values")]
        [SerializeField] private ItemBag[] _requiredItems;
        [SerializeField] private ItemBag _resultItem;
        [SerializeField, Tooltip("Enter duration in seconds.")] private int _duration;
        [SerializeField, Range(0, 1)] private float _baseSuccessRate;
        
        public ItemBag[] RequiredItems
        {
            get => _requiredItems;
        }

        public ItemBag ResultItem
        {
            get => _resultItem;
        }

        public float Duration
        {
            get => _duration;
        }

        public float BaseSuccessRate
        {
            get => _baseSuccessRate;
        }
    }
}
